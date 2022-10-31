using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMnager : MonoBehaviour
{
    public ActorManager am;
    public static GameMnager instance;
    public AnimatorOverrideController unequipAnimator;
    public GameObject deathUI;


    void Awake()
    {
        //构造单例
        CheckGameObject();
        CheckSingle();
        BindActorManager();
        deathUI.SetActive(false);
    }

   

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.itemName == "sword")
        {
            am.wm.UnloadWeapon();
            GameObject tempWeapon = CreateSword("sword", am.wm.wh.transform);
            am.wm.UpdateWeaponCollider(tempWeapon);
            am.wm.UpdateWeaponTrail(tempWeapon);
            am.wm.sh.SetActive(true);
        }
        else if (weapon.itemName == "katana")
        {
            am.wm.UnloadWeapon();
            GameObject tempWeapon = CreateKatana("katana", am.wm.wh.transform);
            am.wm.UpdateWeaponCollider(tempWeapon);
            am.wm.UpdateWeaponTrail(tempWeapon);
            am.wm.sh.SetActive(false);
        }
        else if (weapon.itemName == "bow")
        {
            am.wm.UnloadWeapon();
            GameObject tempWeapon = CreateBow("bow", am.wm.wh.transform);
            am.wm.UpdateWeaponCollider(tempWeapon);
            am.wm.sh.SetActive(false);
        }
        am.ac.model.GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        am.ac.anim = am.ac.model.GetComponent<Animator>();
    }

    public void UnEquipment()
    {
        am.wm.UnloadWeapon();
        am.wm.sh.SetActive(false);
        am.ac.model.GetComponent<Animator>().runtimeAnimatorController = unequipAnimator;
        am.ac.anim = am.ac.model.GetComponent<Animator>();
    }

    public void EquipArmor(ItemData_SO armor)
    {
        am.sm.charaterDefensive = armor.defensive;
    }
    public void UnequipArmor()
    {
        am.sm.charaterDefensive = 0;
    }




    public void OnPlayerDie()
    {
        deathUI.SetActive(true);
        SceneFader fade = deathUI.GetComponent<SceneFader>();
        InventoryManager.instance.GetComponent<Canvas>().enabled = false;
        StartCoroutine(fade.FadeOut(1.5f));
    }
    //public void UnloadWeapon()
    //{
    //    am.wm.UnloadWeapon();
    //}




    //创建武器
    public GameObject CreateSword(string weaponName, Transform parent)
    {
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        return obj;
    }
    public GameObject CreateKatana(string weaponName, Transform parent)
    {
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.parent = parent;
        obj.transform.localPosition = new Vector3(0,0.066f,0);
        obj.transform.localRotation = Quaternion.Euler(-90,0,180);
        return obj;
    }
    public GameObject CreateBow(string weaponName, Transform parent)
    {
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0f, -10.794f, 82.576f);
        return obj;
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
        if (tag == "GM")
        {
            return;

        }
        else
            Destroy(this);
    }
    public void BindActorManager()
    {
        am = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ActorManager>();
    }
}
