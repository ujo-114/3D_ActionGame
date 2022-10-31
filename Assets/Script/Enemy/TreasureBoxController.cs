using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxController : MonoBehaviour
{
    public GameObject treasureBox;
    public ActorManager am;


    public void Start()
    {
        am = GetComponent<ActorManager>();
        //treasureBox.SetActive(false);
    }

    //public void Update()
    //{
    //    if (am.sm.HP <= 0)
    //    {
    //        treasureBox.SetActive(true);
    //    }
    //}
    private void OnDisable()
    {
        treasureBox.SetActive(true);
    }
}
