using UnityEngine;

public class Item
{
    public enum EItemType { weapon, building, item, none }
    public EItemType itemType = EItemType.none;
    public string itemName = "none";
    public int itemCode = 0; //[weapon is 1000 ~ 1999], [building is 2000 ~ 2999], [item is 3000 ~ 3999]
    public int itemCount = 0;
    public int ammoCount = 0;
    public Item(EItemType _ItemType, string _ItemName, int _ItemCode, int _ItemCount, int _AmmoCount)
    {
        itemType = _ItemType;
        itemName = _ItemName;
        itemCode = _ItemCode;
        itemCount = _ItemCount;
        ammoCount = _AmmoCount;
    }
}
public class ItemInfo
{
    public bool isWeapon = false; //무기일 경우 총알 참조 가능
    public int itemCost = 0;
    public int ammoCost = 0;
    public ItemInfo(bool is_weapon, int item_cost, int ammo_cost)
    {
        isWeapon = is_weapon;
        itemCost = item_cost;
        ammoCost = ammo_cost;
    }
}
