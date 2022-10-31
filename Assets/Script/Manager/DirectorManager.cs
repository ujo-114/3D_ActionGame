using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;

    [Header("=== Timeline assets ===")]
    public TimelineAsset frontStab;
    public TimelineAsset openBox;

    [Header("=== Assets Setting ===")]
    public ActorManager attacker;
    public ActorManager victim;
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void PlayFrontStab(string timelineName, ActorManager attacker, ActorManager victim)
    {
        if (timelineName == "frontStab")
        {
            pd.playableAsset = frontStab;
            attacker.transform.position = victim.transform.position + victim.transform.forward * 1.0f;
            attacker.wm.transform.forward = -victim.transform.forward;
            foreach (var track in pd.playableAsset.outputs)
            {
                if (track.streamName == "attacker animetion")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
                }
                else if (track.streamName == "victim animetion")
                {
                    pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
                }
                if (track.streamName == "Attacker Script")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker);
                }
                else if (track.streamName == "Victim Script")
                {
                    pd.SetGenericBinding(track.sourceObject, victim);
                }

            }
            pd.Play();
            victim.FrontStab();
        }

        else if (timelineName == "openBox")
        {
            pd.playableAsset = openBox;
            attacker.transform.position = victim.transform.position + victim.transform.forward * 1.0f;
            attacker.wm.transform.forward = -victim.transform.forward;
            foreach (var track in pd.playableAsset.outputs)
            {
                if (track.streamName == "Player Animation")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
                }
                else if (track.streamName == "Box Animation")
                {
                    pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
                }
                if (track.streamName == "Player Script")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker);
                }
                else if (track.streamName == "Box Script")
                {
                    pd.SetGenericBinding(track.sourceObject, victim);
                }

            }
            pd.Play();
        }
    }
}
