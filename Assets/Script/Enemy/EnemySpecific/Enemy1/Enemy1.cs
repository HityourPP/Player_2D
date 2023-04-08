using System.Collections.Generic;
using UnityEngine;
public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }
    public E1_LookForPlayer lookForPlayerState { get; private set; }
    public E1_MeleeAttackState meleeAttackState { get; private set; }
    public E1_StunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;
    public override void Start()
    {
        base.Start();
        //初始化状态，实例化状态类
        moveState = new E1_MoveState(stateMachine, this, "move", moveStateData, this);
        idleState = new E1_IdleState(stateMachine, this, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(stateMachine, this, "playerDetected", playerDetectedData, this);
        chargeState = new E1_ChargeState(stateMachine, this, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayer(stateMachine, this, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E1_MeleeAttackState(stateMachine, this, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E1_StunState(stateMachine, this, "stun", stunStateData, this);
        deadState = new E1_DeadState(stateMachine, this, "dead", deadStateData, this);

        stateMachine.Initialize(idleState);//初始设为移动状态
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }
    //private void OnDrawGizmos()//绘制攻击范围
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    //}
}
