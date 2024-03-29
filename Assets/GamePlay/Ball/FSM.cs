﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum StateType
{
    Idle,
    Move,
    Attack,
    Dead,
}

//各个状态所执行的函数
public interface IState  
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
    void OnFixedUpdate();
}

[Serializable]
public class BlockBorad
{

}
public class FSM  //有限状态机
{
    public IState curState;  //当前状态

    public Dictionary<StateType, IState> states;  //用字典将状态与状态执行的函数连通

    public BlockBorad blockBorad;

    public FSM(BlockBorad blockBorad)
    {
        states = new Dictionary<StateType, IState>();
        this.blockBorad = blockBorad;
    }

    public void AddState(StateType stateType, IState state)
    {
        if (states.ContainsKey(stateType))
        {
            Debug.Log("已经存在状态" + stateType + "无法在进行添加");
            return;
        }
        states.Add(stateType, state);
    }

    public void SwitchState(StateType stateType)
    {
        if (!states.ContainsKey(stateType))
        {
            Debug.Log("无法切换，不存在状态" + stateType);
        }
        if (curState != null)
        {
            curState.OnExit();
        }
        curState = states[stateType];
        curState.OnEnter();
    }

    public void OnUpdate()
    {
        curState.OnUpdate();
    }
    public void OnFixUpdate()
    {
        curState.OnFixedUpdate();
    }
}
