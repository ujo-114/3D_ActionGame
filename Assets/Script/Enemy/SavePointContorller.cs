using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointContorller : MonoBehaviour
{
    public GameObject[] setDisableOnFight;
    bool IsDisable=false;
    ActorManager am;

    private void Start()
    {
        am = GetComponent<ActorManager>();
    }
    void Update()
    {
        if (am.ac.camcon.isLock && IsDisable == false)
        {
            IsDisable = true;
            for (int i = 0; i < setDisableOnFight.Length; i++)
            {
                setDisableOnFight[i].SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < setDisableOnFight.Length; i++)
        {
            setDisableOnFight[i].SetActive(true);
        }
    }
}
