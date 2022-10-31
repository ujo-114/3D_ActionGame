using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestData_SO currentQuest;

    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;


    //获得任务状态
    public bool IsStarted
    {
        get
        {
            if (QuestManager.instance.HaveQuest(currentQuest))
            {
                return QuestManager.instance.GetTask(currentQuest).IsStart;
            }
            else
                return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.instance.HaveQuest(currentQuest))
            {
                return QuestManager.instance.GetTask(currentQuest).IsComplete;
            }
            else
                return false;
        }
    }
    public bool IsFinsihed
    {
        get
        {
            if (QuestManager.instance.HaveQuest(currentQuest))
            {
                return QuestManager.instance.GetTask(currentQuest).IsFinished;
            }
            else
                return false;
        }
    }

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    private void Start()
    {
        controller.currentData = startDialogue;
        currentQuest = controller.currentData.GetQuest();
    }
    private void Update()
    {
        if (IsStarted)
        {
            if (IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }

        if (IsFinsihed)
        {
            controller.currentData = finishDialogue;
        }
    }

}
