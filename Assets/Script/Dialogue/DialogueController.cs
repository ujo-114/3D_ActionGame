using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    public bool canTalk = false;


    private void Awake()
    {
        currentData.isDialogueEnd = false;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUI.instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }

    public void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.R))
        {
            OpenDialogue();
            if (tag == "NPC")
            {
                if (QuestManager.instance != null)
                {
                    QuestManager.instance.UpdateQuestProgress(this.name, 1);
                }
            }
        }
    }


    void OpenDialogue()
    {
        //传入对话内容
        DialogueUI.instance.UpdateDialogueData(currentData);
        DialogueUI.instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}
