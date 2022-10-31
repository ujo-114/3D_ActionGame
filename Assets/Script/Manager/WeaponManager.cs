using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    public Collider weaponColL;//左手武器的碰撞器
    public Collider weaponColR;//右武器的碰撞器
    public GameObject wh;//weapon handle
    public GameObject sh;//shield handle
    public float arrowSpeed;
    private AudioSource audio;
    public  GameObject Trail;
    public TrailRenderer trailRenderer;

    public WeaponController wcL;//左手 WeaponController
    public WeaponController wcR;//右手 WeaponController

    private void Start()
    {
        try
        {
            sh = FintChid("shieldHandle", this.transform).gameObject;
            wh = FintChid("weaponHandle", this.transform).gameObject;
            if (am.ac.isAI == false)
            {
                Trail = FintChid("Trail", this.transform).gameObject;
                trailRenderer = Trail.GetComponent<TrailRenderer>();
                trailRenderer.emitting = false;
            }
           
            
            

            //调用方法绑定WeaponController
            wcL = BindWeaponControlle(sh);
            wcR = BindWeaponControlle(wh);

            weaponColR = wh.GetComponentInChildren<Collider>();
            weaponColR.enabled = false;
            weaponColL = sh.GetComponentInChildren<Collider>();
            weaponColL.enabled = false;

        }
        catch (System.Exception)
        {

        }
        audio = GetComponent<AudioSource>();
    }



    public void UpdateWeaponCollider(GameObject newWeapon)
    {
        weaponColR = newWeapon.GetComponent<Collider>();
        wcR.wdata = newWeapon.GetComponent<WeaponData>();

    }
    public void UpdateWeaponTrail(GameObject newWeapon)
    {

        Trail = FintChid("Trail", newWeapon.transform).gameObject;
        
        trailRenderer = Trail.GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }
    public void UnloadWeapon()
    {
        try
        {
            foreach (Transform tran in wcR.transform)
            {
                weaponColR = null;
                wcR.wdata = null;
                Trail = null;
                Destroy(tran.gameObject);
            }
        }
        catch (System.Exception)
        {

        }
    }
   


    public WeaponController BindWeaponControlle(GameObject targetObj)//绑定WeaponController
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        weaponColR.enabled = true;
    }
    public void WeaponDisable()
    {
        weaponColR.enabled = false;
    }
    //开启无敌
    public void ImmortalOn()
    {
        am.sm.isImmortal = true;
    }
    //关闭无敌
    public void ImmortalOff()
    {
        am.sm.isImmortal = false;
    }
    public void counterEnable()
    {
        am.sm.isCounter = true;
    }
    public void counterDisable()
    {
        am.sm.isCounter = false;
    }
    public void SwingAudioPlay()
    {
        audio.Play();
    }
    public void TrailOn()
    {
        trailRenderer.emitting = true;
    }
    public void TrailOff()
    {
        trailRenderer.emitting = false;
    }
    public void IncreaseHP()
    {
        am.sm.calHP(30.0f);
        SoundManager.instance.KusuriAudio();
    }
    public void SkillEffectOn()
    {
        GameObject skillEffent= Instantiate(Resources.Load("katanaSlash") as GameObject);
        skillEffent.transform.parent = wcR.transform;
        skillEffent.transform.localPosition = Vector3.zero;
        skillEffent.transform.localRotation = Quaternion.identity;
        SoundManager.instance.SkillAudio();
    }
    public void ArrowShoot()
    {
        Transform shootPoint = weaponColR.transform.GetChild(0);
        GameObject prefab = Resources.Load("arrow") as GameObject;
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.position = shootPoint.position;
        go.transform.forward = this.transform.forward;
        //go.GetComponent<Rigidbody>().velocity = this.transform.forward * arrowSpeed;
        SoundManager.instance.ArrowAudio();

    }
    public void ArrowSkill()
    {
        Transform shootPoint = weaponColR.transform.GetChild(0);
        GameObject prefab = Resources.Load("arrowSkill") as GameObject;
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.position = shootPoint.position;
        go.transform.forward = this.transform.forward;
        SoundManager.instance.ArrowAudio();
        //go.GetComponent<Rigidbody>().velocity = this.transform.forward * arrowSpeed;
    }

    public void SwordSkillOn()
    {
        GameObject skillEffent = Instantiate(Resources.Load("swordEffect") as GameObject);
        skillEffent.transform.parent = this.transform;
        skillEffent.transform.localPosition = new Vector3(0,1,0);
        skillEffent.transform.localRotation = Quaternion.identity;
        SoundManager.instance.SkillAudio();
        ShaderOn();
    }

    public void MoveAduioPlay()
    {
        SoundManager.instance.MoveAudio();
    }

    public void ShaderOn()
    {
        GameObject.Instantiate(Resources.Load("blockShader") as GameObject, this.transform.position, Quaternion.identity);
    }
    public void KnightSkillEffect()
    {
        GameObject skillEffent = Instantiate(Resources.Load("knightSwordSlash") as GameObject);
        skillEffent.transform.parent = this.transform;
        skillEffent.transform.localPosition = Vector3.zero;
        skillEffent.transform.localRotation = Quaternion.Euler(0,-90,0);
        ShaderOn();
    }
    public void GreatKnightSkillEffect()
    {
        GameObject skillEffent = Instantiate(Resources.Load("greatSwordKnightSkill") as GameObject);
        skillEffent.transform.parent = this.transform;
        skillEffent.transform.localPosition = Vector3.zero;
        skillEffent.transform.localRotation = Quaternion.identity;
        ShaderOn();
    }
    public void SamuraiSkillOn()
    {
        GameObject skillEffent = Instantiate(Resources.Load("samuraiSkill") as GameObject);
        skillEffent.transform.parent = this.transform;
        skillEffent.transform.localPosition = new Vector3(0, 1.0f, 0);
        skillEffent.transform.localRotation = Quaternion.identity;
        ShaderOn();
    }
    public void FinalAttackOn()
    {
        am.sm.isFinalAttack = true;
    }
    






    //递归找子物件
    public Transform FintChid(string name, Transform parent)
    {
        if (parent.childCount < 1)
        {
            return null;
        }

        Transform t = parent.transform.Find(name);
        if (t != null)
        {
            return t;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            t = FintChid(name, parent.GetChild(i));
            if (t != null)
            {
                break;
            }
        }

        return t;
    }
}
