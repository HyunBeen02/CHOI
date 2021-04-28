using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreData
{
    public const int NORMAL_BARRICADE_PRICE = 200;
    public const int ENHANCED_BARRICADE_PRICE = 700;
    public const int GUN_TURRET_PRICE = 4000;
    public const int MISSILE_TURRET_PRICE = 6000;
    public const int GATLING_TURRET_PRICE = 8000;
    public const int GRENADE_PRICE = 300;
    public const int RIFLE_PRICE = 8000;
    public const int SHOTGUN_PRICE = 6000;
    public const int RIFLE_AMMO_PRICE = 40;
    public const int SHOTGUN_AMMO_PRICE = 30;
}

public class StoreManager : MonoBehaviour
{
    InputManager inputManager;
    ItemManager itemManager;
    StateManager stateManager;
    BuildManager buildManager;

    [SerializeField] Text[] listText;
    [SerializeField] RectTransform catalog;

    [SerializeField] Text ItemPriceText;
    [SerializeField] Text AmmoPriceText;
    [SerializeField] Text RemaningItemText;
    [SerializeField] Text RemaningAmmoText;

    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject removeButton;

    [SerializeField] GameObject BuyAmmoButton;

    [SerializeField] Text MoneyText;

    [SerializeField] GameObject StoreBg;

    int selectedList = 0;
    public int getSelectedList { get => selectedList; private set { } }

    float targetXPos = 0; //상점 UI가 이동할 X포지션

    int defaultKey = 2000; //기본 아이템키
    int key = 0; //현재 아이템 키
    int item_price = 0;
    int ammo_price = 0;
    int remaning_item = 0;
    int remaning_ammo = 0;

    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        selectedList = 0;
        targetXPos = 0;
        key = defaultKey;
        setItemInfo();
    }
    void Update()
    {
        moveItemList();
        catalog.anchoredPosition = Vector2.Lerp(new Vector2(catalog.anchoredPosition.x, 0), new Vector2(targetXPos, 0), 0.095f);
    }
    //마우스 스크롤시 선택된 상점 아이템과 위치 변경
    void moveItemList()
    {
        if (inputManager.scrollUp())
        {
            if (selectedList < listText.Length - 1)
            {
                setOutFocusText(selectedList);
                selectedList++;
                setFocusText(selectedList);
                targetXPos -= 400;
                setSelectedItem(selectedList);
                //현재 선택된 아이템이 건물이면 건물 데이터를 줌
                buildManager.getCurrentBuilding(true);
                GameObject.Find("ScrollSound").GetComponent<AudioSource>().Play();
            }
        }
        else if (inputManager.scrollDown())
        {
            if (selectedList > 0)
            {
                setOutFocusText(selectedList);
                selectedList--;
                setFocusText(selectedList);
                targetXPos += 400;
                setSelectedItem(selectedList);
                //현재 선택된 아이템이 건물이면 건물 데이터를 줌
                buildManager.getCurrentBuilding(true);
                GameObject.Find("ScrollSound").GetComponent<AudioSource>().Play();
            }
        }
        
    }
    //선택된 아이템 텍스트 포커스 상태로 조정
    void setFocusText(int idx)
    {
        listText[idx].color = new Color32(255, 255, 255, 255);
        listText[idx].fontSize = 150;
    }
    //이전 선택 아이템 텍스트 아웃포커스 상태로 조정
    void setOutFocusText(int idx)
    {
        listText[idx].color = new Color32(180, 180, 180, 255);
        listText[idx].fontSize = 110;
    }
    /* //미사용
    public void setStoreState()
    {
        setOutFocusText(selectedList);
        setFocusText(0);
        selectedList = 0;
        catalog.anchoredPosition = Vector3.zero;
    }
    */
    //아이템 정보 현재 키에 해당하는 아이템 정보로 변경함
    void setItemInfo()
    {
        item_price = itemManager.itemInfos[key].itemCost;
        ammo_price = itemManager.itemInfos[key].ammoCost;
        remaning_item = itemManager.items[key].itemCount;
        remaning_ammo = itemManager.items[key].ammoCount;
    }
    //매개변수로 받은 키를 현재 키에 대입
    //아이템 정보 갱신
    //무기인지 건물인지 검사
    void setSelectedItem(int itemKey)
    {
        key = itemManager.ItemCode[itemKey];
        setItemInfo();
        bool is_weapon = itemManager.itemInfos[key].isWeapon;
        bool is_building = itemManager.items[key].itemType == Item.EItemType.building;
        //아이템 구매 정보 텍스트 현재 아이템 정보로 갱신
        setStoreInfos(item_price, ammo_price, remaning_item, remaning_ammo);
        //무기일시 탄창관련 버튼 활성화
        if (is_weapon)
        {
            setOnOffAmmo(true);
        }
        //무기일시 탄창관련 버튼 비활성화
        else
        {
            setOnOffAmmo(false);
        }
        //건물이면 건물 버튼 활성화
        if (is_building)
        {
            //해당 자리에 건물이 없으면 건설 버튼 활성화
            if (BuildChecker.isBuildState)
                buildButton.SetActive(true);
            //이미 건물이 있으면 제거 버튼 활성화
            else
                removeButton.SetActive(true);
            BuildChecker.isSelectedBuilding = true;
        }
        //아니면 비활성화
        else
        {
            buildButton.SetActive(false);
            BuildChecker.isSelectedBuilding = false;
        }
    }
    //구매 버튼으로 실행 매개변수로 탄창인지 여부 받아옴
    //
    public void buyItem(bool isAmmo)
    {
        /*bool is_weapon = itemManager.itemInfos[key].isWeapon;*/
        //탄창구매이면
        if (isAmmo)
        {
            //가진 돈이 가격보다 많으면
            //해당 키의 탄창을 증가시키고 돈을 가격만큼 줄임
            //현재 아이템 정보 갱신
            if(StateManager.money >= ammo_price)
            {
                itemManager.items[key].ammoCount++;
                reduceMoney(ammo_price);
                setItemInfo();
                //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
                setStoreInfos_Remaning(remaning_item, itemManager.items[key].ammoCount);
                GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            //가진 돈이 가격보다 많으면
            //해당 키의 아이템을 증가시키고 돈을 가격만큼 줄임
            //현재 아이템 정보 갱신
            if (StateManager.money >= item_price)
            {
                itemManager.items[key].itemCount++;
                reduceMoney(item_price);
                //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
                setStoreInfos_Remaning(itemManager.items[key].itemCount, remaning_ammo);
                GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
            }
        }
    }
    //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신 (*다른 스크립트에서 사용)
    public void setItemCountText()
    {
        setStoreInfos_Remaning(itemManager.items[key].itemCount, remaning_ammo);
    }
    //받은 매개변수 만큼 돈을 줄이고 돈 텍스트 갱신
    void reduceMoney(int how_much)
    {
        StateManager.money -= how_much;
        MoneyText.text = "가진 돈 : " + string.Format("{0:#,##0}", StateManager.money) + "원";
    }
    //아이템 구매 정보 텍스트 현재 아이템 정보로 갱신
    void setStoreInfos(int item_price, int ammo_price, int remaning_item, int remaning_ammo)
    {
        ItemPriceText.text = "가격 : " + string.Format("{0:#,##0}", item_price) + "원";
        AmmoPriceText.text = "탄약 : " + string.Format("{0:#,##0}", ammo_price) + "원";
        RemaningItemText.text = "남은개수 : " + string.Format("{0:#,##0}", remaning_item) + "개";
        RemaningAmmoText.text = "남은탄약 : " + string.Format("{0:#,##0}", remaning_ammo) + "개";
    }
    //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
    void setStoreInfos_Remaning(int remaning_item, int remaning_ammo)
    {
        RemaningItemText.text = "남은개수 : " + string.Format("{0:#,##0}", remaning_item) + "개";
        RemaningAmmoText.text = "남은탄약 : " + string.Format("{0:#,##0}", remaning_ammo) + "개";
    }
    //받은 매개변수에 따라 탄창관련 버튼, 텍스트 활성화 또는 비활성화
    void setOnOffAmmo(bool isOn)
    {
        if (isOn)
        {
            BuyAmmoButton.SetActive(true);
            AmmoPriceText.gameObject.SetActive(true);
            RemaningAmmoText.gameObject.SetActive(true);
        }
        else
        {
            BuyAmmoButton.SetActive(false);
            AmmoPriceText.gameObject.SetActive(false);
            RemaningAmmoText.gameObject.SetActive(false);
        }
    }
    //아침이 될시 모든 정보 초기화
    public void setDayStoreState()
    {
        setOutFocusText(selectedList);
        selectedList = 0;
        setFocusText(selectedList);
        key = itemManager.ItemCode[selectedList];
        setItemInfo();
        setStoreInfos(item_price, ammo_price, remaning_item, remaning_ammo);
        targetXPos = 0;
        //낮UI 활성화
        StoreBg.SetActive(true);
        setOnOffAmmo(false);
        buildButton.SetActive(true);
    }
    //밤이 될시 낮UI 비활성화
    public void setNightStoreState()
    {
        StoreBg.SetActive(false);
    }
}
