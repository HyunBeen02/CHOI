using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    Animator animator;
    GameObject currentTarget;
    LayerMask TargetLayer;
    float findDelay = 0; //적 찾기 딜레이
    float closeDistance = 0;
    float attackDistance = 0;
    bool canAttack = true;
    bool isDead = false;

    int currentDamage = 0;
    int currentHP = 0;
    float currentSpeed = 0;
    float currentAttackRate = 0;

    int currentScorePlusValue = 0;
    int currentMoneytPlusValue = 0;

    string thisTag = ""; //자신의 태그

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("CycleOffset", Random.Range(0, 1f));
        TargetLayer = 1 << LayerMask.NameToLayer("Team");
        findDelay = 1f;
        closeDistance = 1.2f;
        attackDistance = 1.5f;
        findTarget();
        setHealthPoint(currentHP);
        isDead = false;
        setZombieData();
        thisTag = gameObject.transform.tag;
    }
    void Update()
    {
        if (!isDead)
        {
            rotateToTarget();
            moveToTarget();
        }
    }
    //활성화시 달리기 애니메이션 실행
    //적 이동속도, 공격력, 체력등 정보를
    //곱하기율을 곱한 값으로 갱신해주고 체력값 갱신
    private void OnEnable()
    {
        GetComponent<Animator>().SetTrigger("Run");
        setZombieData();
        setHealthPoint(currentHP);
    }
    //비활성화시 정보 초기화
    private void OnDisable()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = thisTag == "Zombie" ? "Zombie" : "Boss";
        isDead = false;
        setZombieData();
        setHealthPoint(currentHP);
    }
    //좀비인지 보스인지 구별 후
    //현재 정보에 곱하기율을 곱해 대입
    void setZombieData()
    {
        if (gameObject.CompareTag("Zombie"))
        {
            currentDamage = (int)(EntityData.ZOMBIE_DAMAGE * StateManager.enemyDamageMult);
            currentHP = (int)(EntityData.ZOMBIE_HP * StateManager.enemyHPMult);
            currentSpeed = EntityData.ZOMBIE_SPEED * StateManager.enemySpeedMult;
            currentAttackRate = EntityData.ZOMBIE_ATTACK_RATE;
            //점수는 100점부터 시작해 1일마다 100점의 10%씩 더해줌
            currentScorePlusValue = (int)(100 * (1 + (StateManager.dayCount - 1) * 0.1f));
            //돈은 200원부터 시작해 1일마다 200원의 10%씩 더해줌
            currentMoneytPlusValue = (int)(200 * (1 + (StateManager.dayCount - 1) * 0.1f));
        }
        else if (gameObject.CompareTag("Boss"))
        {
            currentDamage = (int)(EntityData.BOSS_DAMAGE * StateManager.enemyDamageMult);
            currentHP = (int)(EntityData.BOSS_HP * StateManager.enemyHPMult);
            currentSpeed = EntityData.BOSS_SPEED * StateManager.enemySpeedMult;
            currentAttackRate = EntityData.BOSS_ATTACK_RATE;
            //점수는 1000점부터 시작해 1일마다 1000점의 10%씩 더해줌
            currentScorePlusValue = (int)(1000 * (1 + (StateManager.dayCount - 1) * 0.1f));
            //돈은 500원부터 시작해 1일마다 500원의 10%씩 더해줌
            currentMoneytPlusValue = (int)(500 * (1 + (StateManager.dayCount - 1) * 0.05f));
        }
        else
        {
            Debug.Log("Set Data is Failed");
        }
    }
    //타겟을 향해 이동함
    void moveToTarget()
    {
        if (currentTarget == null)
            return;
        //거리구하기
        float dis = Vector3.Distance(transform.position, currentTarget.transform.position);
        //타겟과 일정 거리 밖에있으면 이동함 
        if (dis > closeDistance)
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        //타겟과 일정 거리 안에 있으면 공격
        else
        {
            //공격
            if (canAttack)
            {
                attack();
                canAttack = false;
                startAttackDamageAni();
                Invoke("setTrueAttack", currentAttackRate);
            }
        }
    }
    //어택 가능
    void setTrueAttack()
    {
        canAttack = true;
    }
    //공격 애니메이션 실행 함수
    void startAttackDamageAni()
    {
        animator.SetTrigger("Attack");
    }
    //타겟의 방향으로 회전함
    void rotateToTarget()
    {
        if (currentTarget == null)
            return;
        Vector3 dir = transform.position - currentTarget.transform.position;
        //반대로 회전해서 -1로 보정
        dir = new Vector3(dir.x * -1, 0, dir.z * -1);
        transform.rotation = Quaternion.LookRotation(dir);
    }
    //가장 가까운적을 찾아 현재 타겟에 넣어줌
    void findTarget()
    {
        Collider[] targetCols = Physics.OverlapSphere(transform.position, 100, TargetLayer);
        float dis = 999;
        foreach (Collider col in targetCols)
        {
            float targetDis = Vector3.Distance(transform.position, col.transform.position); //거리가 안 맞으면 수정 here
            if (dis > targetDis)
            {
                dis = targetDis;
                currentTarget = col.gameObject;
            }
        }
        //쓸데없는 실행을 방지하기 위해 함수 실행에 딜레이를 줌
        Invoke("findTarget", findDelay);
    }
    //공격 기능 레이캐스트를 활용해 레이를 맞은 적 피격해줌
    void attack()
    {
        if (currentTarget == null)
            return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, attackDistance, TargetLayer))
        {
            //빌딩일 때
            if (hit.collider.CompareTag("Building"))
            {
                hit.collider.gameObject.GetComponent<Building>().hit(currentDamage);
            }
            //플레이어일 때
            else if (hit.collider.CompareTag("Player"))
                hit.collider.gameObject.GetComponent<Player>().hit(currentDamage);
        }

    }
    //사망 함수
    protected override void dieEntity()
    {
        if (currentHealhPoint <= 0 && !isDead)
        {
            int dieDelay = 5;
            isDead = true;
            //사망 애니메이션 둘 중 하나 실행
            int ran = Random.Range(0, 2);
            if (ran == 0)
                animator.SetTrigger("Death1");
            else
                animator.SetTrigger("Death2");
            //사망 딜레이
            Invoke("die", dieDelay);
            //피 이펙트
            bloodEffect();
            //레이어 데드 상태로 변경
            gameObject.layer = LayerMask.NameToLayer("Dead");
            gameObject.tag = "DeadZombie";
            //점수와 돈을 증가값 만큼 증가 시키고 텍스트 갱신
            StateManager.score += currentScorePlusValue;
            StateManager.money += currentMoneytPlusValue;
            GameManager.instance.stateManager.setScoreText();
            GameManager.instance.stateManager.setMoneyText();
            //적 개수 감소
            EnemyChecker.enemyCount--;
            //적의 수가 0이하면 클리어 실행
            if (EnemyChecker.enemyCount <= 0)
            {
                GameManager.instance.enemyChecker.clearNight();
            }
        }
    }
    //피 이펙트 랜덤 크기와 랜덤 회전값으로 다양하게 표현
    void bloodEffect()
    {
        GameObject parent = GameObject.Find("Bloods");
        float ranRotY = Random.Range(0, 360);
        float ranSclX = Random.Range(0.35f, 0.45f);
        float ranSclY = Random.Range(0.35f, 0.45f);
        //비활성화되어있는 (사용가능한) 오브젝트 찾기
        for (int idx = 0; idx < parent.transform.childCount; idx++)
        {
            if (!parent.transform.GetChild(idx).gameObject.activeSelf)
            {
                //위치와 회전값 조정 후 활성화
                parent.transform.GetChild(idx).position = transform.position + new Vector3(0, 0.01f, 0);
                parent.transform.GetChild(idx).localScale = new Vector3(ranSclX, ranSclY, 1);
                parent.transform.GetChild(idx).rotation = Quaternion.Euler(90, ranRotY, 0);
                parent.transform.GetChild(idx).gameObject.SetActive(true);
                return;
            }
        }
    }
    //사망 비활성화
    void die()
    {
        gameObject.SetActive(false);
    }
    //피격
    public void hit(int dmg_size)
    {
        decreaseHealthPoint(dmg_size);
        dieEntity();
    }
}