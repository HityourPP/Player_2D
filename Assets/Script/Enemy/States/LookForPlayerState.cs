using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayer stateData;

    protected bool turnImmediately;//�����Ƿ�������ת
    protected bool isPlayerInMinAgroRange;
    protected bool isAllTurnsDone;//����ת����ת��ʱ�����
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
        //�����״̬ǰ��Ҫ��ʼ��
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        amountOfTurnsDone = 0;
        lastTurnTime = startTime;

        entity.SetVelocity(0f);//�����ٶ�Ϊ0
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
        else if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)//ʱ�䳬��ת�������һ���ת�����
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }
        if (amountOfTurnsDone >= stateData.amountOfTurns)//ת�������������ת����������
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
