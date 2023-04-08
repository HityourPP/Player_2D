using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState //�ýű�����ʵ�ֽ�ս����
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

        attackDetails.damageAmount = stateData.attackDamage;//�Թ���ϸ�ڽ��и�ֵ
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
        //��ȡ������Χ�ڵ�����
        Collider2D[] detectedObject = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);//��ȡ����Բ�������ڵ�������ײ����б�

        foreach (Collider2D item in detectedObject)
        {
            item.transform.SendMessage("Damage",attackDetails);
        }
    }
}
