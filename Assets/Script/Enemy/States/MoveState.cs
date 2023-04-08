using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;
    //�ж�����
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    //protected bool isPlayerInMaxAgroRange;

    //ʹ�ù��캯����ʼ��
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

    //�����ĸ���
    public override void Enter()
    {
        base.Enter();//base���ڵ��ø����еĺ����뷽��
        entity.SetVelocity(stateData.movementSpeed);
        //����״̬��ֵ
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
