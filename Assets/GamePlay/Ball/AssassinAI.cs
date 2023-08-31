using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public class AssassinAI_IdleState : IState  //站立状态下执行的函数
{

    public BallBlackBoard ballBlackBoard;

    private FSM fsm;
    public AssassinAI_IdleState(FSM fsm)
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
public class AssassinAI_MoveState : IState  //移动状态下执行的函数
{
    private BallBlackBoard ballBlackBoard;

    private float timer = 3f;  //计时器

    public float minDistance = 10000;   //离得最近的小球的距离

    private GameObject targetGameObject;  //目标物体

    private float switchTargetTime = 2f;  //切换目标的时间

    private FSM fsm;
    public AssassinAI_MoveState(FSM fsm)
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
        if (targetGameObject != null)
        {
            Vector3 force = targetGameObject.transform.position - ballBlackBoard.thisBall.transform.position;
            force = force.normalized;
            force = force * ballBlackBoard.acceleration;
            ballBlackBoard.thisBall.GetComponent<Rigidbody2D>().AddForce(force);
            //加角速度
            ballBlackBoard.rigidbody2D.AddTorque(ballBlackBoard.addAngularVelocity * MathF.Sign(ballBlackBoard.rigidbody2D.angularVelocity));
        }
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= switchTargetTime)
        {
            timer = 0;  //重新计时
            foreach (GameObject ball in BallList.instance.ballGameObjectList)  //切换目标物体
            {
                if (ball == null)
                {
                    continue;
                }
                if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && ball.GetComponent<ArcherAI>() != null && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position);
                    targetGameObject = ball;
                }
            }
            if (minDistance == 10000)  //最短距离没变 -> 没找到远程士兵
            {
                fsm.SwitchState(StateType.Attack);
            }
        }
        minDistance = 10000;      //重置最短距离
    }
}
public class AssassinAI_AttackState : IState  //攻击状态下执行的函数
{
    private BallBlackBoard ballBlackBoard;

    private FSM fsm;
    public AssassinAI_AttackState(FSM fsm)
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
public class AssassinAI : BallAi
{
    public override void initBall()
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballBlackBoard.thisBall = this.gameObject;
        fsm = new FSM(ballBlackBoard as BallBlackBoard);
        fsm.states.Add(StateType.Idle, new AssassinAI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new AssassinAI_MoveState(fsm));
        fsm.states.Add(StateType.Attack, new AssassinAI_AttackState(fsm));
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
}
