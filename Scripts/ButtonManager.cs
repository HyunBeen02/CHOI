using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void G_BackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (GameObject.Find("InputManager").GetComponent<InputManager>().BackToMenu())
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
