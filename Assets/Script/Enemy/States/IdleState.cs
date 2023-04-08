using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool flipAfterIdle;//设置敌人在走到尽头后静止一段时间，之后转向的判定
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;

    protected float idleTime;

    public IdleState(FiniteStateMachine stateMachine, Entity entity, string animBoolName,D_IdleState stateData) : base(stateMachine, entity, animBoolName)
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
        entity.SetVelocity(0f);//进入idle状态将速度设为0
        isIdleTimeOver = false;
        SetRandomIdleTime();
        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    public override void Exit()
    {
        base.Exit();
        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > idleTime + startTime)//startTime为每次装换到idle状态会记录时间
        {
            isIdleTimeOver = true;
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);//设置随机的时间
    }
}
