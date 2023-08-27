using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public class ArcherAI_IdleState : IState  //站立状态下执行的函数
{
    public BallBlackBoard ballBlackBoard;

    private FSM fsm;
    public ArcherAI_IdleState(FSM fsm)
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
public class ArcherAI_MoveState : IState  //移动状态下执行的函数
{
    private BallBlackBoard ballBlackBoard;

    private float timer = 0;  //计时器

    private float switchTime = 1f; //切换目标所需时间

    private float attackTimer = 0;  //转换到攻击状态计时器

    private GameObject targetGameObject;  //目标物体

    private FSM fsm;

    public bool firstRandom = true;   //第一次随机

    float randomAttackTime = 0;
    public ArcherAI_MoveState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }

    public void OnEnter()
    {
        //ballBlackBoard.Arrow = ballBlackBoard.thisBall.transform.GetChild(0).gameObject;  //绑定箭
        //ballBlackBoard.Arrow.SetActive(false);
        //加初始速度
        ballBlackBoard.initialOrientation.x = -(float)ballBlackBoard.ballFaction;
        ballBlackBoard.initialOrientation.y = Random.Range(-0.5f, 0.5f);
        ballBlackBoard.rigidbody2D.velocity = ballBlackBoard.initialOrientation;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        //if (targetGameObject != null)
        //{
        //    LookAt(targetGameObject, ballBlackBoard.thisBall);  //转向目标物体
        //}
        //加速
        if (ballBlackBoard.rigidbody2D.velocity != Vector2.zero)
        {
            ballBlackBoard.rigidbody2D.AddForce(new Vector2(ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.x / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y)),
            ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.y / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y))), ForceMode2D.Force);
        }
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        if (timer >= switchTime)
        {
            timer = 0;
            float minDistance = 10000;
            foreach (GameObject ball in BallList.instance.ballGameObjectList)  //切换目标物体
            {
                if (ball == null || ball == ballBlackBoard.thisBall)
                {
                    continue;
                }
                if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position);
                    targetGameObject = ball;
                }
            }
            Debug.Log(targetGameObject);
        }
        if (firstRandom)
        {
            randomAttackTime = Random.Range(ballBlackBoard.thisBall.GetComponent<ArcherAI>().attackSwitchTime - 1f, ballBlackBoard.thisBall.GetComponent<ArcherAI>().attackSwitchTime + 1f);
            firstRandom = false;
        }
        if (attackTimer >= randomAttackTime)
        {
            attackTimer = 0;
            firstRandom = true;
            fsm.SwitchState(StateType.Attack);
        }
    }

    public static void LookAt(GameObject target, GameObject self)    //朝向其他物体
    {
        Vector3 dir = target.transform.position - self.transform.position;
        dir.z = 0;
        float angle =
            Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);                     //利用角度得到rotation
        self.transform.rotation = rotation;
        //self.transform.eulerAngles =
        //    Vector3.Lerp(self.transform.eulerAngles, new Vector3(0, 0, angle), 0.1f);
    }
}
public class ArcherAI_AttackState : IState  //攻击状态下执行的函数
{
    public BallBlackBoard ballBlackBoard;

    private FSM fsm;

    private GameObject targetGameObject;  //目标物体

    public float attackTimer = 0; //射箭计时器

    public float minDistance = 10000;

    public ArcherAI_AttackState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }
    public void OnEnter()
    {  
        List<GameObject> enemyGameObject = new List<GameObject>();   //转向其中的随机物体
        ballBlackBoard.Weapon.GetComponent<Bow>().OpenArrowPic(); //显示箭图片
        minDistance = 10000;  //得到最近物体后重置
        foreach (GameObject ball in BallList.instance.ballGameObjectList)  //切换目标物体
        {
            if (ball == null)
            {
                continue;
            }
            if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) < minDistance)
            {
                enemyGameObject.Add(ball);
            }
        }
        int aRandom = Random.Range(0, enemyGameObject.Count);
        if (aRandom < enemyGameObject.Count)
        {
            targetGameObject = enemyGameObject[aRandom];
        }
        else if(enemyGameObject.Count > 0)
        {
            targetGameObject = enemyGameObject[0];
        }
        enemyGameObject.Clear();
    }

    public void OnExit()
    {
        ballBlackBoard.Weapon.GetComponent<Bow>().CloseArrowPic();  //取消显示箭图片
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        //ballBlackBoard.rigidbody2D.velocity = Vector2.zero; //物体速度为0
        attackTimer += Time.deltaTime;
        if (attackTimer > ballBlackBoard.thisBall.GetComponent<ArcherAI>().shootTime)
        {
            ballBlackBoard.Weapon.GetComponent<Bow>().Archery(); //射箭
            fsm.SwitchState(StateType.Move);
            attackTimer = 0;
        }
        if (targetGameObject != null)
        {
            ArcherAI_MoveState.LookAt(targetGameObject, ballBlackBoard.thisBall);  //转向目标物体
        }
    }
}
public class ArcherAI : BallAi
{
    public float shootTime;  //射出箭所需的时间

    public float attackSwitchTime; //切换攻击状态所需时间
    public override void initBall()
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballBlackBoard.thisBall = this.gameObject;
        fsm = new FSM(ballBlackBoard as BallBlackBoard);
        fsm.states.Add(StateType.Idle, new ArcherAI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new ArcherAI_MoveState(fsm));
        fsm.states.Add(StateType.Attack, new ArcherAI_AttackState(fsm));
        fsm.states.Add(StateType.Dead, new AI_Dead(fsm));
        BallList.instance.ballBlackBoards.Add(gameObject, ballBlackBoard);  //添加进黑板小球物体对应字典
        if (!BallList.instance.ballGameObjectList.Contains(gameObject)) //列表中没该小球就加入
        {
            BallList.instance.ballGameObjectList.Add(gameObject);
            if (gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction == BallBlackBoard.Faction.Left)
            {
                BallList.instance.leftBallNum++;
            }
            else if (gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction == BallBlackBoard.Faction.Right)
            {
                BallList.instance.rightBallNum++;
            }
        }
        if (gameObject.transform.parent == null || gameObject.transform.parent.gameObject.name != "BallList")  //存放小球的物体中没该小球就加入
        {
            gameObject.transform.SetParent(GameObject.Find("BallList").transform);
        }
        ChangeState();
    }
}
