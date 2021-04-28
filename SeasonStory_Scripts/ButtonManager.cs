using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{   //this is used at MainScene
    public GameObject MainImage;
    public GameObject StageImage;
    public GameObject OptionPanel;

    public Slider volumeSlider;
    public AudioSource audio_Main;

    private void Start()
    {
        volumeSlider.value = Manager.instance.volumeSize;
        audio_Main.volume = Manager.instance.volumeSize;
    }

    public void main_StartButton() //버튼
    {
        MainImage.SetActive(false);
        StageImage.SetActive(true);
    }
    public void main_ExitButton() //버튼
    {
        Application.Quit();
    }
    public void main_OptionButton(string state) //버튼
    {
        if (state == "start")
            OptionPanel.SetActive(true);
        if (state == "back")
            OptionPanel.SetActive(false);
    }
    public void stage_GoMainButton() //버튼
    {
        MainImage.SetActive(true);
        StageImage.SetActive(false);
    }
    public void stage_SelectStage(string stage) //버튼
    {
        switch (stage)
        {
            case "stage1": Manager.instance.selectedStage = 1; break;
            case "stage2": Manager.instance.selectedStage = 2; break;
            case "stage3": Manager.instance.selectedStage = 3; break;
            case "stage4": Manager.instance.selectedStage = 4; break;
            default: Debug.Log("failed to load stage scene"); return;
        }
        SceneManager.LoadScene("GameScene");
    }
    public void setVolumeSlider()
    {
        Manager.instance.volumeSize = volumeSlider.value;
        audio_Main.volume = Manager.instance.volumeSize;
    }
}