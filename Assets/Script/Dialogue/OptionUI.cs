using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optinText;
    private Button thisButton;
    private DialoguePiece currentPiece;
    private string nextPieceID;
    private bool takeQuest;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }
    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece = piece;
        optinText.text=option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;
    }

    public void OnOptionClicked()
    {
        if (currentPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask//创建currentPiece.quest对应的QusetTask，用临时变量接一下
            {
                questData=Instantiate(currentPiece.quest)
            };
            if (takeQuest)
            {
                //添加到角色任务列表
                //判断是否已经接取了这个任务
                if (QuestManager.instance.HaveQuest(newTask.questData))
                {
                    //如果判断为完成，则给予奖励
                    if (QuestManager.instance.GetTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    QuestManager.instance.tasks.Add(newTask);
                    QuestManager.instance.GetTask(newTask.questData).IsStart = true;//找到与当前对话任务对应的Task并修改isstart
                }
            }
        }


        if (nextPieceID == "")
        {
            DialogueUI.instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.instance.UpdateMainDialogue(DialogueUI.instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
