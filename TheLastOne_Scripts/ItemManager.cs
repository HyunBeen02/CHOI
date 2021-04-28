using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Dictionary<int, Item> items = new Dictionary<int, Item>();
    public Dictionary<int, ItemInfo> itemInfos = new Dictionary<int, ItemInfo>();
    public Dictionary<int, int> ItemCode = new Dictionary<int, int>();

    int Dic_Length = 0;

    int noneAmmo = -1;
    int cantBuyAmmoPrice = 9000000;
    int setItemCount = 0;
    int setBulletCount = 0;

    bool weapon_item = true;
    bool other_item = false;

    void Start()
    {
        addDefaultItem(1000, weapon_item, Item.EItemType.weapon, "권총", 1, 99999, 9000000, cantBuyAmmoPrice);

        //아이템 추가는 상점 UI의 순서와 같게 해야함.
        addItem(2000, other_item, Item.EItemType.building, "방어벽", setItemCount, noneAmmo, StoreData.NORMAL_BARRICADE_PRICE, cantBuyAmmoPrice);
        addItem(2001, other_item, Item.EItemType.building, "강화방어벽", setItemCount, noneAmmo, StoreData.ENHANCED_BARRICADE_PRICE, cantBuyAmmoPrice);
        addItem(2002, other_item, Item.EItemType.building, "건포탑", setItemCount, noneAmmo, StoreData.GUN_TURRET_PRICE, cantBuyAmmoPrice);
        addItem(2003, other_item, Item.EItemType.building, "미사일포탑", setItemCount, noneAmmo, StoreData.MISSILE_TURRET_PRICE, cantBuyAmmoPrice);
        addItem(2004, other_item, Item.EItemType.building, "게틀링포탑", setItemCount, noneAmmo, StoreData.GATLING_TURRET_PRICE, cantBuyAmmoPrice);

        addItem(3000, other_item, Item.EItemType.item, "수류탄", setItemCount, noneAmmo, StoreData.GRENADE_PRICE, cantBuyAmmoPrice);

        addItem(1001, weapon_item, Item.EItemType.weapon, "자동소총", setItemCount, setBulletCount, StoreData.RIFLE_PRICE, StoreData.RIFLE_AMMO_PRICE);
        addItem(1002, weapon_item, Item.EItemType.weapon, "산탄총", setItemCount, setBulletCount, StoreData.SHOTGUN_PRICE, StoreData.SHOTGUN_AMMO_PRICE);
    }
    void addItem(int key_or_itemCode, bool is_weapon, Item.EItemType item_type, string item_name, int item_count, int ammo_count, int item_cost, int bullet_cost)
    {
        items[key_or_itemCode] = (new Item(item_type, item_name, key_or_itemCode, item_count, ammo_count));
        itemInfos[key_or_itemCode] = (new ItemInfo(is_weapon, item_cost, bullet_cost));
        ItemCode[Dic_Length] = key_or_itemCode;
        Dic_Length++;
    }
    void addDefaultItem(int key_or_itemCode, bool is_weapon, Item.EItemType item_type, string item_name, int item_count, int ammo_count, int item_cost, int bullet_cost)
    {
        items[key_or_itemCode] = (new Item(item_type, item_name, key_or_itemCode, item_count, ammo_count));
        itemInfos[key_or_itemCode] = (new ItemInfo(is_weapon, item_cost, bullet_cost));
    }
}
