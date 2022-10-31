using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CastleDirectorController : MonoBehaviour
{
    public PlayableDirector pd;
    public QuestData_SO S2Q1;
    public QuestData_SO S2Q2;
    bool isPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.GetTask(S2Q1) != null && QuestManager.instance.GetTask(S2Q2) != null && isPlayed == false)
        {
            pd.Play();
            isPlayed = true;
        }
    }
}
