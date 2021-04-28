using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    InputManager inputManager;
    ItemManager itemManager;
    Weapon weapon;
    enum EWeaponType { Pistol, Rifle, ShotGun }
    bool canfire = true;
    bool canWeaponChange = true;

    float weaponChangeSpanTime = 0;
    float currentFireRate = 0f;
    float currentReloadSpeed = 0;
    int currentItemKey = 0;
    int currentMaxAmmo = 0;
    int currentBulletCount = 0;
    Dictionary<int, int> weaponBulletMaxCount = new Dictionary<int, int>();
    Dictionary<int, int> currentWeaponBulletCount = new Dictionary<int, int>();
    List<int> allKey = new List<int>(); //총알이 추가될 때 해당 키에 키값을 대입

    public Transform pistolStartPos;
    public Transform longGunStartPos;
    private Transform currentrStartPos;

    public GameObject[] pistolBullets;
    public GameObject[] rifleBullets;
    public GameObject[] shotGunBullets;
    private GameObject[] currentBullets;

    public GameObject[] pistolFireFlame;
    public GameObject[] rifleFireFlame;
    public GameObject[] shotGunFireFlame;
    private GameObject[] currentFireFlame;

    public GameObject PistolObject;
    public GameObject RifleObject;
    public GameObject ShotGunObject;

    public Player player;

    public Text WeaponText;

    public GameObject WeaponInfoBg;
    public Slider RemaningBulletSlider;
    public Text RemaningAmmoText;

    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        weapon = GetComponent<Weapon>();

        currentrStartPos = pistolStartPos;
        currentBullets = pistolBullets;
        currentFireFlame = pistolFireFlame;

        currentFireRate = WeaponData.PISTOL_FIRE_RATE;
        currentReloadSpeed = WeaponData.PISTOL_RELOAD_SPEED;
        currentMaxAmmo = WeaponData.PISTOL_MAX_AMMO;

        canfire = true;
        canWeaponChange = true;

        weaponChangeSpanTime = 1.2f;

        currentItemKey = 1000;
        currentBulletCount = 0;

        setBulletContent(1000, WeaponData.PISTOL_MAX_AMMO);
        setBulletContent(1001, WeaponData.RIFLE_MAX_AMMO);
        setBulletContent(1002, WeaponData.SHOTGUN_MAX_AMMO);
    }
    void Update()
    {
        changeWeapon();
        reloadManual();
    }
    //총알 딕셔너리와 최대 총알수 딕셔너리에 키값과 최대 총알을 받아 넣어줌
    void setBulletContent(int key, int max_ammo)
    {
        weaponBulletMaxCount[key] = max_ammo;
        currentWeaponBulletCount[key] = weaponBulletMaxCount[key];
        allKey.Add(key);
    }
    //전략패턴을 활용 무기 변경시 현재 무기 타입에 무기 클래스를 넣어주고
    //현재 무기 데이터에 무기 타입에 맞춰 넣어줌
    void changeWeapon(EWeaponType weaponState, int key_)
    {
        switch (weaponState)
        {
            case EWeaponType.Pistol:
                weapon.setWeapon(new Pistol());
                setWeaponData(key_, WeaponData.PISTOL_FIRE_RATE, WeaponData.PISTOL_RELOAD_SPEED, 
                    pistolBullets, pistolFireFlame, pistolStartPos);
                break;
            case EWeaponType.Rifle:
                weapon.setWeapon(new RifleGun());
                setWeaponData(key_, WeaponData.RIFLE_FIRE_RATE, WeaponData.RIFLE_RELOAD_SPEED, 
                    rifleBullets, rifleFireFlame, longGunStartPos);
                break;
            case EWeaponType.ShotGun:
                weapon.setWeapon(new ShotGun());
                setWeaponData(key_, WeaponData.SHOTGUN_FIRE_RATE, WeaponData.SHOTGUN_RELOAD_SPEED, 
                    shotGunBullets, shotGunFireFlame, longGunStartPos);
                break;
            default: Debug.Log("Finding gun state is failed"); break;
        }
    }
    //현재 정보들에 받은 무기 정보 매개변수들을 대입해줌
    void setWeaponData(int itemKey_, float fireRate_, float reloadSpeed_, GameObject[] bullets_, GameObject[] fireFlame_, Transform startPos_)
    {
        currentItemKey = itemKey_;
        currentFireRate = fireRate_;
        currentReloadSpeed = reloadSpeed_;
        currentBullets = bullets_;
        currentFireFlame = fireFlame_;
        currentrStartPos = startPos_;
    }
    //발사 함수 현재 키에 해당하는 총알 개수가 0개 이하면 장전 실행
    public void shoot()
    {
        if (!inputManager.fire() || !canfire || !canWeaponChange || GameManager.instance.isDay)
            return;
        if (currentWeaponBulletCount[currentItemKey] <= 0)
        {
            reload(currentReloadSpeed);
        }
        if (currentWeaponBulletCount[currentItemKey] > 0)
        {
            GameObject.Find("PistolFireSound").GetComponent<AudioSource>().Play();
            currentWeaponBulletCount[currentItemKey]--; //총알 감소
            //총알 정보 슬라이드UI를 감소된 총알에 따라 갱신
            setBulletSlide(currentWeaponBulletCount[currentItemKey], weaponBulletMaxCount[currentItemKey]);
            //전략패턴으로 받은 무기 클래스의 발사 함수를 실행
            //매개변수로 현재 총알과 발사위치를 줌
            weapon.shoot(currentBullets, currentrStartPos);
            //발사시 불꽃 이펙트 실행
            StartCoroutine(onFireFlame());
            //일정시간 발사 못하도록 함
            waitForFireRate(currentFireRate);
            if(currentWeaponBulletCount[currentItemKey] <= 0)
            {
                reload(currentReloadSpeed);
            }
        }
    }
    //받은 시간동안 canfire를 false를 주어 발사를 막음
    public void waitForFireRate(float fireRate)
    {
        canfire = false;
        if (!canfire)
        {
            //딜레이 후 발사 가능하게
            Invoke("turnTrueCanFire", fireRate);
        }
    }
    //인보크로 실행될 딜레이 함수
    void turnTrueCanFire()
    {
        canfire = true;
    }
    //발사시 불꽃 이펙트 활성화
    IEnumerator onFireFlame()
    {
        //2개의 불꽃 타입중 하나를 활성화 해줌
        int ran = Random.Range(0, currentFireFlame.Length);
        currentFireFlame[ran].SetActive(true);
        yield return new WaitForSeconds(0.06f);
        currentFireFlame[ran].SetActive(false);
    }
    //수동 재장전 함수
    void reloadManual()
    {
        if (GameManager.instance.isDay)
            return;
        if (inputManager.reload() && canWeaponChange)
        {
            reload(currentReloadSpeed);
        }
    }
    //재장전 함수 탄창이 0이상일시 실행 가능
    void reload(float reloadSpanTime)
    {
        if(itemManager.items[currentItemKey].ammoCount > 0)
        {
            
            GameObject.Find("ReloadSound").GetComponent<AudioSource>().Play();
            StartCoroutine(reloadEffect(reloadSpanTime));
            itemManager.items[currentItemKey].ammoCount--; //탄창 감소
            setAmmoText(currentItemKey); //탄창 텍스트 갱신
        }
    }
    //장전시 슬라이더가 천천히 증가하는 이펙트
    IEnumerator reloadEffect(float reloadSpanTime)
    {
        //장전중 무기 변경 불가능
        canWeaponChange = false;
        for (float idx = 0; idx < reloadSpanTime; idx += 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            //최대 총알 개수 / 장전속도 * 100을 해서 반복되는 동안 일정하게 증가시켜줌
            RemaningBulletSlider.value += weaponBulletMaxCount[currentItemKey] / (reloadSpanTime * 100);
        }
        //최대값으로 초기화
        RemaningBulletSlider.value = weaponBulletMaxCount[currentItemKey];
        currentWeaponBulletCount[currentItemKey] = weaponBulletMaxCount[currentItemKey];
        canWeaponChange = true;
    }
    //무기 변경키 입력시 무기 변경 함수들 실행
    void changeWeapon()
    {
        if (!canWeaponChange || !canWeaponChange || GameManager.instance.isDay)
            return;

        int pistolAniState = 0;
        int longGunAniState = 1; //긴 소총류

        //Object는 무기 모델
        if (inputManager.KeyDownNum(1))
        {
            setOnWeapon(1000, EWeaponType.Pistol, PistolObject, pistolAniState);
        }
        else if (inputManager.KeyDownNum(2))
        {
            setOnWeapon(1001, EWeaponType.Rifle, RifleObject, longGunAniState);
        }
        else if (inputManager.KeyDownNum(3))
        {
            setOnWeapon(1002, EWeaponType.ShotGun, ShotGunObject, longGunAniState);
        }
    }
    //무기 활성화될 무기 개수가 0이상이면 현재 무기 정보를
    //활성화될 무기 정보로 갱신해줌
    void setOnWeapon(int key, EWeaponType weaponType, GameObject weaponObj, int aniState)
    {
        //총구 섬광 비활성화
        setOffFireFlameActive();
        if (itemManager.items[key].itemCount > 0)
        {
            currentBulletCount = currentWeaponBulletCount[key];
            setBulletSlide(currentBulletCount, weaponBulletMaxCount[key]);
            //전략패턴으로 무기 갈아끼워줌 
            changeWeapon(weaponType, key);
            setActiveWeapon(weaponObj);
            //무기 교체 애니메이션 실행 후 매개변수가 0일시 권총 1일시 소총 애니메이션 실행
            //아바타 마스크로 상체와 하체 애니메이션을 다르게 함
            player.startWeaponSwapAni(aniState);
            canWeaponChange = false;
            //무기 교체 딜레이
            Invoke("delayWeaponChange", weaponChangeSpanTime);
            //현재 무기 텍스트 갱신
            WeaponText.text = "무기 : " + itemManager.items[key].itemName;
            //현재 탄창 텍스트 갱신
            setAmmoText(currentItemKey);
        }
    }
    //무기 변경 딜레이 함수
    void delayWeaponChange()
    {
        canWeaponChange = true;
    }
    //총구 섬광 비활성화
    void setOffFireFlameActive()
    {
        currentFireFlame[0].SetActive(false);
        currentFireFlame[1].SetActive(false);
    }
    //매개변수로 받은 값을 남은총알 슬라이드에 대입해 슬라이드 갱신
    void setBulletSlide(int current_bullet_count, int max_bullet)
    {
        RemaningBulletSlider.maxValue = max_bullet;
        RemaningBulletSlider.value = current_bullet_count;
    }
    //남은탄창 텍스트 갱신
    void setAmmoText(int key)
    {
        if(key == 1000)
        {
            RemaningAmmoText.text = "탄창 : 무제한";
            return;
        }
        RemaningAmmoText.text = "탄창 : " + itemManager.items[key].ammoCount;
    }
    //매개변수로 받은 무기 모델 오브젝트를 제외한 모든 모델 비활성화
    void setActiveWeapon(GameObject obj)
    {
        PistolObject.SetActive(false);
        RifleObject.SetActive(false);
        ShotGunObject.SetActive(false);
        obj.SetActive(true);
    }
    //아침이 될시 권총으로 바꿔주고 권총 정보로 슬라이더와
    //무기 정보 텍스트 갱신
    public void setDayWeaponState()
    {
        int pistolAniState = 0;
        setOnWeapon(1000, EWeaponType.Pistol, PistolObject, pistolAniState);
        foreach(int key_ in allKey)
        {
            currentWeaponBulletCount[key_] = weaponBulletMaxCount[key_];
        }
        RemaningBulletSlider.maxValue = WeaponData.PISTOL_MAX_AMMO;
        RemaningBulletSlider.value = WeaponData.PISTOL_MAX_AMMO;
        WeaponText.text = "무기 : 무제한";
        //무기 정보창 비활성화
        WeaponInfoBg.SetActive(false);
    }
    //밤이 될시 무기 정보창을 활성화 함
    public void setNightWeaponState()
    {
        WeaponInfoBg.SetActive(true);
    }
}
