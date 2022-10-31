using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float HPMax = 100.0f;
    public float HP;
    public float charaterDefensive = 0;

    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounter = false;
    public bool isPerfectBlock = false;
    public bool isFinalAttack = false;


    public bool isAllowDefense;
    public bool isImmortal;//是否无敌



    void Start()
    {
        ResetHP();
    }

    void Update()
    {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isAttack = am.ac.CheckStateTag("attack");
        isHit = am.ac.CheckState("hit");
        isBlocked = am.ac.CheckState("blocked");
        isDefense = am.ac.CheckState("defense", "defense");
        isDie= am.ac.CheckState("death");

        isAllowDefense = isGround || isBlocked;



    }
    public void calHP(float value)//加减HP值
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);//使用事件更新血条显示
    }

    public void ResetHP()
    {
        HP = HPMax;
        am.ac.IssueTrigger("resurrect");
    }

}
