using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2AiboControllor : MonoBehaviour
{
    public QuestGiver NPC03qv;
    public QuestGiver NPC04qv;

    // Update is called once per frame
    void Update()
    {
        if (NPC03qv.IsFinsihed&&NPC04qv.IsFinsihed)
        {
            this.gameObject.SetActive(false);
        }
    }
}
