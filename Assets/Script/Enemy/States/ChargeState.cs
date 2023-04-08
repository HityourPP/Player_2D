using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;

    protected bool isPlayerInAgroRange;

    protected bool isDectedLedge;//��ⴰ̨
    protected bool isDetectingWall;//���ǽ��
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;//ִ�н��̶���������
    public ChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_ChargeState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks(); 
        isDectedLedge = entity.CheckLedge();//���ǽ���봰̨���������ó��������ת��״̬
        isDetectingWall = entity.CheckWall();
        isPlayerInAgroRange = entity.CheckPlayerInMinAgroRange();//����Ƿ��ڳ�޷�Χ��
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.chargeSpeed);//�����״̬���ٶ���Ϊ���ʱ���ٶ�
        isChargeTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >startTime + stateData.chargeTime)//���ʱ�����
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
