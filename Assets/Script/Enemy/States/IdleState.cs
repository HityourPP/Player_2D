using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool flipAfterIdle;//���õ������ߵ���ͷ��ֹһ��ʱ�䣬֮��ת����ж�
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
        entity.SetVelocity(0f);//����idle״̬���ٶ���Ϊ0
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
        if (Time.time > idleTime + startTime)//startTimeΪÿ��װ����idle״̬���¼ʱ��
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
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);//���������ʱ��
    }
}
