using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType//使用枚举变量定义可拾取物品的类型
{
    Useable,Weapon,Armor,MissionItem
}

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]

public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public float defensive;
    [TextArea]
    public string description = "";

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AnimatorOverrideController weaponAnimator;

}
