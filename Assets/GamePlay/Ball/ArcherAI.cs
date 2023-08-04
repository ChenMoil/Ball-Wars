using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

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

    private GameObject targetGameObject;  //目标物体

    private FSM fsm;
    public ArcherAI_MoveState(FSM fsm)
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
            ballBlackBoard.thisBall.transform.DOLookAt(targetGameObject.transform.position, 100000f);  //转向目标物体
        }
        ////加速
        //if (ballBlackBoard.rigidbody2D.velocity != Vector2.zero)
        //{
        //    ballBlackBoard.rigidbody2D.AddForce(new Vector2(ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.x / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y)),
        //    ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.y / (Math.Abs(ballBlackBoard.rigidbody2D.velocity.x) + Math.Abs(ballBlackBoard.rigidbody2D.velocity.y))), ForceMode2D.Force);
        //}
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        {
            if (timer >= switchTime)
            {
                timer = 0;
                float maxDistance = 0;
                foreach (GameObject ball in PlaceSoliderScript.ballGameObjectList)  //切换目标物体
                {
                    Debug.Log(ball.transform.position);
                    if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) > maxDistance)
                    {
                        maxDistance = Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position);
                        targetGameObject = ball;    
                    }
                }
                //PlaceSoliderScript.ballGameObjectList.ForEach(ball =>
                //{
                //    Debug.Log(ball.transform.position);
                //    if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction != ballBlackBoard.ballFaction && Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position) > maxDistance)
                //    {
                //        maxDistance = Vector2.Distance(ball.transform.position, ballBlackBoard.thisBall.transform.position);
                //        targetGameObject = ball;
                //    }
                //});
                Debug.Log(targetGameObject);
            }
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
        fsm.SwitchState(StateType.Move);
    }
}
