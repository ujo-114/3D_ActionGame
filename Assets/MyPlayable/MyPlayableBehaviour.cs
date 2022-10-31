using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviour : PlayableBehaviour
{
    public Camera myCamera;
    public float myFloat;

    PlayableDirector pd;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
    //在graph开始时上锁player
    public override void OnGraphStart(Playable playable)
    {
        pd = (PlayableDirector)playable.GetGraph().GetResolver();//通过playable获取Director对象
        foreach (var track in pd.playableAsset.outputs)
        {
            
            if(track.streamName== "Attacker Script"||track.streamName== "Victim Script")//在Attacker Script轨道上对attacker进行操作
            {
                ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);//获取playerhandle上的ActorManager
                am.LockActorController();//通过ActorManager上锁动画控制器
            }
        }
    }
    //在graph结束时解锁player
    public override void OnGraphStop(Playable playable) {
        foreach (var track in pd.playableAsset.outputs)
        {

            if (track.streamName == "Attacker Script" || track.streamName == "Victim Script")//在Attacker Script轨道上对attacker进行操作
            {
                ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);//获取playerhandle上的ActorManager
                am.UnlockActorController();//通过ActorManager解锁动画控制器
            }
        }
    }
}
