using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject guideUI;
    public Button guideCloseBtn;



    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    Button guideBtn;
    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        guideBtn = transform.GetChild(4).GetComponent<Button>();
        quitBtn.onClick.AddListener(QuitGame);
        newGameBtn.onClick.AddListener(NewGame);
        continueBtn.onClick.AddListener(ContinueGame);
        guideBtn.onClick.AddListener(OpenGuide);
        guideCloseBtn.onClick.AddListener(CloseGuide);
    }

    void NewGame()
    {
        PlayerPrefs.DeleteAll();
        //加载初始数据
        InventoryManager.instance.LoadTemplateData();
        QuestManager.instance.tasks= new List<QuestManager.QuestTask>();
        SceneController.instance.TransitionToFirstLevel();
    }
    void ContinueGame()
    {
        InventoryManager.instance.LoadData();
        QuestManager.instance.LoadQuestManager();
        SceneController.instance.TransitionToLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void OpenGuide()
    {
        guideUI.SetActive(true);
    }
    void CloseGuide()
    {
        guideUI.SetActive(false);
    }


}
