using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    GameStartManager gameStartManager;
    public static int enemyCount = 0; //적의 수
    void Start()
    {
        gameStartManager = GameObject.Find("GameStartManager").GetComponent<GameStartManager>();
        enemyCount = 0;
    }
    //클리어시 실행
    public void clearNight()
    {
        gameStartManager.nextDay();
    }
}