using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Entity : MonoBehaviour//��Ϊ����ʵ���ĸ��࣬�������ʵ������ʹ�õ�һЩ����
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    //����ʵ���г��õ���������
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

    private float currentHealth;//��ǰѪ��
    private float currentStunResistance;
    private float lastDamageTime;

    protected bool isStunned;
    protected bool isDead;

    private Vector2 velocityWorkSpace;//�����м����

    public virtual void Start()//����virtual start����,��ʼ������
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

        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)//ʱ�䳬���ָ�ʱ�䣬��������ѣ����
        {
            ResetStunResistance();
        }
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public virtual void SetVelocity(float velocity)//�����ƶ��ٶ�
    {
        velocityWorkSpace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkSpace;
    }
    public virtual void SetVelocity(float velocity,Vector2 angle ,int direction)//���غ�������Ϊ����������ٶ�
    {
        angle.Normalize();//�����һ��
        velocityWorkSpace.Set(angle.x * velocity * direction, angle.y * velocity);//�����ٶ��뷽��
        rb.velocity = velocityWorkSpace;
    }
    public virtual bool CheckWall()//���ǽ��
    {
        return Physics2D.Raycast(wallCheck.position, aliveGo.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckLedge()//��ⴰ��
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDiatance, entityData.whatIsGround);
    }
    public virtual bool CheckGround()//������
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckDistance, entityData.whatIsGround);
    }
    public virtual void Flip()//ת��
    {
        facingDirection *= -1;
        aliveGo.transform.Rotate(0f, 180f, 0f);//ʵ��ת��
    }
    private void OnDrawGizmos()//���Ƽ������
    { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(entityData.wallCheckDistance * facingDirection * Vector2.right));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDiatance));

        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance)), 0.2f);
        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance)), 0.2f);
        Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance)), 0.2f);
    }
    public virtual bool CheckPlayerInMinAgroRange()//����Ƿ��ڳ�޷�Χ��
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()//����ɫ�Ƿ��ڿɹ�����Χ
    {
        return Physics2D.Raycast(playerCheck.position, aliveGo.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;//��ù���ʱ��

        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;//ÿ�μ���������������������ٵ�0�������ѣ״̬

        DamageHop(entityData.damageHopSpeed);

        Instantiate(entityData.hitParticle, aliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));//���ù�����Ч

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
    public virtual void ResetStunResistance()//����ѣ������߾���һ��ʱ�������ѣ��������
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }
    public virtual void DamageHop(float velocity)//�������ˣ����ɵ���
    {
        velocityWorkSpace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkSpace;
    }

}
