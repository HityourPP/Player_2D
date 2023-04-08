using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;

    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    public AttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosition) : base(stateMachine, entity, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        entity.atsm.attackState = this;//初始化
        isAnimationFinished = false;
        entity.SetVelocity(0f);//进入攻击状态将速度设为0
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
    }
    public virtual void TriggerAttack()//进行攻击
    {

    }
    public virtual void FinishAttack()//检测攻击结束，在攻击动画结束后添加事件调用
    {
        isAnimationFinished = true;
    }
}
