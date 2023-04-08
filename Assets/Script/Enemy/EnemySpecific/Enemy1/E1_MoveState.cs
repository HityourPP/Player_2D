using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MoveState : MoveState
{
    private Enemy1 enemy;//����ʹ��˽��������Ϊ���ٽ��м̳�

    public E1_MoveState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_MoveState stateData, Enemy1 enemy) : base(stateMachine, entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {//ת��״̬��Idle
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}