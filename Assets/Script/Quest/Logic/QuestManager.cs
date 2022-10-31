using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [System.Serializable]
    public class QuestTask//角色接取的任务
    {
        public QuestData_SO questData;
        public bool IsStart { get { return questData.isStart; } set { questData.isStart = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished= value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    private void Start()
    {
        LoadQuestManager();
    }


    public void SaveQuestManager()
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);
        for (int i = 0; i < tasks.Count; i++)
        {
            SaveManager.instance.Save(tasks[i].questData, "task" + i);
        }
    }

    public void LoadQuestManager()
    {
        tasks = new List<QuestTask>();
        var questCount = PlayerPrefs.GetInt("QuestCount");
        for (int i = 0; i < questCount; i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.instance.Load(newQuest, "task" + i);
            tasks.Add(new QuestTask { questData = newQuest });
        }
    }



    //敵が死亡したとき、アイテムを拾ったとき、ミッションのステータスが更新されたときに呼び出される
    public void UpdateQuestProgress(string requireName,int amount)
    {
        foreach (var task in tasks)
        {
            if (task.IsFinished)
                continue;
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null)
            {
                matchTask.currentAmount += amount;
            }
            task.questData.CheckQuestProgress();

        }
    }


   public bool HaveQuest(QuestData_SO data)//タスクがすでにタスクリストに存在するかどうかを判断する
    {
        if (data != null)
            return tasks.Any(q => q.questData.qustName == data.qustName);
        else return false;
    }

    public QuestTask GetTask(QuestData_SO data)//データに対応するQuestTaskを検索する
    {
        return tasks.Find(q => q.questData.qustName == data.qustName);
    }






    void Awake()
    {
        //构造单例
        CheckGameObject();
        CheckSingle();
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
        if (name == "QuestManager")
        {
            return;

        }
        else
            Destroy(this);
    }
}
