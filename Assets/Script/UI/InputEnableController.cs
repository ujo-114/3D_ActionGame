using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnableController : MonoBehaviour
{
    public GameObject QuestPanel;
    public GameObject DialoguePanel;
    public GameObject InventoryPanel;

    public static InputEnableController instance;
    public bool isPanelOpen;

    void Awake()
    {
        //构造单例
        CheckSingle();
    }

    private void Update()
    {
        isPanelOpen = QuestPanel.activeInHierarchy || DialoguePanel.activeInHierarchy || InventoryPanel.activeInHierarchy;
        if (isPanelOpen)
        {
            GameMnager.instance.am.ac.inputEnabled = false;
        }
        else
        {
            if(GameMnager.instance.am!=null)
            GameMnager.instance.am.ac.inputEnabled = true;
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
}
