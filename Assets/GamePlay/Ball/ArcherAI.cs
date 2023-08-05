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
            ballBlackBoard.thisBall.transform.localScale = new Vector3(-1, 1, 1);
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

    private float switchTime = 1; //切换目标所需时间

    private float attackTimer = 0;  //转换到攻击状态计时器

    private float attackSwitchTime = 1; //切换攻击状态所需时间

    private GameObject targetGameObject;  //目标物体

    private FSM fsm;
    public ArcherAI_MoveState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }

    public void OnEnter()
    {
        ////加初始速度
        //ballBlackBoard.initialOrientation.x = (float)ballBlackBoard.ballFaction;
        //ballBlackBoard.initialOrientation.y = Random.Range(-0.5f, 0.5f);
        //ballBlackBoard.rigidbody2D.velocity = ballBlackBoard.initialOrientation;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        if (targetGameObject != null)
        {
            LookAt(targetGameObject, ballBlackBoard.thisBall);  //转向目标物体
        }
        //加速
        if (ballBlackBoard.rigidbody2D.velocity != Vector2.zero)
        {
            ballBlackBoard.rigidbody2D.AddForce(new Vector2(-ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.x / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y)),
            -ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.y / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y))), ForceMode2D.Force);
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
                if (ball == null)
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
        if (attackTimer >= attackSwitchTime)
        {
            fsm.SwitchState(StateType.Attack);
        }
    }

    public void LookAt(GameObject target, GameObject self)    //朝向其他物体
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

    public float shootTime;  //射出箭所需的时间

    public float attackTimer = 0; //射箭计时器

    public float minDistance = 10000;
    public ArcherAI_AttackState(FSM fsm)
    {
        this.fsm = fsm;
        this.ballBlackBoard = fsm.blockBorad as BallBlackBoard;
    }
    public void OnEnter()
    {
        minDistance = 10000;  //得到最近物体后重置
        foreach (GameObject ball in BallList.instance.ballGameObjectList)  //切换目标物体
        {
            if (ball == null)
            {
                continue;
            }
            if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position);
                targetGameObject = ball;
            }
        }
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
        attackTimer += Time.deltaTime;
        if (attackTimer > shootTime)
        {

        }
    }
}
public class ArcherAI : BallAi
{
    public override void initBall()
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballBlackBoard.thisBall = this.gameObject;
        fsm = new FSM(ballBlackBoard);
        fsm.states.Add(StateType.Idle, new ArcherAI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new ArcherAI_MoveState(fsm));
        fsm.states.Add(StateType.Attack, new ArcherAI_AttackState(fsm));
        fsm.SwitchState(StateType.Move);
    }
}
