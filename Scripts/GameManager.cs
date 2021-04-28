using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글턴 패턴
    public static GameManager instance;
    public EnemyChecker enemyChecker;
    public StateManager stateManager;
    public Player player;

    public float volume_val = 0;

    public bool isDay = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        isDay = true;
        volume_val = 0.5f;
    }
    public void getData()
    {
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        enemyChecker = GameObject.Find("EnemyChecker").GetComponent<EnemyChecker>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
}
