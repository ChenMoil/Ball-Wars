using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class BallBlackBoard : BlockBorad
{
    public Rigidbody2D rigidbody2D;
    public enum Faction   //判断为左方还是右方
    {
        Right, Left
    }
    public Faction ballFaction;     //球体的阵营

    public float acceleration;   //球体移动的加速度

    public float addAngularVelocity;   //球体移动的加角速度

    public Vector2 initialOrientation;  //球体移动的初始方向
    //public BallInformation(Faction ballFaction, float acceleration, float addAngularVelocity, Vector2 initialOrientation)
    //{
    //    this.ballFaction = ballFaction;
    //    this.acceleration = acceleration;
    //    this.addAngularVelocity = addAngularVelocity;
    //    this.initialOrientation = initialOrientation;
    //}
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
        
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        
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
        ballBlackBoard.rigidbody2D.velocity = ballBlackBoard.initialOrientation;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        ballBlackBoard.rigidbody2D.AddForce(new Vector2(ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.x / Math.Abs(ballBlackBoard.rigidbody2D.velocity.x + ballBlackBoard.rigidbody2D.velocity.y),
            ballBlackBoard.acceleration * ballBlackBoard.rigidbody2D.velocity.y / Math.Abs(ballBlackBoard.rigidbody2D.velocity.x + ballBlackBoard.rigidbody2D.velocity.y)), ForceMode2D.Force);

        ballBlackBoard.rigidbody2D.AddTorque(ballBlackBoard.addAngularVelocity);
    }

    public void OnUpdate()
    {

    }
}
public class BallAi : MonoBehaviour
{
    private FSM fsm;
    public BallBlackBoard ballBlackBoard;

    private void Start()
    {
        ballBlackBoard.rigidbody2D = this.GetComponent<Rigidbody2D>();
        fsm = new FSM(ballBlackBoard);
        fsm.states.Add(StateType.Idle, new AI_IdleState(fsm));
        fsm.states.Add(StateType.Move, new AI_MoveState(fsm));
        fsm.SwitchState(StateType.Move);
    }
    public void Update()
    {
        fsm.OnUpdate();
    }
    public void FixedUpdate()
    {
        fsm.OnFixUpdate();
    }
}
