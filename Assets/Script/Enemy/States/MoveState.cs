using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;
    //判断条件
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    //protected bool isPlayerInMaxAgroRange;

    //使用构造函数初始化
    public MoveState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_MoveState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    //添加类的覆盖
    public override void Enter()
    {
        base.Enter();//base用于调用父类中的函数与方法
        entity.SetVelocity(stateData.movementSpeed);
        //进入状态赋值
        //isDetectingLedge = entity.CheckLedge();
        //isDetectingWall = entity.CheckWall();
        ////isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //isDetectingLedge = entity.CheckLedge();
        //isDetectingWall = entity.CheckWall();
        //isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }
}
