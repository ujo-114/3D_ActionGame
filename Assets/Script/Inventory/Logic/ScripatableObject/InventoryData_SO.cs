using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]


public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public void AddItem(ItemData_SO newItemData,int amount)
    {
        bool found = false;
        foreach (var item in items)
        {
            if (item.ItemData == newItemData)
            {
                item.amount += amount;
                found = true;
                break;
            }
        }
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemData == null && !found)
            {
                items[i].ItemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }


    }
    
}

[System.Serializable]//序列化
public class InventoryItem//管理类
{
    public ItemData_SO ItemData;
    public  int amount;
}
