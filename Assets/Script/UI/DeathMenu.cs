using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    Button backBtn;
    Button continueBtn;
    SceneFader fade;
    GameObject deathUI;
    public static DeathMenu instance;
    private void Awake()
    {
        backBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        backBtn.onClick.AddListener(BackToMain);
        continueBtn.onClick.AddListener(ContinueGame);
        fade = GetComponent<SceneFader>();
        deathUI = this.transform.parent.gameObject;
    }

    void BackToMain()
    {
        SceneController.instance.BackToMainMenu();
        StartCoroutine(fade.FadeIn(1.0f));
    }
    void ContinueGame()
    {
        SceneController.instance.ContinueFight();
        
    }


}
