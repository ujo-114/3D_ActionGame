using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;

    public GameObject senser;
    public GameObject caster;
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;
    public EventCasterManager esm;
    private bool blockLRController = true;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();
        try
        {
            senser = transform.Find("senser").gameObject;
            caster = transform.Find("caster").gameObject;


        }
        catch (System.Exception)
        {

        }
        bm = Bind<BattleManager>(senser);
        wm = Bind<WeaponManager>(ac.model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InteractionManager>(senser);
        esm = Bind<EventCasterManager>(caster);
        ac.OnAction += DoAction;

    }
    private void OnEnable()
    {
        ac.OnAction += DoAction;
    }


    public void DoAction()
    {
        if (im.overlapEcastms.Count != 0)
        {
            if (im.overlapEcastms[0].active == true)
            {

                if (im.overlapEcastms[0].eventName == "frontStab")
                {
                    dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);

                }
                else if (im.overlapEcastms[0].eventName == "openBox")
                {
                    im.overlapEcastms[0].active = false;
                    dm.PlayFrontStab("openBox", this, im.overlapEcastms[0].am);
                }
            }
        }
    }

    //ジェネリックスを使用したmanagerの読み込み
    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempManager;
        if (go == null)
        {
            return null;
        }
        tempManager = go.GetComponent<T>();
        if (tempManager == null)
        {
            tempManager = go.AddComponent<T>();
        }
        tempManager.am = this;
        return tempManager;
    }

    // Update is called once per frame
    public void TryDoDamage(WeaponData targetWd, Vector3 hitPoint)
    {
        if (sm.isCounter)
        {
            targetWd.wc.wm.am.ShieldStunned();
        }
        else if (sm.isImmortal)
        {
            //do nothing
        }
        else if (sm.isPerfectBlock && sm.isDefense == true)
        {
            PerfectBlock();
            if (targetWd.wc != null)
            {
                if (targetWd.wc.wm.am.sm.isFinalAttack)
                {
                    targetWd.wc.wm.am.KatanaStunned();
                }
            }
        }
        else if (sm.isDefense && sm.isAllowDefense && sm.isPerfectBlock == false)
        {
            Blocked();
            sm.calHP(-1 * targetWd.ATK * wm.wcR.wdata.defensive);
            if (sm.HP <= 0)
            {
                Die();
            }
        }

        else
        {
            if (sm.HP > 0)
            {
                sm.calHP(-1 * (targetWd.ATK - sm.charaterDefensive));
                if (sm.HP > 0)
                {
                    if (targetWd.weapenType == WeapenType.greatSword)
                    {
                        HeavyHit();
                    }
                    else
                    {
                        Hit();
                    }

                }
                else
                {
                    Die();
                }
                GameObject.Instantiate(Resources.Load("hit") as GameObject, hitPoint, Quaternion.identity);
            }
        }
    }

    private void PerfectBlock()
    {
        if (blockLRController)
        {
            ac.IssueTrigger("perfectBlock1");
        }
        else
        {
            ac.IssueTrigger("perfectBlock2");
        }
        blockLRController = !blockLRController;
        SoundManager.instance.PerfectBlockAudio();
        GameObject.Instantiate(Resources.Load("blockEffect") as GameObject, wm.Trail.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load("blockShader") as GameObject, wm.Trail.transform.position, Quaternion.identity);
        StartCoroutine(Pause());
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");//通知ac设置hit tirgger
        StartCoroutine(Pause());
        SoundManager.instance.HitAudio();
        ac.MyInpulse.GenerateImpulse();
    }
    public void HeavyHit()
    {
        ac.IssueTrigger("heavyHit");
        StartCoroutine(Pause());
        SoundManager.instance.HitAudio();
        ac.MyInpulse.GenerateImpulse();
    }
    IEnumerator Pause()//使用协程实现冻帧
    {
        float pauseTime = 0.15f;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
    IEnumerator EnemyDie()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        transform.SetPositionAndRotation(new Vector3(0, 0, 0), transform.transform.rotation);
        this.gameObject.SetActive(false);
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        sm.isDie = true;
        if (ac.isAI)
        {
            StartCoroutine(EnemyDie());
        }
        else
        {
            GameMnager.instance.OnPlayerDie();
        }
    }
    public void Blocked()
    {
        ac.IssueTrigger("blocked");
        SoundManager.instance.BlockedAudio();
        
    }
    public void ShieldStunned()
    {
        ac.IssueTrigger("shieldStunned");
        SoundManager.instance.ExecutionAudio();
    }
    public void KatanaStunned()
    {
        ac.IssueTrigger("katanaStunned");
    }

    public void FrontStab()
    {
        sm.calHP(-30.0f);
    }

    public void LockActorController()
    {
        ac.IssueBool("lock", true);
        sm.isImmortal = true;
    }
    public void UnlockActorController()
    {
        ac.IssueBool("lock",false);
        sm.isImmortal = false;
    }
    private void OnDisable()
    {
        if (tag == "Enemy")
        {
            if (QuestManager.instance != null && sm.isDie)
            {
                QuestManager.instance.UpdateQuestProgress(this.name, 1);
            }
        }
    }

}
