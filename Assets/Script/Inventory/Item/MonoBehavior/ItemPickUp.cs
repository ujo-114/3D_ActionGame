using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO ItemData;
    public bool canPick=false;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"){
            canPick = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPick = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&&canPick)
        {
            //放入背包销毁并
            InventoryManager.instance.inventoryData.AddItem(ItemData, ItemData.itemAmount);
            InventoryManager.instance.inventoryUI.RefreshUI();

            //检查任务
            QuestManager.instance.UpdateQuestProgress(ItemData.itemName, ItemData.itemAmount);
            Destroy(this.gameObject);

        }
    }
}
