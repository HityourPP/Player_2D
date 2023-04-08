using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }
    private State currentState;
    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        moveSpeed,
        maxHealth,
        knockbackDuration,
        lastTouchDamageTime,//�����˺���ɫ����ر���
        touchDamageCoolDown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;
    private Vector2 touchDamageBotLeft;
    private Vector2 touchDamageTopRight;

    private AttackDetails attackDetails = new AttackDetails();

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck,
        touchDamageCheck;
    [SerializeField]
    private LayerMask 
        whatIsGround,
        whatIsPlayer
        ;
    [SerializeField]
    private GameObject
        hitParticle,        //������Ч��Ѫ����Ч��ѪҺ������Ч
        deathChunkParticle,
        deathBloodParticle;
    [SerializeField]
    private Vector2 knockbackSpeed;
    private float currentHealth;
    private float knockbackStartTime;

    private Vector2 movement;
    private int facingDir;
    private int damageDir;

    private bool
        groundDetected,
        wallDetected;
    private void Start()
    {
        alive = transform.GetChild(0).gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();
        facingDir = 1;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        UpdateState();
    }
    private void UpdateState()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }
    //---Other Functions------------
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //���ü��Ӵ�λ�õ��ĸ�λ��
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2),
                touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2),
                touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2),
                touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2),
                touchDamageCheck.position.y + (touchDamageHeight / 2));
        //������Ӧλ�õ���
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;
        //�����һ���Ƕ����ɹ�����Ч
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails.position.x > alive.transform.position.x)
        {
            damageDir = -1;
        }
        else
        {
            damageDir = 1;
        }
        //������Ч

        if (currentHealth > 0.0f)//Ѫ���߽������״̬
        {
            SwitchState(State.Knockback);
        }
        else if(currentHealth <= 0.0f)//Ѫ������0��������״̬
        {
            SwitchState(State.Dead);
        }
    }
    private void CheckTouchDamage()
    {
        if(Time.time >= lastTouchDamageTime + touchDamageCoolDown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2),
                touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2),
                touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);//���ü������
            if(hit != null)
            {//�����˺�
                lastTouchDamageTime = Time.time;
                attackDetails.damageAmount = touchDamage;
                attackDetails.position = alive.transform.position;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }
    private void Flip()
    {
        facingDir *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void SwitchState(State state)//�����˳���ǰ״̬�������µ�״̬
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }
    //���״̬����ʹ���и��õĿ���չ��
    //---Moving State------------------
    private void EnterMovingState()
    {

    }
    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)//δ��⵽������⵽ǽ��
        {//ת��
            Flip();
        }
        else
        {//�ƶ�
            movement.Set(moveSpeed * facingDir, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }
    private void ExitMovingState()
    {

    }
    //---Knockback State------------------
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDir, knockbackSpeed.y);//���û��˵��ƶ�
        aliveRb.velocity = movement;
        aliveAnim.SetBool("knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("knockback", false);
    }
    //---Dead State------------------
    private void EnterDeadState()
    {//������Ч
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }
    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

}
