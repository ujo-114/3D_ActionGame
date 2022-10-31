using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { BAG,WEAPON,ACTION,ARMOR}
public class SlotHolder : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;


    public void UseItem()
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameMnager.instance.am.ac.IssueTrigger("eat");
                itemUI.Bag.items[itemUI.Index].amount -= 1;
            }
        }
        UpdateItem();
    }




    public void UpdateItem()//ピックアップのバックパックを更新
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.instance.equipmentData;
                //武器を変える
                if (itemUI.Bag.items[itemUI.Index].ItemData != null)
                {
                    GameMnager.instance.EquipWeapon(itemUI.Bag.items[itemUI.Index].ItemData);
                }
                else
                {
                    GameMnager.instance.UnEquipment();
                }
                
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.instance.actionData;
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.instance.armorData;
                if (itemUI.Bag.items[itemUI.Index].ItemData != null)
                {
                    GameMnager.instance.EquipArmor(itemUI.Bag.items[itemUI.Index].ItemData);
                }
                else
                {
                    GameMnager.instance.UnequipArmor();
                }

                break;
        }


        var item = itemUI.Bag.items[itemUI.Index];//データベース内の対応するシリアル番号の項目を返します。
        itemUI.SetUpItemUI(item.ItemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)//アイテムボックスにマウスが入ったときに、アイテム情報を表示する
    {
        if (itemUI.GetItem())
        {
            InventoryManager.instance.tooltip.SetUpTooltip(itemUI.GetItem());
            InventoryManager.instance.tooltip.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(InventoryManager.instance.tooltip.GetComponent<RectTransform>());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.tooltip.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        InventoryManager.instance.tooltip.gameObject.SetActive(false);
    }


}
