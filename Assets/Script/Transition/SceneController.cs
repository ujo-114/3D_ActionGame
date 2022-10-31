using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public  GameObject player;
    public GameObject playerPrefeb;
    private CinemachineVirtualCamera followCamera;
    public Animator anmi;
    public SceneFader sceneFaderPrefeb;
    public GameObject inventoryUI;
    public Vector3 playerFlashPoint=Vector3.zero;
    GameObject[] enemys;
    public void Awake()
    {
        CheckGameObject();
        CheckSingle();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)//开启传送
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }



    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)//IEnumeratorでシーンをロードする
    {
        if (SceneManager.GetActiveScene().name != sceneName)//不同场景传送
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            player = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            GameMnager.instance.BindActorManager();
            yield return new WaitForSecondsRealtime(0.1f);
            InventoryManager.instance.equipmentUI.RefreshUI();
            InventoryManager.instance.actionUI.RefreshUI();
            yield break;
        }
        else//同场景传送
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, player.transform.rotation);
            GameMnager.instance.BindActorManager();
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)//获取目的传送门的位置
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
            {
                print(entrances[i].name);
                return entrances[i];
            }

        }
        return null;
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(ContinueLevel(SaveManager.instance.SceneName));
    }
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Snow"));
    }
    public void BackToMainMenu()
    {
        InventoryManager.instance.SavaData();
        QuestManager.instance.SaveQuestManager();
        StartCoroutine(LoadMain());
    }
    public void GoToEnding()
    {
        InventoryManager.instance.SavaData();
        QuestManager.instance.SaveQuestManager();
        StartCoroutine(LoadEnding());
    }

    IEnumerator LoadLevel(string scene)//ファストシーンをロード（开始游戏的场合）
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        if (scene != "")
        {
            anmi.SetTrigger("StandUp");
            yield return new WaitForSecondsRealtime(3.0f);
            yield return StartCoroutine(fade.FadeOut(1.5f));//fade淡入
            yield return SceneManager.LoadSceneAsync(scene);
            inventoryUI.GetComponent<Canvas>().enabled = true;
            GameMnager.instance.BindActorManager();
            yield return new WaitForSecondsRealtime(0.1f);
            player = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");

            InventoryManager.instance.equipmentUI.RefreshUI();
            InventoryManager.instance.actionUI.RefreshUI();
            playerFlashPoint = player.transform.position;
            yield return StartCoroutine(fade.FadeIn(1.5f));//fade淡出
            yield break;
        }
    }
    IEnumerator ContinueLevel(string scene)//ファストシーンをロード（开始游戏的场合）
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        if (scene != "")
        {
            anmi.SetTrigger("StandUp");
            yield return new WaitForSecondsRealtime(3.0f);
            yield return StartCoroutine(fade.FadeOut(1.5f));//fade淡入
            yield return SceneManager.LoadSceneAsync(scene);
            inventoryUI.GetComponent<Canvas>().enabled = true;
            GameMnager.instance.BindActorManager();
            yield return new WaitForSecondsRealtime(0.1f);
            player = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");

            InventoryManager.instance.equipmentUI.RefreshUI();
            InventoryManager.instance.actionUI.RefreshUI();
            if (playerFlashPoint != Vector3.zero)
            {
                player.transform.SetPositionAndRotation(playerFlashPoint, player.transform.rotation);
            }
            else
            {
                playerFlashPoint = player.transform.position;
            }
            yield return StartCoroutine(fade.FadeIn(1.5f));//fade淡出
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        inventoryUI.GetComponent<Canvas>().enabled = false;
        yield return StartCoroutine(fade.FadeOut(2.5f));//fade淡入
        yield return SceneManager.LoadSceneAsync("Main");
        anmi = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>();
        yield return StartCoroutine(fade.FadeIn(1.5f));//fade淡出
        yield break;
    }
    IEnumerator LoadEnding()
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        inventoryUI.GetComponent<Canvas>().enabled = false;
        yield return StartCoroutine(fade.FadeOut(2.5f));//fade淡入
        yield return SceneManager.LoadSceneAsync("Ending");
        yield return StartCoroutine(fade.FadeIn(1.5f));//fade淡出
        yield break;
    }

    IEnumerator ResetFight()
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        yield return StartCoroutine(fade.FadeOut(1.5f));//fade淡入
        ReflashPlayer();
        ReflashEnemy();
        GameMnager.instance.deathUI.SetActive(false);
        InventoryManager.instance.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(fade.FadeIn(0.5f));//fade淡出
        GameMnager.instance.BindActorManager();
        yield break;
    }

    IEnumerator SavePoint(Vector3 savePoint)
    {
        SceneFader fade = Instantiate(sceneFaderPrefeb);
        yield return StartCoroutine(fade.FadeOut(1.0f));
        playerFlashPoint = savePoint;
        yield return StartCoroutine(fade.FadeIn(1.0f));
        yield break;
    }

    public void SaveFlashPoint(Vector3 savePoint)
    {
        StartCoroutine(SavePoint(savePoint));
        player.GetComponent<StateManager>().ResetHP();
        InventoryManager.instance.ResetKusuri();
    }
    public void SetFlashPoint(Vector3 savePoint)
    {
        playerFlashPoint = savePoint;
    }
    public void ReflashPlayer()
    {
        player.GetComponent<StateManager>().ResetHP();
        player.transform.SetPositionAndRotation(playerFlashPoint, player.transform.rotation);
        InventoryManager.instance.ResetKusuri();
    }
    public void ReflashEnemy()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(true);
            Vector3 enemyFlashPoint = enemys[i].GetComponent<EnemyFlashPointSaver>().flashPoint;
            Quaternion enemyFlashRotation= enemys[i].GetComponent<EnemyFlashPointSaver>().flashRotation;
            enemys[i].transform.SetPositionAndRotation(enemyFlashPoint, enemyFlashRotation);
            enemys[i].GetComponent<StateManager>().ResetHP();
        }
    }

    public void ContinueFight()
    {
        StartCoroutine(ResetFight());
        InventoryManager.instance.equipmentUI.RefreshUI();
        InventoryManager.instance.actionUI.RefreshUI();
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
            Destroy(this);
    }
    private void CheckGameObject()
    {
        if (tag == "SceneManager")
        {
            return;

        }
        else
            Destroy(this);
    }

}
