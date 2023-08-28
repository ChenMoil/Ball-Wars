using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BallBlackBoard : BlockBorad
{
    [NonSerialized]public Rigidbody2D rigidbody2D;
    public enum Faction   //判断为左方还是右方
    {
        Left = 1, Right = -1    
    }
    public Faction ballFaction;     //球体的阵营

    public float acceleration;   //球体移动的加速度

    public float addAngularVelocity;   //球体移动的加角速度

    [NonSerialized]public Vector2 initialOrientation;  //球体移动的初始方向

    public int HP;                //小球的血量

    [NonSerialized]public GameObject thisBall;  //这个小球

    public GameObject Weapon;  //这个小球所带的武器

    [NonSerialized] public SpriteRenderer spriteRenderer;  //精灵组件
    public Sprite normalSprite;    //正常情况下的精灵
    public Sprite beAttackSprite;  //被攻击的精灵
}

public class AI_IdleState : IState  //站立状态下执行的函数
{
    public BallBlackBoard ballBlackBoard;

    private FSM fsm;
    public AI_IdleState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }
    public void OnEnter()
    {
        if (ballBlackBoard.ballFaction == BallBlackBoard.Faction.Right)
        {
            ballBlackBoard.thisBall.transform.rotation = Quaternion.Euler(0, 0, 180);  
        }
        ballBlackBoard.rigidbody2D.freezeRotation = true;   //冻结旋转
    }

    public void OnExit()
    {
        ballBlackBoard.rigidbody2D.freezeRotation = false;  //取消冻结旋转
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        ballBlackBoard.rigidbody2D.velocity = Vector2.zero; //物体速度为0
    }
}
public class AI_MoveState : IState  //移动状态下执行的函数
{
    private BallBlackBoard ballBlackBoard;

    private FSM fsm;
    public AI_MoveState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }

    public void OnEnter()
    {
        //加初始速度
        ballBlackBoard.initialOrientation.x = (float)ballBlackBoard.ballFaction;
        ballBlackBoard.initialOrientation.y = Random.Range(-0.5f, 0.5f);
        ballBlackBoard.rigidbody2D.velocity = ballBlackBoard.initialOrientation;
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        //加速
        if (ballBlackBoard.rigidbody2D.velocity != Vector2.zero)
        {
            ballBlackBoard.rigidbody2D.AddForce(new Vector2(ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.x / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y)),
            ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.y / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y))), ForceMode2D.Force);
        }
        //加角速度
        ballBlackBoard.rigidbody2D.AddTorque(ballBlackBoard.addAngularVelocity * MathF.Sign(ballBlackBoard.rigidbody2D.angularVelocity));
    }

    public void OnUpdate()
    {

    }
}
public class BallAi : MonoBehaviour
{
    public FSM fsm;
    public BallBlackBoard ballBlackBoard;

    private List<Coroutine> beHurtIEnumerators = new List<Coroutine>();  //储存受伤运行的协程
    private bool isDead;  //小球是否死亡

    private void Start()
    {
        ballBlackBoard.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        initBall();
    }
    public void Update()
    {
        fsm.OnUpdate();
    }
    public void FixedUpdate()
    {
        fsm.OnFixUpdate();
    }
    public void DeductHP(int HP) //扣血函数
    {
        foreach(Coroutine beHurt in beHurtIEnumerators)  //结束之前的协程
        {
            StopCoroutine(beHurt);
        }
        beHurtIEnumerators.Clear(); //清除协程列表
        ballBlackBoard.HP -= HP;
        if (ballBlackBoard.HP <= 0)
        {
            Dead();
            //Debug.Log(ballBlackBoard.thisBall.name);
            return;
        }
        Coroutine a = StartCoroutine(BeHurtSprite());
        Coroutine b = StartCoroutine(BeHurtColor());
        beHurtIEnumerators.Add(a);
        beHurtIEnumerators.Add(b);
    }
    IEnumerator BeHurtSprite() //改变小球表情
    {
        ballBlackBoard.spriteRenderer.sprite = ballBlackBoard.beAttackSprite;
        yield return new WaitForSeconds(1.5f);
        ballBlackBoard.spriteRenderer.sprite = ballBlackBoard.normalSprite;
    }
    IEnumerator BeHurtColor() //改变小球颜色
    {
        float timer = 0;
        ballBlackBoard.spriteRenderer.material.SetColor("_spriteColor", new Color32(255, 105, 105, 255));
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            ballBlackBoard.spriteRenderer.material.SetColor("_spriteColor", new Color32(255, (byte)(105 + timer * 100), (byte)(105 + timer * 100), 255));
            yield return null;
        }
        ballBlackBoard.spriteRenderer.material.SetColor("_spriteColor", new Color32(255, 255, 255, 255));
    }
    public void Dead()   //小球死亡函数，死了后变尸体
    {
        if (isDead)
        {
            return;
        }
        else
        {
            isDead = true;
        }
        //切换到死亡状态
        fsm.SwitchState(StateType.Dead);
        //从数据结构中删除
        BallList.instance.ballGameObjectList.Remove(gameObject);
        BallList.instance.ballBlackBoards.Remove(gameObject);
        gameObject.layer = LayerMask.NameToLayer("DeadBody"); //更改物体的Layer层
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);     //更改物体大小
        gameObject.GetComponent<SpriteRenderer>().material.SetColor("_spriteColor", new Color32(100, 100, 100, 255));  //更改颜色
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;  //改图层的排序顺序
        gameObject.name = "Dead";
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {
            //遍历当前物体及其所有子物体
            tran.gameObject.layer = LayerMask.NameToLayer("DeadBody"); //更改物体的Layer层
            if (tran.gameObject.name != "Trail")
            {
                tran.gameObject.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255); //更改颜色
                tran.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;  //改图层的排序顺序
            }
            //tran.localScale = new Vector3(0.8f, 0.8f, 0.8f);  //更改物体大小
        }

        GameEnd.CheckGameEnd(ballBlackBoard.ballFaction);       //检测战斗是否结束
    }
    public virtual void initBall() //初始化小球函数
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballBlackBoard.thisBall = this.gameObject;
        fsm = new FSM(ballBlackBoard);
        fsm.states.Add(StateType.Idle, new AI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new AI_MoveState(fsm));
        fsm.states.Add(StateType.Dead, new AI_Dead(fsm));
        BallList.instance.ballBlackBoards.Add(gameObject, ballBlackBoard);  //添加进黑板小球物体对应字典
        if (!BallList.instance.ballGameObjectList.Contains(gameObject)) //列表中没该小球就加入
        {
            BallList.instance.ballGameObjectList.Add(gameObject);
        }
        if (gameObject.transform.parent == null || gameObject.transform.parent.gameObject.name != "BallList")  //存放小球的物体中没该小球就加入
        {
            gameObject.transform.SetParent(GameObject.Find("BallList").transform);
        }
        ChangeState();
    }
    public void ChangeState()
    {
        if (BallList.instance.sceneType == BallList.SceneType.Test)
        {
            fsm.SwitchState(StateType.Move);
        }
        else if (BallList.instance.sceneType == BallList.SceneType.level)
        {
            fsm.SwitchState(StateType.Idle);
        }
    }
}

public class AI_Dead : IState
{
    public FSM fsm;
    public BallBlackBoard ballBlackBoard;
    public AI_Dead(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        ballBlackBoard.rigidbody2D.velocity = Vector2.zero; //物体速度为0
        ballBlackBoard.rigidbody2D.angularVelocity = 0;     //角速度为0
    }
}
