using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine//有限状态机，用于更改各种状态
{
    public State currentState { get; private set; }
    
    public void Initialize(State startingState)//初始化
    {
        currentState = startingState;
        currentState.Enter();
    }
    public void ChangeState(State newState)//改变状态
    {
        currentState.Exit();//先退出当前状态
        currentState = newState;
        currentState.Enter();//进入新的状态
    }

}
