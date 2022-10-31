using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    private GameObject CharacterHandle;
    private ActorManager am;
    public GameObject Player;
    public float CameraSpeed = 1024;
    private float CameraH;//左右视角
    private float CameraV;//上下视角
    private float tempEulerX;
    private GameObject model;//获取模型对象
    public LockTarget lockTarget;
    public Image lockDot;
    public bool isLock;
    public bool isAI;
    public AnimatorOverrideController fightAnimator;
    void Awake()
    {
        CharacterHandle = transform.parent.gameObject;
        tempEulerX = 20;
        model = CharacterHandle.GetComponent<ActorController>().model;
        lockDot.enabled = false;
        isLock = false;
        am = CharacterHandle.GetComponent<ActorManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {
            if (!isAI)//プレイヤーの場合、マウスでカメラを制御
            {
                if (am.ac.inputEnabled)
                {
                    CameraH = Input.GetAxis("Mouse X");
                    CameraV = Input.GetAxis("Mouse Y");
                }
                else
                {
                    CameraH = 0;
                    CameraV = 0;
                }
                Vector3 tempModelEuler = model.transform.eulerAngles;
                CharacterHandle.transform.Rotate(Vector3.up, CameraH * CameraSpeed * Time.fixedDeltaTime);
                tempEulerX -= CameraV * CameraSpeed * Time.fixedDeltaTime;
                tempEulerX = Mathf.Clamp(tempEulerX, -10, 30);
                transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
                //锁死模型朝向
                model.transform.eulerAngles = tempModelEuler;
            }
            else
            {
                LockSetAI();
            }
        }
        else//锁定目标
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            CharacterHandle.transform.forward = tempForward;
            transform.LookAt(lockTarget.obj.transform.position + new Vector3(0, 1.0f, 0));
            if (!isAI)
            {
                lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHight, 0));
            }
        }

        if (lockTarget != null)
        {
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) >= 15.0f)//遠すぎる場合はロック解除
            {
                lockTarget = null;
                lockDot.enabled = false;
                isLock = false;
            }
            if (lockTarget != null && lockTarget.am.sm.isDie)//敵が死亡するとアンロックされる
            {
                lockTarget = null;
                lockDot.enabled = false;
                isLock = false;
            }
            if (lockTarget != null && am.sm.isDie)//自分が死亡するとアンロックされる
            {
                lockTarget = null;
                lockDot.enabled = false;
                isLock = false;
            }
        }
    }
    public void LockSet()//敵にロックする
    {
        if (lockTarget == null)
        {
            Vector3 boxCenter = transform.position + transform.forward * 10.0f;
            Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(1.0f, 1.0f, 10.0f), transform.rotation, LayerMask.GetMask("enemy"));
            if (cols.Length == 0)
            {
                lockTarget = null;
                lockDot.enabled = false;
                isLock = false;
            }
            else
            {
                foreach (var col in cols)
                {
                    lockTarget = new LockTarget(col.gameObject, 1.0f);
                    break;
                }
                lockDot.enabled = true;
                isLock = true;
            }
        }
        else
        {
            lockTarget = null;
            lockDot.enabled = false;
            isLock = false;
        }
    }
    public void LockSetAI()//AIのカメラを制御
    {
        if (isAI)
        {
            Vector3 playerDirection = Player.transform.position - model.transform.position;
            float targetAngle = Vector3.Angle(playerDirection, model.transform.forward);
            float targetDistance = Vector3.Distance(model.transform.position, Player.transform.position);
            if (targetDistance <= 5.0f && targetAngle > -30.0f && targetAngle < 30.0f)//与玩家距离小于5，正面与玩家夹角小于60度时，锁定
            {
                LockTargetAI();
            }
        }
    }

    public void LockTargetAI()
    {
        if (isAI)
        {
            lockTarget = new LockTarget(Player, 1.0f);
            isLock = true;
            am.ac.model.GetComponent<Animator>().runtimeAnimatorController = fightAnimator;
            am.ac.anim = am.ac.model.GetComponent<Animator>();
        }
    }
}





    public class LockTarget
    {
        public GameObject obj;
        public float halfHight;
        public ActorManager am;

        public LockTarget(GameObject _obj,float _halfHight)
        {
            obj = _obj;
            halfHight = _halfHight;
            am = _obj.GetComponent<ActorManager>();
        }
    }

