using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    //public bool currentDialogueIsEnd=false;
    [Header("Base Elements")]
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    public RectTransform optionPanel;
    public GameObject optionPrefab;


    [Header("Data")]
    public DialogueData_SO currentData;
    public int currentIndex = 0;






    void Awake()
    {
        //构造单例
        CheckGameObject();
        CheckSingle();
        nextButton.onClick.AddListener(ContinueDialogue);
    }
 

    void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        else
        {
            dialoguePanel.SetActive(false);
            currentData.isDialogueEnd = true;
            //currentDialogueIsEnd = true;
        }
    }



    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
        currentData.isDialogueEnd = false;
        //currentDialogueIsEnd = false;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }
        mainText.text = "";
        mainText.DOText(piece.text, 1.0f);//使用DOTween控制文本显示效果


        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        //创建options
        CreateOptions(piece);

    }

    void CreateOptions(DialoguePiece piece)
    {
        if (optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel).GetComponent<OptionUI>();
            option.UpdateOption(piece,piece.options[i]);
        }
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
        if (name == "Dialogue Canvas")
        {
            return;

        }
        else
            Destroy(this);
    }
}
