using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public  Animator anim; 
    public ActorManager am;
    private Vector3 moveMent;
    public CamController camcon;
    public float strandSpeed = 1.4f;
    private float speed;
    private   float Dmag;
    private  Vector3 DVec;
    private Rigidbody rigidbody;
    //private CharacterController characterController;
    private float velocity;
    public bool run;//走る判断
    public float runMultiplier = 2.0f;//走るスピード
    public string runKey;
    private float runAnimController;
    public bool jump;
    public string jumpKey;
    public bool attack;//攻撃判断
    public string attackKey;
    public bool defense;//防御判断
    public string defenseKey;//防御键
    public bool canDefense=true;
    public bool skill;
    public string lockKey;
    public string counterKey; 
    private bool canAction;//イベント判断
    public string actionKey;//事件键
    public float jumphigh = 4.0f;
    public  bool inputEnabled=true;//输入控制
    public  bool moveEnabled = true;
    private bool lockPlaner;//移動方向をロック
    private bool trackDirection = false;//モデルの方向をロック
    float h;
    float v;
    float vh;
    private Vector3 thrustVec;
    private Vector3 thrustHor;
    private  float rollhigh = 3.0f;
    public PhysicMaterial friction1;
    public PhysicMaterial friction0;
    private CapsuleCollider col;
    private bool canAttack;
    private Vector3 deltaPos;//アニメルートポジション
    public bool isAI;
    public bool isBoss;
    public float AttackColdTime=4.5f;
    private  float enemyAttackColdTime;
    public   float enemySkillColdTime = 5.0f;
    private float PerfectBlockTime=0.2f;

    public delegate void OnActionDelegate();//方法挂载接口
    public event OnActionDelegate OnAction;
    public Cinemachine.CinemachineImpulseSource MyInpulse;


    //インプット信号クラス
    public MyButton runK = new MyButton();
    public MyButton jumpK = new MyButton();
    public MyButton attackK = new MyButton();
    public MyButton defenseK = new MyButton();
    public MyButton lockK = new MyButton();
    public MyButton counterK = new MyButton();
    public MyButton actionK = new MyButton();




    void Awake()
    {
        anim = model.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        speed = strandSpeed;
        col = GetComponent<CapsuleCollider>();
        am = GetComponent<ActorManager>();
        MyInpulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {


        
        if (inputEnabled&&isAI==false)//プレーヤーの場合、入力信号は鍵盤から得る
        {
            runK.Tick(Input.GetKey(runKey));
            jumpK.Tick(Input.GetKey(jumpKey));
            attackK.Tick(Input.GetKey(attackKey));
            defenseK.Tick(Input.GetKey(defenseKey));
            lockK.Tick(Input.GetKey(lockKey));
            counterK.Tick(Input.GetKey(counterKey));
            actionK.Tick(Input.GetKey(actionKey));
            jump = jumpK.OnPressed;
            attack = attackK.OnReleased&&attackK.IsDalying;
            skill = attackK.IsPressing && !attackK.IsDalying;
            if(canDefense)
            defense = defenseK.IsPressing;
            //完美弹反的输入
            if (defenseK.OnPressed&&canDefense&&am.wm.weaponColR!=null&&am.wm.wcR.wdata.weapenType==WeapenType.katana)
            {
                StartCoroutine(PerfectBlockController());

            }
        }



        if (isAI && inputEnabled&&moveEnabled)//AIの場合は、プログラムで信号を制御
        {
            AIAction();
        }


        //ジャンプ、ロール部分
        if (jump)
        {
            anim.SetTrigger("jump");
        }
        //転倒を誘発するほどのスピードで落下した場合
        if (rigidbody.velocity.magnitude > 9.0f)
        {
            anim.SetTrigger("roll");
        }
        //攻击部分
        if (attack && (CheckState("ground")||CheckStateTag("attack"))&&canAttack)
        {
            anim.SetTrigger("attack");
            
        }
        //防御部分
        if (defense)//防御に入るときは、防御層の重さを1にして、攻撃をOFFにする
        {
            if (CheckStateTag("block") == false)
            {
                float currentWeight2 = anim.GetLayerWeight(anim.GetLayerIndex("defense"));
                currentWeight2 = Mathf.Lerp(currentWeight2, 1.0f, 0.1f);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), currentWeight2);
                canAttack = false;
            }
            else
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            canAttack = true;
        }



        anim.SetBool("defense", defense);
        //锁定部分
        if (lockK.OnPressed)
        {
            camcon.LockSet();
        }
        //盾反部分
        if (counterK.OnPressed)
        {
            anim.SetTrigger("counter");
        }
        //事件部分
        if (actionK.OnPressed&&canAction)
        {
            OnAction.Invoke();//调用挂载在OnAction上的所有方法
        }
        //スキル部、攻撃時に長押しでスキル発動
        if (skill)
        {
            print("skill ON");
            anim.SetTrigger("skill");
        }
        if (am.sm.isDie)
        {
            inputEnabled = false;
        }

    }
    private void FixedUpdate()
    {
        rigidbody.position += deltaPos;
        move();
        deltaPos = Vector3.zero;

    }
    void move()
    {

        if (moveEnabled==true&&isAI==false)
        {
            v = Input.GetAxis("Vertical");    //水平速度
            h = Input.GetAxis("Horizontal");
            vh = Mathf.Sqrt((v * v) + (h * h));
            Dmag = Mathf.Clamp(vh, 0, 1);
            DVec = v * transform.forward + h * transform.right;
            run = runK.IsPressing;
        }
        
        if (run)
        {
            runAnimController = 2.0f;
        }
        else
        {
            runAnimController = 1.0f;
        }


        if (camcon.isLock == false)
        {
            anim.SetFloat("forward", Dmag * Mathf.Lerp(anim.GetFloat("forward"), runAnimController, 0.15f));
            anim.SetFloat("right", 0);
        }
        else//
        {
            anim.SetFloat("forward", v * runAnimController);
            anim.SetFloat("right", h * runAnimController);
        }
        if (camcon.isLock == false)//ロックされていない場合は、モデルを入力方向に向け
        {
            if (Dmag >= 0.1f)
            {
                Vector3 targetForward = h * transform.right + v * transform.forward;
                model.transform.forward = Vector3.Slerp(model.transform.forward, targetForward, 0.2f);//slerpでモデルの向きを制御
            }
            if (lockPlaner == false)
                moveMent = Dmag * model.transform.forward * speed * ((run) ? runMultiplier : 1.0f);  
        }
        else//如果有锁定，则根据是否翻滚判断朝向
        {
            if (trackDirection == false)//ロールコマンドなし、敵の方向を向く
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, transform.forward, 0.2f);
            }
            else//ロールオーバーコマンド入力時、ロールオーバー入力方向向き
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward,DVec.normalized, 0.2f);
            }
            if (lockPlaner == false)
            {
                moveMent = DVec * speed * ((run) ? runMultiplier : 1.0f);
            }
        }
        rigidbody.velocity = new Vector3(moveMent.x, rigidbody.velocity.y, moveMent.z) + thrustVec + thrustHor;
        thrustVec = Vector3.zero;
        thrustHor = Vector3.zero;
    }

    void AIAction()
    {
        Vector3 playerDirection = camcon.Player.transform.position - model.transform.position;

        float targetAngle = Vector3.Angle(playerDirection, model.transform.forward);
        float targetDistance = Vector3.Distance(model.transform.position, camcon.Player.transform.position);
        if (camcon.isLock == false)
        {
            v = 0;
            attack = false;
        }
        else
        {
            if (targetDistance >= 7.0f)
            {
                v = 2;
                DVec = v * transform.forward * speed;
                anim.SetFloat("forward", v);
            }
            else if (targetDistance < 7.0f && targetDistance >= 2.0f)
            {
                v = 1;
                DVec = v * transform.forward * speed;
                anim.SetFloat("forward", v);
                if (enemySkillColdTime <= 0)
                {
                    v = 0;
                    anim.SetTrigger("skill");
                    enemySkillColdTime = 4.0f;
                }
            }
            else if (targetDistance < 2.0 && enemyAttackColdTime <= 0)
            {
                v = 0;
                DVec = Vector3.zero;
                if (isBoss)
                {
                    int attackType = Random.Range(0, 2);
                    if (attackType < 1)
                    {
                        anim.SetTrigger("attack");
                    }
                    else
                    {
                        anim.SetTrigger("attack2");
                    }
                }
                else
                {
                    anim.SetTrigger("attack");
                }
                enemyAttackColdTime = Random.Range(AttackColdTime-1.0f,AttackColdTime);
            }

            enemyAttackColdTime -= Time.deltaTime;
            enemySkillColdTime -= Time.deltaTime;
        }
    }


    public bool CheckState(string stateName,string layerName="Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }

    IEnumerator PerfectBlockController()
    {
        am.sm.isPerfectBlock = true;
        yield return new WaitForSecondsRealtime(PerfectBlockTime);
        am.sm.isPerfectBlock = false;
    }


    public void OnJumpEnter()//ジャンプ状態に入るときに呼び出される
    {
        inputEnabled = false;
        thrustVec = new Vector3(0, jumphigh, 0);
        lockPlaner = true;
        canAttack = false;
        trackDirection = true;
        moveEnabled = false;
    }
    public void OnJumpExit()//ジャンプ状態に出るときに呼び出される
    {
        speed = strandSpeed;
        canAttack = true;
    }
    public void isGround()
    {
        anim.SetBool("isGround", true);
        col.material = friction1;
    }
    public void isNotGround()
    {
        anim.SetBool("isGround", false);
        col.material = friction0;
    }
    public void OnGroundEnter()
    {
        inputEnabled = true;
        lockPlaner = false;
        canAttack = true;
        moveEnabled = true;
        trackDirection = false;
        canDefense = true;
        canAction = true;
        am.sm.isCounter = false;
        am.sm.isFinalAttack = false;
        if (am.wm.weaponColR!=null)
        am.wm.weaponColR.enabled = false;
        if (am.sm.HP <= 0)
        {
            am.Die();
        }
    }
    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollhigh, 0);
        moveMent = Vector3.zero;
        moveEnabled = false;
        lockPlaner = true;
        canAttack = false;
        canDefense = false;
        trackDirection = true;
    }
    public void OnRollUpdate()
    {
        thrustHor = model.transform.forward * anim.GetFloat("RollSpeed") * 3.6f;
    }
    public void OnAttackRollUpdate()
    {
        thrustHor = -model.transform.forward * anim.GetFloat("RollSpeed") * 3.2f;
    }

    public void OnJabEnter()
    {
        inputEnabled = false;
        lockPlaner = true;
        canAttack = false;
    }
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward*anim.GetFloat("jabVelocity");
    }
    public void OnAttackEnter()
    {
        Dmag = 0;
        DVec = Vector3.zero;
        moveEnabled = false;
        canDefense = false;
    }
    
    public void OnRunAttackEnter()
    {
        moveEnabled = false;
        inputEnabled = false;
        lockPlaner = true;
    }
    public void OnAttackIdleEnter()
    {
        lockPlaner = false;
        moveEnabled = true;
    }
    public void OnAttackUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attackVelocity");
    }
    public void OnAttackIdleUpdate()
    {
        if (defense == false)//防御なし時は、防御層の重みを0にする
        {
            float currentWeight2 = anim.GetLayerWeight(anim.GetLayerIndex("defense"));
            currentWeight2 = Mathf.Lerp(currentWeight2, 0f, 0.2f);
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), currentWeight2);
        }
    }
    public void OnHitEnter()
    {
        inputEnabled = false;
        moveEnabled = false;
        DVec = Vector3.zero;
        Dmag = 0;
        am.wm.weaponColR.enabled = false;
        enemyAttackColdTime = 0.3f;
        //anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
    }
    public void OnStunnedEnter()
    {
        inputEnabled = false;
        moveEnabled = false;
        DVec = Vector3.zero;
        Dmag = 0;
        am.wm.weaponColR.enabled = false;
    }

    public void OnDieEnter()
    {
        lockPlaner = false;
        moveEnabled = false;
        inputEnabled = false;
        Dmag = 0;
        DVec = Vector3.zero;
        if (camcon.isLock == true)
        {
            camcon.LockSet();
        }
    }
    public void OnLockEnter()
    {
        moveEnabled = false;
        canAction = false;
        Dmag = 0;
        DVec = Vector3.zero;
    }

    public void OnStunnedIdleEnter()
    {
        am.esm.active = true;
    }
    public void OnStunnedIdleExit()
    {
        am.esm.active = false;
    }

    public void OnBlockEnter()
    {
        moveEnabled = false;
        DVec = Vector3.zero;
        Dmag = 0;
        am.wm.weaponColR.enabled = false;
        anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
    }
    public void OnBlockExit()
    {
        canDefense = true;
    }
    public void ResetAttackCD()
    {
        enemyAttackColdTime = Random.Range(2.0f, 3.0f);
        print(enemyAttackColdTime);
    }





    public void IssueTrigger(string tiggerName)//统一设置动画器trigger,在actor manager脚本中调用
    {
        anim.SetTrigger(tiggerName);
    }
    public void IssueBool(string boolName,bool value)
    {
        anim.SetBool(boolName,value);
    }
    public void OnUpdateRM(object _deltaPos)//ルートアニメーションの変位を取得
    {
        if (CheckStateTag("attack"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }
}
