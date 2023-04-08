using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;

    protected bool isPlayerInAgroRange;

    protected bool isDectedLedge;//检测窗台
    protected bool isDetectingWall;//检测墙体
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;//执行近程动作，攻击
    public ChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_ChargeState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks(); 
        isDectedLedge = entity.CheckLedge();//检测墙体与窗台，用于设置冲锋条件，转换状态
        isDetectingWall = entity.CheckWall();
        isPlayerInAgroRange = entity.CheckPlayerInMinAgroRange();//检测是否在仇恨范围内
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.chargeSpeed);//进入该状态将速度设为冲锋时的速度
        isChargeTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >startTime + stateData.chargeTime)//冲刺时间结束
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
