using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState //该脚本用于实现近战攻击
{
    protected D_MeleeAttack stateData;

    protected AttackDetails attackDetails = new AttackDetails();
    public MeleeAttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosition, D_MeleeAttack stateData) : base(stateMachine, entity, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;//对攻击细节进行赋值
        attackDetails.position = entity.aliveGo.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        //获取攻击范围内的物体
        Collider2D[] detectedObject = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);//获取落在圆形区域内的所有碰撞体的列表

        foreach (Collider2D item in detectedObject)
        {
            item.transform.SendMessage("Damage",attackDetails);
        }
    }
}
