using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   //this is used at GameScene
    public static int healthPoint = 3;

    float jumpSpeed = 300;

    bool canAction = false;
    bool isAvoid = false;

    string obstacleName;

    public Animator animator_Spring;
    public Animator animator_Summer;
    public Animator animator_Autumn;
    public Animator animator_Winter;

    Animator animator_Current = null;

    public GameObject[] healthImage = new GameObject[3];

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthPoint = 3;
        checkStage();
    }
    void checkStage() //현재 애니메이션에 스테이지에 따른 캐릭터 애니메이션 대입
    {
        switch (Manager.instance.selectedStage)
        {
            case 1: animator_Current = animator_Spring; break;
            case 2: animator_Current = animator_Summer; break;
            case 3: animator_Current = animator_Autumn; break;
            case 4: animator_Current = animator_Winter; break;
            default: Debug.Log("Failed to load stage animation"); break;
        }
    }
    void jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed, ForceMode2D.Force);
        showJumpAnimation();
    }
    void sliding()
    {
        showSlidingAnimation();
    }
    void hit()
    {
        showHitAnimation();
        decreaseHealth();
        gameManager.runFailure();
    }
    void showJumpAnimation()
    {
        animator_Current.SetTrigger("jump");
    }
    void showSlidingAnimation()
    {
        animator_Current.SetTrigger("sliding");
    }
    void showHitAnimation()
    {
        animator_Current.SetTrigger("hit");
    }
    void enterObstacle() //장애물 범위 진입 시
    {
        canAction = true;
        isAvoid = false;
    }
    void exitObstacle() //장애물 범위 나갈 시
    {
        canAction = false;
        isAvoid = false;
    }
    //체력 감소
    void decreaseHealth()
    {
        if (0 < healthPoint)
        {
            healthImage[healthPoint - 1].SetActive(false);
            healthPoint--;
        }
    }
    //장애물 정보 수정
    void setObstacle(Collider2D obstacle_)
    {
        obstacleName = obstacle_.GetComponent<Obstacle>().objectName;
        obstacle_.GetComponent<Obstacle>().setColor();
    }
    //장애물 범위에 들어 왔을 시
    void OnTriggerEnter2D(Collider2D obstacle)
    {
        if (obstacle.CompareTag("Obstacle"))
        {
            setObstacle(obstacle);
            enterObstacle();
            StartCoroutine(checkAvoid());
        }
    }
    //장애물이 범위 안에서 일정시간 버튼 입력 감지
    IEnumerator checkAvoid()
    {
        var wait = new WaitForSeconds(0.08f);
        for (int Index = 0; Index < 10; Index++)
        {
            yield return wait;
            if (isAvoid)
            {
                exitObstacle();
                yield break;
            }
        }
        //버튼을 누르지 못했다는 것으로 간주
        Debug.Log("HIT!");
        exitObstacle();
        hit();
    }

    //봄
    public void actionButton_Spring_0()
    {
        if (!canAction) 
            return;
        if (obstacleName == "s_pool")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Spring_1()
    {
        if (!canAction)
            return;
        if (obstacleName == "s_flower")
        {
            isAvoid = true;
            jump();
        }
    }
    //여름
    public void actionButton_Summer_0()
    {
        if (!canAction)
            return;
        if (obstacleName == "sm_tube")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Summer_1()
    {
        if (!canAction)
            return;
        if (obstacleName == "sm_warermelon")
        {
            isAvoid = true;
            jump();
        }
    }
    //가을
    public void actionButton_Autumn_0()
    {
        if (!canAction)
            return;
        if (obstacleName == "a_wood")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Autumn_1()
    {
        if (!canAction)
            return;
        if (obstacleName == "a_chestnut")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Autumn_2()
    {
        if (!canAction)
            return;
        if (obstacleName == "a_spider")
        {
            isAvoid = true;
            sliding();
        }
    }
    //겨울
    public void actionButton_Winter_0()
    {
        if (!canAction)
            return;
        if (obstacleName == "w_ice")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Winter_1()
    {
        if (!canAction)
            return;
        if (obstacleName == "w_snowman")
        {
            isAvoid = true;
            jump();
        }
    }
    public void actionButton_Winter_2()
    {
        if (!canAction)
            return;
        if (obstacleName == "w_branch")
        {
            isAvoid = true;
            sliding();
        }
    }
}
