using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    public InventoryData_SO Bag { get; set; }
    public int Index = -1;

    public void SetUpItemUI(ItemData_SO item,int itemAmount)//アイコンと数量を取得
    {
        
        if (itemAmount < 0)//若item数量小于零，则关闭UI显示（任务ui用
        {
            item = null;
        }


        if (item != null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
            //格子中道具数量为0，则删除itemData和图标（可使用道具除外）
            if (itemAmount == 0 && item.itemType != ItemType.Useable)
            {
                Bag.items[Index].ItemData = null;
                icon.gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem()//创建接口返回当前slot中的item数据，方便调用
    {
        return Bag.items[Index].ItemData;
    }



}
