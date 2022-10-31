using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    //用于交换holder
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据
        InventoryManager.instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.instance.dragCanvas.transform, true);//切换父canvas确保显示
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
        //是否指向UI物品
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.instance.CheckInEquipmentUI(eventData.position) ||
            InventoryManager.instance.CheckInInventoryUI(eventData.position)|| 
            InventoryManager.instance.CheckInActionUI(eventData.position)|| 
            InventoryManager.instance.CheckInArmorUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //判断目标格子是否是原先的格子
                if (targetHolder != InventoryManager.instance.currentDrag.originalHolder)
                {
                    switch (targetHolder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Weapon)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ACTION:
                            if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Useable)
                            {
                                targetHolder.UpdateItem();
                                SwapItem();
                            }
                            break;
                        case SlotType.ARMOR:
                            if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Armor)
                            {
                                SwapItem();
                                //print(currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemName);
                            }
                            break;
                    }
                    currentHolder.UpdateItem();
                    targetHolder.UpdateItem();
                }

            }
        }
        transform.SetParent(InventoryManager.instance.currentDrag.originalParent);

        RectTransform t = transform as RectTransform;

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

   public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.ItemData == targetItem.ItemData;
        if (isSameItem)
        {
            targetItem.amount += targetItem.amount;
            tempItem.ItemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
