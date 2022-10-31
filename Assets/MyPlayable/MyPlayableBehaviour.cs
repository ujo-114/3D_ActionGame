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
    //��graph��ʼʱ����player
    public override void OnGraphStart(Playable playable)
    {
        pd = (PlayableDirector)playable.GetGraph().GetResolver();//ͨ��playable��ȡDirector����
        foreach (var track in pd.playableAsset.outputs)
        {
            
            if(track.streamName== "Attacker Script"||track.streamName== "Victim Script")//��Attacker Script����϶�attacker���в���
            {
                ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);//��ȡplayerhandle�ϵ�ActorManager
                am.LockActorController();//ͨ��ActorManager��������������
            }
        }
    }
    //��graph����ʱ����player
    public override void OnGraphStop(Playable playable) {
        foreach (var track in pd.playableAsset.outputs)
        {

            if (track.streamName == "Attacker Script" || track.streamName == "Victim Script")//��Attacker Script����϶�attacker���в���
            {
                ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);//��ȡplayerhandle�ϵ�ActorManager
                am.UnlockActorController();//ͨ��ActorManager��������������
            }
        }
    }
}
