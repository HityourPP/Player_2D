using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Entity : MonoBehaviour//作为敌人实例的父类，该类包含实例所需使用的一些方法
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    //定义实例中常用的组件与参数
    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGo { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int lastDamageDirection { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;

    private float currentHealth;//当前血量
    private float currentStunResistance;
    private float lastDamageTime;

    protected bool isStunned;
    protected bool isDead;

    private Vector2 velocityWorkSpace;//用做中间变量

    public virtual void Start()//设置virtual start函数,初始化参数
    {
        aliveGo = transform.Find("Alive").gameObject;
        anim = aliveGo.GetComponent<Animator>();
        rb = aliveGo.GetComponent<Rigidbody2D>();
        stateMachine = new FiniteStateMachine();
        atsm = aliveGo.GetComponent<AnimationToStateMachine>();

        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
    }
    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)//时间超过恢复时间，重置其晕眩阻力
        {
            ResetStunResistance();
        }
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public virtual void SetVelocity(float velocity)//设置移动速度
    {
        velocityWorkSpace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkSpace;
    }
    public virtual void SetVelocity(float velocity,Vector2 angle ,int direction)//重载函数，作为设置其击退速度
    {
        angle.Normalize();//方向归一化
        velocityWorkSpace.Set(angle.x * velocity * direction, angle.y * velocity);//设置速度与方向
        rb.velocity = velocityWorkSpace;
    }
    public virtual bool CheckWall()//检测墙体
    {
        return Physics2D.Raycast(wallCheck.position, aliveGo.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckLedge()//检测窗口
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDiatance, entityData.whatIsGround);
    }
    public virtual bool CheckGround()//检测地面
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckDistance, entityData.whatIsGround);
    }
    public virtual void Flip()//转向
    {
        facingDirection *= -1;
        aliveGo.transform.Rotate(0f, 180f, 0f);//实现转向
    }
    private void OnDrawGizmos()//绘制检测射线
    { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(entityData.wallCheckDistance * facingDirection * Vector2.right));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDiatance));

        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance)), 0.2f);
        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance)), 0.2f);
        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance)), 0.2f);
    }
    public virtual bool CheckPlayerInMinAgroRange()//检测是否在仇恨范围内
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()//检测角色是否在可攻击范围
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;//获得攻击时间

        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;//每次减少这个阻力，当阻力减少到0后进入晕眩状态

        DamageHop(entityData.damageHopSpeed);

        Instantiate(entityData.hitParticle, aliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));//设置攻击特效

        if(attackDetails.position.x > aliveGo.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }
        if (currentStunResistance <= 0)
        {
            isStunned = true;
        }
        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }
    public virtual void ResetStunResistance()//当晕眩过后或者经过一段时间后将其晕眩阻力重置
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }
    public virtual void DamageHop(float velocity)//攻击敌人，击飞敌人
    {
        velocityWorkSpace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkSpace;
    }

}
