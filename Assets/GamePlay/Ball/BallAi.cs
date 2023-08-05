using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BallBlackBoard : BlockBorad
{
    public Rigidbody2D rigidbody2D;
    public enum Faction   //判断为左方还是右方
    {
        Left = 1, Right = -1    
    }
    public Faction ballFaction;     //球体的阵营

    public float acceleration;   //球体移动的加速度

    public float addAngularVelocity;   //球体移动的加角速度

    [NonSerialized]public Vector2 initialOrientation;  //球体移动的初始方向

    public int HP;                //小球的血量

    public int EXP;               //小球的经验

    [NonSerialized]public GameObject thisBall;  //这个小球
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

    private void Start()
    {
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
        ballBlackBoard.HP -= HP;
        if (ballBlackBoard.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public virtual void initBall() //初始化小球函数
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballBlackBoard.thisBall = this.gameObject;
        fsm = new FSM(ballBlackBoard);
        fsm.states.Add(StateType.Idle, new AI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new AI_MoveState(fsm));
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
