using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiboControllor : MonoBehaviour
{
    public QuestGiver qv;
    

    // Update is called once per frame
    private void Awake()
    {
        qv = GetComponent<QuestGiver>();
    }

    void Update()
    {
            if (qv.IsFinsihed)
            {
                this.gameObject.SetActive(false);
            }
    }
}
