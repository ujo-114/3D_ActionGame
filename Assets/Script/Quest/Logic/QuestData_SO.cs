using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire//任务需求
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }


    public string qustName;
    [TextArea]
    public string description;
    public bool isStart;
    public bool isComplete;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count;
    }

    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.amount < 0)//需要上交任务物品的情况
            {
                int requireCount = Mathf.Abs(reward.amount);

                if (InventoryManager.instance.QuestItemInBag(reward.ItemData) != null) ; //背包当中有需要交的物品
                {
                    InventoryManager.instance.QuestItemInBag(reward.ItemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.instance.inventoryData.AddItem(reward.ItemData, reward.amount);
            }
        }
        InventoryManager.instance.inventoryUI.RefreshUI();
    }
}
