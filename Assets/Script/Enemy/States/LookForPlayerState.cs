using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayer stateData;

    protected bool turnImmediately;//设置是否立即旋转
    protected bool isPlayerInMinAgroRange;
    protected bool isAllTurnsDone;//所有转向与转向时间结束
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;
    protected int amountOfTurnsDone;
    public LookForPlayerState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_LookForPlayer stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        //进入该状态前需要初始化
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        amountOfTurnsDone = 0;
        lastTurnTime = startTime;

        entity.SetVelocity(0f);//设置速度为0
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (turnImmediately)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)//时间超过转向间隔并且还有转向次数
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }
        if (amountOfTurnsDone >= stateData.amountOfTurns)//转向结束设置所有转向次数已完成
        {
            isAllTurnsDone = true;
        }
        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip;
    }
}
