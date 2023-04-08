using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;

    protected bool isStunTimeOver;
    protected bool isOnGround;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    public StunState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_StunState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isOnGround = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isStunTimeOver = false;
        isMovementStopped = false;
        entity.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
        if(isOnGround && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)//击退到地面后将其速度设为0
        {
            entity.SetVelocity(0f);
            isMovementStopped = true;//为true时不再将其检测设置速度为0
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
