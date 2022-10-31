using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }
    public string sceneName;
    public TransitionType transitionType;

    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canTrans)
        {
            InventoryManager.instance.SavaData();
            QuestManager.instance.SaveQuestManager();
            print("transtion");
            SceneController.instance.TransitionToDestination(this);
        }
    }



    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
            canTrans = true;
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
            canTrans = false;
    }

}
