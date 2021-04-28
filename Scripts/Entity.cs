using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int defaultHealhPoint; //개체 기본 체력
    protected int currentHealhPoint; //개체 현재 체력

    int dieDelaySecond = 5; //사망 대기시간

    protected virtual void Start()
    {

    }
    //현재 체력을 매개변수로 받은 데미지만큼 빼줌
    protected void decreaseHealthPoint(int dmg_size)
    {
        currentHealhPoint -= dmg_size;
    }
    //개체 사망 기본 기능 함수
    protected virtual void dieEntity()
    {
        if(currentHealhPoint <= 0)
        {
            Invoke("delayDie", dieDelaySecond);
        }
    }
    //개체 체력 업데이트
    protected void setHealthPoint(int HP)
    {
        defaultHealhPoint = HP;
        currentHealhPoint = defaultHealhPoint;
    }
    //딜레이로 실행될 사망기능
    void delayDie()
    {
        gameObject.SetActive(false);
    }
}
public class EntityData
{
    //Entity 초기 정보
    public const int PLAYER_HP = 200;
    public const int ZOMBIE_HP = 30;
    public const int BOSS_HP = 200;

    public const int ZOMBIE_DAMAGE = 10;
    public const int BOSS_DAMAGE = 30;

    public const float ZOMBIE_ATTACK_RATE = 2.3f;
    public const float BOSS_ATTACK_RATE = 3;

    public const int PLAYER_SPEED = 160;
    public const float ZOMBIE_SPEED = 2f;
    public const float BOSS_SPEED = 3f;

    public const float PLAYER_RECOVER_DELAY = 3;
}
