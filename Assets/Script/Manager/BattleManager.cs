using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]//赋予一个碰撞器
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defCol;
    // Start is called before the first frame update
    void Start()
    {
        //设定受击碰撞器的初始值
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up ;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;

    }

    private void OnTriggerEnter(Collider col)
    {
        WeaponData targetWd = col.GetComponent<WeaponData>();
        Vector3 hitPoint = col.ClosestPointOnBounds(transform.position);

        if (col.tag == "Weapon")
        {
            if (targetWd.isEnemyWeapon && am.ac.isAI)
            {
                //do nothing(关闭友军伤害
            }
            else
            {
                am.TryDoDamage(targetWd, hitPoint);
            }

            if (am.ac.camcon.isLock == false)
            {
                am.ac.camcon.LockTargetAI();
            }
        }
    }
}
