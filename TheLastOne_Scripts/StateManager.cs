using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static int money; //돈
    public static int score; //점수
    public static int point; //업그레이드 포인트

    //Mults는 플레이어 또는 적의 정보에 곱해질 값
    public static int dayCount; //일수
    public static float enemyHPMult; //적 체력 곱하기율
    public static float enemyDamageMult; //적 공격력 곱하기율
    public static float enemySpeedMult; //적 이동속도 곱하기율

    public static float playerHPMult; //플레이어 체력 곱하기율
    public static float playerSpeedMult; //플레이어 이동속도 곱하기율
    public static float playerRecoverMinus; //플레이어 회복속도 감소량

    [SerializeField] Text NightCountText; //일수 출력 텍스트
    [SerializeField] Text EnemyHPText; //적 체력 곱하기율 출력 텍스트
    [SerializeField] Text EnemyPowerText; //적 공격력 곱하기율 출력 텍스트
    [SerializeField] Text EnemySpeedText; //적 이동속도 곱하기율 출력 텍스트

    [SerializeField] Text ScoreText; //점수 출력 텍스트
    [SerializeField] Text MoneyText; //돈 출력 텍스트

    void Start()
    {
        money = 0;
        score = 0;
        point = 0;

        dayCount = 0;
        enemyHPMult = 0.7f;
        enemyDamageMult = 0.7f;
        enemySpeedMult = 0.8f;

        playerHPMult = 1;
        playerSpeedMult = 1;
        playerRecoverMinus = 0;
    }
    //일수에 따른 적 체력, 공격력, 이동속도에 곱하기율 증가하고
    //5일마다 곱하기율을 크게 증가시켜 난이도를 향상시키는 함수
    public void setEnemyMult()
    {
        if(dayCount == 20)
        {
            //체력 증가량 곱하기율 1, 데미지 곱하기율 증가량 1, 데미지 이동속도 곱하기율 증가량 0.5
            setEnemyInfoMult(enemyHPMult + 1f, enemyDamageMult + 1f, enemySpeedMult + 0.5f);
        }
        else if ((dayCount + 5) % 5 == 0)
        {
            //체력 증가량 곱하기율 0.35, 데미지 곱하기율 증가량 0.35, 데미지 이동속도 곱하기율 증가량 0.12
            setEnemyInfoMult(enemyHPMult + 0.35f, enemyDamageMult + 0.35f, enemySpeedMult + 0.12f);
        }
        else
        {
            //체력 증가량 곱하기율 0.125, 데미지 곱하기율 증가량 0.125, 데미지 이동속도 곱하기율 증가량 0.065
            setEnemyInfoMult(enemyHPMult + 0.125f, enemyDamageMult + 0.125f, enemySpeedMult + 0.065f);
        }
    }
    //적 곱하기율 변수에 매개변수로 받은 값 넣는 함수
    void setEnemyInfoMult(float enemy_hp_Mult, float enemy_damage_mult, float enemy_speed_mult)
    {
        enemyHPMult = enemy_hp_Mult;
        enemyDamageMult = enemy_damage_mult;
        enemySpeedMult = enemy_speed_mult;
    }
    //일수와 적의 곱하기율 텍스트를 업데이트 시켜줌
    public void setNightInfoText()
    {
        NightCountText.text = ""+ dayCount + "일차";
        EnemyHPText.text = "적 체력 x" + enemyHPMult.ToString("F3");
        EnemyPowerText.text = "적 공격력 x" + enemyDamageMult.ToString("F3");
        EnemySpeedText.text = "적 속도 x" + enemySpeedMult.ToString("F3");
    }
    //점수 텍스트 업데이트 해줌
    public void setScoreText()
    {
        ScoreText.text = "점수 : " + score +"점";
    }
    //돈 텍스트 업데이트 해줌
    public void setMoneyText()
    {
        MoneyText.text = "가진 돈 : " + string.Format("{0:#,##0}", money) + "원"; 
    }
}
