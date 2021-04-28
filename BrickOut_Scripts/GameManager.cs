using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int brickCount;
    public static bool isEnd;
    public GameObject StartText;
    public GameObject BackGround;

    public GameObject SecondBall;
    public GameObject Stage2Wall;
    public GameObject Stage3Brick;

    void Awake()
    {
        Time.timeScale = 0;
        brickCount = 0;
        isEnd = false;
    }
    void Start()
    {
        checkStage();
    }
    void Update()
    {
        if (isEnd)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene("GameScene");
            if (Input.GetKeyDown(KeyCode.Backspace))
                SceneManager.LoadScene("MainScene");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartText.SetActive(false);
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainScene");
    }
    public void SetEndGame()
    {
        BackGround.SetActive(true);
        isEnd = true;
    }
    void checkStage()
    {
        switch (Manager.instance.stage)
        {
            case 1: SecondBall.SetActive(true); break;
            case 2: Stage2Wall.SetActive(true); break;
            case 3: Stage3Brick.SetActive(true); break;
            case 4: SecondBall.SetActive(true); Stage3Brick.SetActive(true); break;
        }
    }
}
