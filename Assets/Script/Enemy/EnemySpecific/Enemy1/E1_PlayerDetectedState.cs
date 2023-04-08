using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    Enemy1 enemy;
    public E1_PlayerDetectedState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_PlayerDetected stateData, Enemy1 enemy) : base(stateMachine, entity, animBoolName, stateData)
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (performLongRangeAction)//超出范围将状态转换为move
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if(!isPlayerInMaxAgroRange)//如果角色不在仇恨范围，转换为寻找状态
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (!isDetectedLedge)//检测到窗台后转向
        {
            entity.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
