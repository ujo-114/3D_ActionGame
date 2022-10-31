using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public class DragData//ドラッグ＆ドロップ時のデータ保存用
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    public DragData currentDrag;



    //添加模板用于保存数据
    [Header("Template Data")]
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO equipmentTemplate;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO armorTemplate;


    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO equipmentData;
    public InventoryData_SO actionData;
    public InventoryData_SO armorData;

    [Header("Container")]
    public ContainerUI inventoryUI;
    public ContainerUI equipmentUI;
    public ContainerUI actionUI;
    public ContainerUI armorUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;

    [Header("Stats Text")]
    public Text healthText;
    public Text attackText;
    public Text defenseText;

    [Header("Tooltip")]
    public ItemTooltip tooltip;



    public bool isOpen = false;



    void Awake()
    {
        //构造单例
        CheckGameObject();
        CheckSingle();
        //加载初始数据
        LoadTemplateData();
    }
    //加载初始数据
    public void LoadTemplateData()
    {
        if (inventoryTemplate != null)
        {
            inventoryData = Instantiate(inventoryTemplate);
        }
        if (actionTemplate != null)
        {
            actionData = Instantiate(actionTemplate);
        }
        if (equipmentTemplate != null)
        {
            equipmentData = Instantiate(equipmentTemplate);
        }
        if (armorTemplate != null)
        {
            armorData = Instantiate(armorTemplate);
        }
    }

    public void UpdateStatsText(float health,float attack,float defensive)//ステータスバー表示の更新
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        defenseText.text = defensive.ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            armorUI.RefreshUI();
            inventoryUI.RefreshUI();
            equipmentUI.RefreshUI();
            actionUI.RefreshUI();
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }
        if (SceneManager.GetActiveScene().name != "Main")
            if (GameMnager.instance.am.wm.wcR.wdata!= null)
            {
                UpdateStatsText(GameMnager.instance.am.sm.HP, GameMnager.instance.am.wm.wcR.wdata.ATK,GameMnager.instance.am.sm.charaterDefensive);
            }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetKusuri();
        }
    }

    public InventoryItem QuestItemInBag(ItemData_SO questItem)//バックパックの中にミッションアイテムがあるかどうかを確認する
    {
        return inventoryData.items.Find(i => i.ItemData == questItem);
    }


    //ドラッグしたアイテムがSLOT内にあるかどうかをチェックする
    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
            
        }
        return false;
    }
    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)equipmentUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }
    public bool CheckInArmorUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)armorUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }
    //保存,读取背包信息
    public void SavaData()
    {
        SaveManager.instance.Save(inventoryData, "inventoryData");
        SaveManager.instance.Save(actionData, "actionData");
        SaveManager.instance.Save(equipmentData, "equipmentData");
        SaveManager.instance.Save(armorData, "armorData");
    }

    public void LoadData()
    {
        SaveManager.instance.Load(inventoryData, "inventoryData");
        SaveManager.instance.Load(actionData, "actionData");
        SaveManager.instance.Load(equipmentData, "equipmentData");
        SaveManager.instance.Load(armorData, "armorData");
    }

    public void ResetKusuri()
    {
        bool foundInAction = false;
        foreach (var item in actionData.items)
        {
            if(item.ItemData!=null)
            {
                if (item.ItemData.itemName == "血瓶")
                {
                    item.amount = 9;
                    foundInAction = true;
                    break;
                }
            }
        }
        if (!foundInAction)
        {
            foreach (var item in inventoryData.items) 
            {
                if (item.ItemData!= null)
                {
                    if (item.ItemData.itemName == "血瓶")
                    {
                        item.amount = 9;
                        break;
                    }
                }
            }
        }
        inventoryUI.RefreshUI();
        equipmentUI.RefreshUI();
        actionUI.RefreshUI();
    }








    private void CheckSingle()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        else
            Destroy(this.gameObject);
    }
    private void CheckGameObject()
    {
        if (name == "Inventory Canvas")
        {
            return;

        }
        else
            Destroy(this);
    }

}
