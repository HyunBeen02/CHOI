using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{   //this is used at GameScene
    public GameObject map_Spring;
    public GameObject map_Summer;
    public GameObject map_Autumn;
    public GameObject map_Winter;

    public GameObject actionButton_Spring;
    public GameObject actionButton_Summer;
    public GameObject actionButton_Autumn;
    public GameObject actionButton_Winter;

    public GameObject playerSkin_Spring;
    public GameObject playerSkin_Summer;
    public GameObject playerSkin_Autumn;
    public GameObject playerSkin_Winter;

    public AudioSource audio_Spring;
    public AudioSource audio_Summer;
    public AudioSource audio_Autumn;
    public AudioSource audio_Winter;

    AudioSource audio_Current = null;

    public GameObject OptionPanel;
    public GameObject EndPanel;

    public GameObject clearImage;
    public GameObject failureImage; 
    public GameObject NextStage_Button;
    public GameObject PreviousStage_Button;

    public Slider volumeSlider;

    void Start()
    {
        Time.timeScale = 1;
        setStage();
        setAudio();
    }
    void setStage() //스테이지에 따른 객체 활성화
    {
        switch (Manager.instance.selectedStage)
        {
            case 1:
                map_Spring.SetActive(true);
                actionButton_Spring.SetActive(true);
                playerSkin_Spring.SetActive(true);
                audio_Current = audio_Spring;
                break;
            case 2:
                map_Summer.SetActive(true);
                actionButton_Summer.SetActive(true);
                playerSkin_Summer.SetActive(true);
                audio_Current = audio_Summer;
                break;
            case 3:
                map_Autumn.SetActive(true);
                actionButton_Autumn.SetActive(true);
                playerSkin_Autumn.SetActive(true);
                audio_Current = audio_Autumn;
                break;
            case 4:
                map_Winter.SetActive(true);
                actionButton_Winter.SetActive(true);
                playerSkin_Winter.SetActive(true);
                audio_Current = audio_Winter;
                break;
            default: Debug.Log("Failed to load stage map"); break;
        }
    }
    void setAudio()
    {
        audio_Current.Play();
        volumeSlider.value = Manager.instance.volumeSize;
        audio_Current.volume = Manager.instance.volumeSize;
    }
    public void enterOptionButton()
    {
        OptionPanel.SetActive(true);
        audio_Current.Pause();
        Time.timeScale = 0;
    }
    public void exitOptionButton()
    {
        OptionPanel.SetActive(false);
        audio_Current.UnPause();
        Time.timeScale = 1;
    }
    public void goMain_Option()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }
    public void exitGame_Option()
    {
        Application.Quit();
    }
    public void setVolumeSlider()
    {
        Manager.instance.volumeSize = volumeSlider.value;
        audio_Current.volume = Manager.instance.volumeSize;
    }

    public void showEndPanel(bool isClear) //종료판을 클리어 여부에 따라 보여줌
    {
        EndPanel.SetActive(true);

        if (isClear)
        {
            clearImage.SetActive(true);
        }
        else if (!isClear)
        {
            failureImage.SetActive(true);
            NextStage_Button.SetActive(false);
        }

        if (Manager.instance.selectedStage == 1)
            PreviousStage_Button.SetActive(false);
        else if (Manager.instance.selectedStage == 4)
            NextStage_Button.SetActive(false);
    }
    public void nextStage_EndPanel()
    {
        Manager.instance.selectedStage++;
        SceneManager.LoadScene("GameScene");
    }
    public void previousStage_EndPanel()
    {
        Manager.instance.selectedStage--;
        SceneManager.LoadScene("GameScene");
    }
    public void goMain_EndPanel()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void reStart_EndPanel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void runFailure() //실행 실패
    {
        if (PlayerController.healthPoint <= 0)
        {
            showEndPanel(false);
            audio_Current.Stop();
            Time.timeScale = 0;
        }
    }
    public void runClear() //실행 성공
    {
        showEndPanel(true);
        Time.timeScale = 0;
    }
}
