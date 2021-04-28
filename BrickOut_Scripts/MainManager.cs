using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject BasicBG;
    public GameObject VariantBG;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Application.Quit();
    }
    public void selectMenu(string name)
    {
        switch (name)
        {
            case "classic": GameObject.Find("select").GetComponent<AudioSource>().Play(); Manager.instance.stage = 0; SceneManager.LoadScene("GameScene"); break;
            case "variant": GameObject.Find("select").GetComponent<AudioSource>().Play(); BasicBG.SetActive(false); VariantBG.SetActive(true); break;
            case "stage1": GameObject.Find("select").GetComponent<AudioSource>().Play(); Manager.instance.stage = 1; SceneManager.LoadScene("GameScene"); break;
            case "stage2": GameObject.Find("select").GetComponent<AudioSource>().Play(); Manager.instance.stage = 2; SceneManager.LoadScene("GameScene"); break;
            case "stage3": GameObject.Find("select").GetComponent<AudioSource>().Play(); Manager.instance.stage = 3; SceneManager.LoadScene("GameScene"); break;
            case "stage4": GameObject.Find("select").GetComponent<AudioSource>().Play(); Manager.instance.stage = 4; SceneManager.LoadScene("GameScene"); break;
            case "back": GameObject.Find("select").GetComponent<AudioSource>().Play(); BasicBG.SetActive(true); VariantBG.SetActive(false); break;
        }
    }
}
