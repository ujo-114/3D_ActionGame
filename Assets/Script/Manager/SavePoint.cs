using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public bool canSave = false;
    public GameObject savePoint;


    private void Awake()
    {
        savePoint = this.transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSave = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSave = false;
        }
    }


    void Update()
    {
        if (canSave && Input.GetKeyDown(KeyCode.R))
        {
            SceneController.instance.SaveFlashPoint(savePoint.transform.position);
        }
    }
}
