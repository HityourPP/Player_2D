using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetected stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;//执行远程动作
    protected bool performCloseRangeAction;//执行近程动作，攻击

    protected bool isDetectedLedge;
    public PlayerDetectedState(FiniteStateMachine stateMachine, Entity entity, string animBoolName,D_PlayerDetected stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

        isDetectedLedge = entity.CheckLedge();
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
        performLongRangeAction = false;

        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        //isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= startTime + stateData.longRnageActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        //isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
    }
}
