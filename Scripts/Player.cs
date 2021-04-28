using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    Rigidbody rigid;
    Animator animator;
    InputManager inputManager;
    WeaponManager weaponManager;

    [SerializeField] Slider HPslider; //플레이어 체력 슬라이더
    [SerializeField] GameObject GameEndBG; //게임오버시 나타날 UI백그라운드

    public float speed = 160f;
    bool isPlayerDead = false;

    float recoverHPDelay = 3; //체력회복 대기시간

    int groundLayer = 0;

    protected override void Start()
    {
        isPlayerDead = false;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        speed = EntityData.PLAYER_SPEED;
        setHealthPoint(EntityData.PLAYER_HP);
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        setHealthPoint(EntityData.PLAYER_HP);
        HPslider.maxValue = EntityData.PLAYER_HP;
        HPslider.value = EntityData.PLAYER_HP;
        setPlayerInfo();
        StartCoroutine(recoverHP());
    }
    void FixedUpdate()
    {
        if (!isPlayerDead)
        {
            moveEntity();
            lookPoint();
            fireGun();
        }
    }
    //플레이어 움직임
    //자연스러운 움직임과 충돌을 위해 AddForce 사용
    public void moveEntity()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        startRunAni(h, v);
        if (h == 0 && v == 0)
            return;
        //대각선 이동속도가 빠른 문제해결을 위해 대각선 이동시 0.7을 곱해줌
        if (h != 0 && v != 0)
            rigid.AddForce(new Vector3(h * 0.7f * speed, 0, v * 0.7f * speed));
        else
            rigid.AddForce(new Vector3(h * speed, 0, v * speed));
        rigid.velocity = Vector3.zero;
    }
    //플레이어 회전 함수
    public void lookPoint()
    {
        //아침일시 플레이어가 이동하는 방향을 바라보게 함
        if (GameManager.instance.isDay)
        {
            if (inputManager.moveKey())
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");

                Vector3 dir = h * Vector3.right + v * Vector3.forward;
                Quaternion targeRot = Quaternion.LookRotation(dir);
                //쿼터니온 선형 보간으로 자연스러운 회전을 구현
                transform.rotation = Quaternion.Lerp(transform.rotation, targeRot, 0.16f);
            }
        }
        //밤이면 마우스커서 위치를 바라보도록 함
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 9999, groundLayer))
            {
                Quaternion targeRot = Quaternion.LookRotation(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targeRot, 0.18f);
            }
        }
    }
    //이동중일시 플레이어 달리는 애니메이션 실행
    public void startRunAni(float horiz, float verti)
    {
        if(horiz != 0 || verti != 0)
            animator.SetBool("isRun", true);
        else
            animator.SetBool("isRun", false);
    }
    //무기 매니저의 발사 함수를 실행
    private void fireGun()
    {
        if (!inputManager.fire())
            return;
        weaponManager.shoot();
    }
    //무기 교체 애니메이션 실행 매개변수가 0이면 권총 1이면 소총류
    public void startWeaponSwapAni(int UpperMotion)
    {
        animator.SetTrigger("startSwap");
        animator.SetInteger("weaponState", UpperMotion);
    }
    //사망 함수
    protected override void dieEntity()
    {
        if (!isPlayerDead && currentHealhPoint <= 0)
        {
            animator.SetTrigger("Dead");
            isPlayerDead = true;
            //애니메이션 카운트를 늘림으로써 아침이 될 수 없도록 함
            EnemyChecker.enemyCount = 9999;
            GameEndBG.SetActive(true);
        }
    }
    //피격함수
    public void hit(int size)
    {
        decreaseHealthPoint(size);
        HPslider.value = currentHealhPoint;
        dieEntity();
    }
    //플레이어의 이동속도, 체력, 회복속도와 같은 정보를 업데이트
    //각 곱하기율과 뺄셈율을 적용해줌
    public void setPlayerInfo()
    {
        speed = EntityData.PLAYER_SPEED * StateManager.playerSpeedMult;
        setHealthPoint((int)(EntityData.PLAYER_HP * StateManager.playerHPMult));
        recoverHPDelay = 3 - StateManager.playerRecoverMinus;
        HPslider.maxValue = defaultHealhPoint;
        HPslider.value = currentHealhPoint;
    }
    //체력회복 기능 코루틴
    IEnumerator recoverHP()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (!GameManager.instance.isDay)
            {
                yield return new WaitForSeconds(recoverHPDelay);
                if (defaultHealhPoint > currentHealhPoint)
                {
                    currentHealhPoint += 1; //1씩 회복
                    HPslider.value = currentHealhPoint;
                }
            }
            yield return new WaitForSeconds(recoverHPDelay);
        }
    }
}
