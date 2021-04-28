using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    ItemManager itemManager;
    StoreManager storeManager;
    GameObject buildChecker;

    [SerializeField] Transform player;

    [SerializeField] GameObject PreviewNormalBarricade;
    [SerializeField] GameObject PreviewEnhancedBarricade;
    [SerializeField] GameObject PreviewGunTurret;
    [SerializeField] GameObject PreviewMissileTurret;
    [SerializeField] GameObject PreviewGatlingTurret;

    [SerializeField] GameObject NormalBarricade;
    [SerializeField] GameObject EnhancedBarricade;
    [SerializeField] GameObject GunTurret;
    [SerializeField] GameObject MissileTurret;
    [SerializeField] GameObject GatlingTurret;
    GameObject currentBuilding;

    [SerializeField] GameObject PreviewBuilding;

    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject removeButton;

    public int buildPos_X = 0;
    public int buildPos_Z = 0;

    public static bool canBuild = true;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        storeManager = GameObject.Find("StoreManager").GetComponent<StoreManager>();
        buildChecker = GameObject.Find("BuildChecker");

        currentBuilding = NormalBarricade;

        canBuild = true;
    }
    void Update()
    {
        getBuildPos();
        getCurrentBuilding(false);
    }
    //일정 거리마다 설치 가능 구역을 구함 (단위:1) 
    void getBuildPos()
    {
        Vector3 playerPos = player.transform.position;
        //플레이어 위치가 0.5 기준으로 위인지 아래인지 판정하고
        //설치 위치를 조정
        if (playerPos.x > Mathf.FloorToInt(playerPos.x) + 0.5f)
        {
            buildPos_X = Mathf.FloorToInt(playerPos.x) + 1;
        }
        else
        {
            buildPos_X = Mathf.FloorToInt(playerPos.x);
        }
        if (playerPos.z > Mathf.FloorToInt(playerPos.z) + 0.5f)
        {
            buildPos_Z = Mathf.FloorToInt(playerPos.z) + 1;
        }
        else
        {
            buildPos_Z = Mathf.FloorToInt(playerPos.z);
        }
    }
    int currentX = 0;
    int currentZ = 0;
    //Forced = 강제 실행 여부
    //상점에서 현재 선택된 아이템에 따라 해당 미리보기 건물 활성화
    public void getCurrentBuilding(bool Forced)
    {
        if (!GameManager.instance.isDay)
            return;
        //불필요한 실행을 없애기 위해 건물 설치 위치 변경이 감지되지 않으면 리턴
        if (currentX == buildPos_X && currentZ == buildPos_Z && !Forced)
            return;

        //움직임이 감지되면 현재 x포지션과 z포지션을 업데이트 시켜줌.
        currentX = buildPos_X;
        currentZ = buildPos_Z;

        int key = itemManager.ItemCode[storeManager.getSelectedList];
        ActiveOffAllPreview();
        //건물 아이템키에 따라 미리보기 건물 설정
        switch (key)
        {
            case 2000: setPreviewBuilding(PreviewNormalBarricade, NormalBarricade); break;
            case 2001: setPreviewBuilding(PreviewEnhancedBarricade, EnhancedBarricade); break;
            case 2002: setPreviewBuilding(PreviewGunTurret, GunTurret); break;
            case 2003: setPreviewBuilding(PreviewMissileTurret, MissileTurret); break;
            case 2004: setPreviewBuilding(PreviewGatlingTurret, GatlingTurret); break;
            default:
                Debug.Log("This is not building");
                currentBuilding = null;
                setPosxbuildChecker();
                break;
        }
    }
    //매개변수로 받은 건물의 미리보기 오브젝트를 활성화
    //현재 건물에 매개변수로 받은 건물 넣기
    //받은 미리보기 건물 위치 구한 건설위치로 조정
    void setPreviewBuilding(GameObject preview_building, GameObject building)
    {
        preview_building.SetActive(true);
        currentBuilding = building;
        preview_building.transform.position = new Vector3(buildPos_X, 0, buildPos_Z);
        setPosxbuildChecker();
    }
    //실제 건물 건설위치 조정
    void setPosxbuildChecker()
    {
        buildChecker.transform.position = new Vector3(buildPos_X, 0, buildPos_Z);
    }
    //모든 미리보기 건물을 비활성화
    void ActiveOffAllPreview()
    {
        PreviewNormalBarricade.SetActive(false);
        PreviewEnhancedBarricade.SetActive(false);
        PreviewGunTurret.SetActive(false);
        PreviewMissileTurret.SetActive(false);
        PreviewGatlingTurret.SetActive(false);
    }
    //건설버튼 기능
    public void buildObj()
    {
        if (!canBuild)
            return;
        int key = itemManager.ItemCode[storeManager.getSelectedList];
        //현재 키에 해당하는 건물의 개수가 0이상이면
        //아이템 감소 및 아이템 정보 갱신
        //건물 생성
        if (itemManager.items[key].itemCount > 0)
        {
            GameObject.Find("BuildingSound").GetComponent<AudioSource>().Play();
            itemManager.items[key].itemCount--;
            storeManager.setItemCountText();
            Instantiate(currentBuilding, new Vector3(buildPos_X, 0, buildPos_Z), Quaternion.identity);
        }
    }
    BuildingInfo currentBuildingInfo;
    //건물 제거 버튼
    public void removeBuildingObj()
    {
        //제거할 건물의 키를 가져와 제거 후
        //제거한 건물의 재고 개수를 증가시킴
        int targetKey = currentBuildingInfo.keyCode;
        Destroy(currentBuildingInfo.gameObject);
        itemManager.items[targetKey].itemCount++;
        //아이템 개수 텍스트 갱신
        storeManager.setItemCountText();
        //제거했으니 해당 자리엔 건물이 없으므로 제거 버튼 비활성화
        removeButton.SetActive(false);
        GameObject.Find("BuildingSound").GetComponent<AudioSource>().Play();
        //제거 후 현재 상점에서 선택된 아이템이 건물이면 건설버튼 활성화
        if (BuildChecker.isSelectedBuilding)
        {
            buildButton.SetActive(true);
        }
        //건설 가능하게
        BuildChecker.isBuildState = true;
    }
    //현재 제거할 건물의 키를 가져옴
    public void getRemoveTargetBuliding(BuildingInfo buildingInfo_)
    {
        currentBuildingInfo = buildingInfo_;
    }
    //아침이 될시 미리보기 활성화 및 버튼 설정
    public void setDayBuildState()
    {
        buildButton.SetActive(true);
        removeButton.SetActive(false);
        PreviewBuilding.SetActive(true);
    }
    //밤이 될시 미리보기 건물 기본값으로 초기화 후 비활성화
    public void setNightBuildState()
    {
        BuildChecker.isBuildState = true;
        setPreviewBuilding(PreviewNormalBarricade, NormalBarricade);
        PreviewBuilding.SetActive(false);
    }
}
