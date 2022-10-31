using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    public ItemData_SO ItemData;
    public int itemAmount;


    public void OpenBox()
    {
        InventoryManager.instance.inventoryData.AddItem(ItemData, itemAmount);
        InventoryManager.instance.inventoryUI.RefreshUI();
        print("Open Box" + ItemData.itemName);
        //Destroy(this.gameObject);
    }
}
