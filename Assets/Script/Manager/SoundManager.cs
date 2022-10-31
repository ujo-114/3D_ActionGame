using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip swingAudio, hitAudio,blockedAudio,perfectBlockAudio,moveAudio,arrowAudio,skillAudio,kusuriAudio,executionAudio;
    void Awake()
    {
        CheckGameObject();
        CheckSingle();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }

   public void SwingAudio()
    {
        audioSource.clip = swingAudio;
        audioSource.Play();
    }

    public void HitAudio()
    {
        audioSource.clip = hitAudio;
        audioSource.Play();
    }
    public void BlockedAudio()
    {
        audioSource.clip = blockedAudio;
        audioSource.Play();
    }
    public void PerfectBlockAudio()
    {
        audioSource.clip = perfectBlockAudio;
        audioSource.Play();
    }
    public void MoveAudio()
    {
        audioSource.clip = moveAudio;
        audioSource.Play();
    }
    public void ArrowAudio()
    {
        audioSource.clip = arrowAudio;
        audioSource.Play();
    }
    public void SkillAudio()
    {
        audioSource2.clip = skillAudio;
        audioSource2.Play();
    }
    public void ExecutionAudio()
    {
        audioSource.clip = executionAudio;
        audioSource.Play();
    }
    public void KusuriAudio()
    {
        audioSource.clip = kusuriAudio;
        audioSource.Play();
    }




    private void CheckSingle()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        else
            Destroy(this);
    }
    private void CheckGameObject()
    {
        if (tag == "SoundManager")
        {
            return;

        }
        else
            Destroy(this);
    }
}
