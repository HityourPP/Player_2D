using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_StunState : StunState
{
    private Enemy1 enemy;
    public E1_StunState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_StunState stateData, Enemy1 enemy) : base(stateMachine, entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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
        if (isStunTimeOver)//晕眩时间结束后
        {
            if (performCloseRangeAction)//若还在近战攻击范围内
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if (isPlayerInMinAgroRange)//若在仇恨范围，转换为向角色冲刺
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else//否则返回到寻找玩家的状态
            {
                enemy.lookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
