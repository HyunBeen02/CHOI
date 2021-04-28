using UnityEngine;
using UnityEngine.UI;
public class PlayerStatusUpButton : MonoBehaviour
{
    [SerializeField] GameObject PlayerUpgradeBG;
    [SerializeField] Text HPText;
    [SerializeField] Text SpeedText;
    [SerializeField] Text RecoverText;
    [SerializeField] Text RemainingPointText;
    [SerializeField] Text RemainingPointText_outside;

    public void joinUpBg()
    {
        PlayerUpgradeBG.SetActive(true);
        RemainingPointText.text = "포인트 : " + StateManager.point;
        RemainingPointText_outside.text = "사용가능 포인트 : " + StateManager.point;
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    public void exitUpBg()
    {
        PlayerUpgradeBG.SetActive(false);
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    public void increaseHP()
    {
        if (StateManager.point > 0)
        {
            StateManager.playerHPMult += 0.1f;
            HPText.text = "체력 증가 " + Mathf.Round(StateManager.playerHPMult * 100) + "%";
            usePoint();
        }
    }
    public void increaseSpeed()
    {
        if (StateManager.point > 0)
        {
            StateManager.playerSpeedMult += 0.05f;
            SpeedText.text = "속도 증가 " + Mathf.Round(StateManager.playerSpeedMult * 100) + "%";
            usePoint();
        }
    }
    public void DownRecoverDelay()
    {
        if (StateManager.playerRecoverMinus < 2.9)
        {
            if (StateManager.point > 0)
            {
                StateManager.playerRecoverMinus += 0.075f;
                RecoverText.text = "회복 속도 " + (EntityData.PLAYER_RECOVER_DELAY - StateManager.playerRecoverMinus) + "초";
                usePoint();
            }
        }
    }
    void usePoint()
    {
        StateManager.point--;
        RemainingPointText.text = "포인트 : " + StateManager.point;
        RemainingPointText_outside.text = "사용가능 포인트 : " + StateManager.point;
        GameManager.instance.player.setPlayerInfo();
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
}
