using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    WeaponManager weaponManager;
    StoreManager storeManager;
    MouseCursor mouseCursor;
    BuildManager buildManager;
    StateManager stateManager;
    StageManager stageManager;

    [SerializeField] GameObject Player;

    [SerializeField] Light DayLight;
    [SerializeField] Light NightLight;

    [SerializeField] GameObject GameInfoTextBg; //밤이 될 때 보여줄 텍스트들 부모
    [SerializeField] Text[] GameInfoTexts;  //밤이 될 때 보여줄 텍스트들

    [SerializeField] Text RemaningPointText; //남은 업그레이드 포인트 텍스트

    void Start()
    {
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        storeManager = GameObject.Find("StoreManager").GetComponent<StoreManager>();
        mouseCursor = GameObject.Find("MouseCursor").GetComponent<MouseCursor>();
        buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        GameManager.instance.isDay = true;
        GameManager.instance.getData(); //게임매니저의 데이터 불러오기 기능함수
    }
    //아침으로 바꾸는 기능 함수
    public void nextDay()
    {
        GameManager.instance.isDay = true;
        //플레이어 레이어를 Player_day(9)레이어로 바꿔 건물과 충돌이 일어나지 않게 해줌
        Player.layer = 9;
        mouseCursor.setDayCursor();
        storeManager.setDayStoreState();
        weaponManager.setDayWeaponState();
        StartCoroutine(setDay());
        buildManager.setDayBuildState();
        GameManager.instance.player.setPlayerInfo();
        StateManager.point++;
        RemaningPointText.text = "사용가능 포인트 : " + StateManager.point;
        GameObject.Find("NightBGMSound").GetComponent<AudioSource>().Stop();
    }
    //밤으로 바꿔 게임을 시작하는 함수
    public void nextNight()
    {
        GameManager.instance.isDay = false;
        StateManager.dayCount++;
        stateManager.setEnemyMult();
        stateManager.setNightInfoText();
        Player.layer = 10;
        mouseCursor.setNightCursor();
        storeManager.setNightStoreState();
        weaponManager.setNightWeaponState();
        buildManager.setNightBuildState();
        stageManager.generateEnemy();
        StartCoroutine(setNight());
        StartCoroutine(effectGameInfo());
        GameObject.Find("ZombieSound").GetComponent<AudioSource>().Play();
        GameObject.Find("NightBGMSound").GetComponent<AudioSource>().Play();
    }
    //밤이 될 때 낮라이트를 회전시켜 해가 동쪽에서 서쪽으로 지는 기능 구현 (게임적 허용)
    //낮 라이트 빛의 세기를 줄이고 밤 라이트 빛의 세기를 증가시켜 자연스러운 빛 구현
    IEnumerator setNight()
    {
        WaitForSeconds upDelay = new WaitForSeconds(0.012f);

        float intensity_up = 0;
        float intensity_down = 1;
        while(intensity_up < 1)
        {
            intensity_up += 0.02f;
            intensity_down -= 0.02f;
            DayLight.intensity = intensity_down;
            NightLight.intensity = intensity_up;
            DayLight.transform.rotation = Quaternion.Euler(40, 50 + (intensity_up * -100), 0);
            yield return upDelay;
        }
        DayLight.intensity = 0;
        NightLight.intensity = 1;
    }
    //밤이 될 때 낮라이트를 회전시켜 해가 서쪽에서 동쪽으로 뜨는 기능 구현
    //낮 라이트 빛의 세기를 증가시켜고 밤 라이트 빛의 세기를 줄여 자연스러운 빛 구현
    IEnumerator setDay()
    {
        WaitForSeconds upDelay = new WaitForSeconds(0.012f);

        float intensity_up = 0;
        float intensity_down = 1;
        while (intensity_up <= 1)
        {
            intensity_up += 0.02f;
            intensity_down -= 0.02f;
            DayLight.intensity = intensity_up;
            NightLight.intensity = intensity_down;
            DayLight.transform.rotation = Quaternion.Euler(40, -50 + (intensity_up * 100), 0);
            yield return upDelay;
        }
        DayLight.intensity = 1;
        NightLight.intensity = 0;
    }
    //텍스트의 크기를 서서히 증가시키고 동시에 투명도를 조절해 자연스럽게 텍스트가 사라지도록 함
    IEnumerator effectGameInfo()
    {
        GameInfoTextBg.SetActive(true);
        WaitForSeconds effectDelay = new WaitForSeconds(0.01f);
        float multiNum = 0.35f;
        for (float i = 0; i < 1; i += 0.005f)
        {
            //(1 + (i * multiNum)은 1.nnn처럼 만들어주기 위함 그리고 나온 값을 현재 스케일 1에 곱해줌
            GameInfoTextBg.transform.localScale = new Vector3(1 * (1 + (i * multiNum)), 1 * (1 + (i * multiNum)), 1);
            foreach(Text text in GameInfoTexts)
            {
                byte reduceNum = (byte)(i * 250);
                text.color = new Color32(255, 255, 255, (byte)(255 - reduceNum));
            }
            yield return effectDelay;
        }
        //텍스트 초기값으로 바꿔줌
        foreach (Text text in GameInfoTexts)
            text.color = new Color32(255, 255, 255, 255);
        GameInfoTextBg.SetActive(false);
        GameInfoTextBg.transform.localScale = Vector3.one;
    }
}
