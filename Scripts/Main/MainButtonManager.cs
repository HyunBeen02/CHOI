using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainButtonManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject OptionBG;
    [SerializeField] GameObject MoreBG;
    SoundManager soundManager;

    void Start()
    {
        Cursor.visible = true; //마우스 커서가 보이게
        volumeSlider.value = GameManager.instance.volume_val;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    //게임시작
    public void startGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    //환경설정 활성화 또는 비활성화
    public void setOption(bool isOn)
    {
        OptionBG.SetActive(isOn);
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    //볼륨 조정 슬라이드
    public void setVolumeSize()
    {
        GameManager.instance.volume_val = volumeSlider.value;
        soundManager.setSoundVolume();
    }
    //전체화면 또는 창모드
    public void setScreenMode(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    //더보기 활성화 또는 비활성화
    public void setMore(bool isOn)
    {
        MoreBG.SetActive(isOn);
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    //게임종료
    public void QuitGame()
    {
        Application.Quit();
    }

}
