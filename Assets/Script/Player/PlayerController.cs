using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Transform groundCheck;
    public Transform wallCheck;

    private float moveInputDir;
    public float moveSpeed = 10f;
    public float moveSpeedInit = 10f;
    public float jumpforce = 16f;
    public float wallSlideSpeed = 3f;//ǽ�������ٶ�
    public float movementForceInAir = 50f;//���ÿ�����Ծת�Ƹ�������
    public float airDragMultiplier = 0.95f;//������Ծʱ����������ϵ��
    public float variableJumpHeight = 0.5f;//���õ㰴��Ծ����Ծϵ��
    [Header("��ǽ")]
    public float wallHopForce;//��ǽ����
    public float wallJumpForce;//��ǽ����
    public Vector2 wallHopDir;//ȷ����ǽ�ϵ���ķ���
    public Vector2 wallJumpDir;
    private int facingDirection = 1;
    [Header("��ⷶΧ")]
    public float groundCheckRadius;//�����ⷶΧ
    public float wallCheckDistance;//ǽ�ڼ�����
    public LayerMask ground;
    [Header("��Ծ�Ľ�")]
    private float jumpTimer;
    public float jumpTimerSet = 0.15f;//�������
    //��ǽ���ӳ�
    public float turnTimerSet = 0.1f;
    private float turnTimer;
    //����
    private float wallJumpTimer;//��ǽ��������ʱ��
    public float wallJumpTimerSet = 0.5f;
    private int lastWallJumpDirection;

    public int amountOfJump = 1;//������Ծ����
    public int amountOfJumpLeft;//����ʣ����Ծ����
    [Header("������������")]
    public Transform ledgeCheck;
    public float ledgeCheckDistance;//������
    private bool isTouchingLedge;
    private bool canClimbLedge;
    private bool ledgeDetected;
    private Vector2 ledgePosBot;//ȡ����������Ledgecheck��λ��
    private Vector2 ledgePos1;
    public Vector2 ledgePos2;//��ɫ��������ʱ��󵽴��λ��

    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;

    [Header("��̲�������")]   
    public float dashSpeed;//����ٶ�
    public float distanceBetweenImage;//��Ӱ֮��ļ��
    public float dashCoolDown;//�����ȴʱ��
    public float dashTime = 0.3f;//��̳���ʱ��

    private bool isDashing;
    private float dashTimeLeft;//��¼����һ�γ�̵�ʱ��
    private float lastImagePos;
    private float lastDash = -100;//��¼��һ�γ��ʱ��
    [Header("��ɫ���˲�������")]
    public float knockbackDuration;//���˳���ʱ��
    public Vector2 knockbackSpeed;//�����ٶ�

    private float knockbackStartTime;
    private bool knockback;
    [Header("��������")]
    public GameObject bag;
    private bool isOpening;

    [Header("�ж�����")]
    public bool isFacingRight = true;
    public bool isTouchingWall;
    public bool isWallSliding;
    public bool isWalking;
    public bool canJump;
    public bool isGround;
    public bool isAttemptingToJump;
    public bool canNormalJump;
    public bool canWallJump;
    private bool checkJumpMultiplier;//�ж��Ƿ�Ϊ����
    public bool canMove;
    public bool canFlip;
    private bool hasWallJump;
    public bool allowTurn;
    public bool CanJump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpLeft = amountOfJump;
        wallHopDir.Normalize();//��һ��
        wallJumpDir.Normalize();
        allowTurn = true;
        moveSpeedInit = moveSpeed;
        CanJump = true;
    }
    void Update()
    {
        CheckInput();
        SwitchAnim();
        CheckIfWallSliding();
        if (CanJump)
        {
            CheckIfCanJump();
            CheckJump();
        }
        CheckIfLedgeClimb();
        CheckDash();
        CheckKnockback();
        OpenBag();
    }
    private void FixedUpdate()
    {       
        CheckMoveDirection();
        ApplyMovement();
        CheckSurroundings();   
        CheckIsWalking();  
    }
    private void SwitchAnim()//ת����������
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGround", isGround);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    private void CheckMoveDirection()//�ı䳯��ͬʱҲ�ܸı����ߵĳ���
    {
        if (allowTurn)
        {
            if (isFacingRight && moveInputDir > 0)
            {
                Flip();
            }
            else if(!isFacingRight&&moveInputDir < 0)
            {
                Flip();
            }
        }
    }
    public void DisableFlip()//�ڶ����¼��е���
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()//�ı䳯��ʵ�ֺ���
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180f, 0f);
        }
    }
    private void CheckIfWallSliding()//����Ƿ���ǽ�ϻ���
    {
        if (isTouchingWall && moveInputDir == facingDirection && rb.velocity.y < 0 && !canClimbLedge)//�������ķ������ɫ������ͬ 
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckIsWalking()//����Ƿ����ƶ�
    {
        if (Mathf.Abs(rb.velocity.x) > 0.01f)
            isWalking = true;
        else
            isWalking = false;
    }
    private void CheckSurroundings()//�������߼����Χ����
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);//���·�������
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, ground);//���ҷ�������
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, ledgeCheckDistance, ground);
        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected= true;
            ledgePosBot = ledgeCheck.position;
        }
    }
    private void OnDrawGizmosSelected()//��ʾ���������ķ�Χ
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
    private void CheckIfLedgeClimb()//����Ƿ�������
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if (!isFacingRight)//������������λ�õ�ƫ�ƣ����ֶ����������Ի�����Ч��
            {//Mathf.Floor(float f)����Ϊ����С�ڵ���f���������
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, 
                                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2,
                                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {//Mathf.Ceil(float f)����Ϊ���ش��ڵ���f����С����
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1,
                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset2,
                                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            canMove = false;
            canFlip = false;
            rb.velocity = Vector2.zero;//��������ʱ��ֹ
            moveSpeed = 0f;//��������ʱ�������ƶ�
            allowTurn = false;//��������ʱ����ת��
            isTouchingWall = false;
            canWallJump = false;
            CanJump = false;
            canNormalJump = false;
            rb.gravityScale = 0f;
            transform.position=new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
            anim.SetBool("canClimbLedge", canClimbLedge);
        }
    }
    public void FinishLedgeClimb()//���ڲ��뵽���������У��ڶ���������ִ��
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        allowTurn = true;
        canNormalJump = true;
        CanJump = true;
        ledgeDetected = false;
        rb.gravityScale = 8f;
        moveSpeed = moveSpeedInit;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }
    private void CheckInput()//������룬��������������������
    {
        moveInputDir = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if(isGround || (amountOfJumpLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)//��ɫ��ǽʱ������ͣ�ƶ��뷭ת
        {
            if(!isGround && moveInputDir != facingDirection)
            {
                canFlip = false;
                canMove = false;
                turnTimer = turnTimerSet;//����һ����ʱ��������з�Ӧʱ��
            }
        }
        if (turnTimer >= 0)//����һ����ʱ������������ʱ���з�Ӧ���
        {
            turnTimer -= Time.deltaTime;
            if(turnTimer <= 0)
            {
                canFlip = true;
                canMove = true;
            }
        }
        if (checkJumpMultiplier && !Input.GetButton("Jump"))//����Ƿ�Ϊ�㰴��Ծ�����Ľ�֮ǰ�ļ�ⷽ��
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeight);//�㰴ʱ����Ծ�߶Ƚ���
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();
            }
        }
    } 
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImagePos = transform.position.x;
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;
                if(Mathf.Abs(transform.position.x - lastImagePos) > distanceBetweenImage)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImagePos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }

        }
    }
    private void CheckIfCanJump()//����Ƿ��ܽ�����Ծ
    {
        if (isGround && rb.velocity.y <= 0.01f)//�ڵ����ϲ��Ҳ��ڽ�����Ծ
        {
            amountOfJumpLeft = amountOfJump;
        }
        if (isTouchingWall)//�ж��Ƿ�����ǽ
        {
            canWallJump = true;
        }
        if (amountOfJumpLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }
    //private void Jump()//��Ծ����
    //{
    //    if(canJump && !isWallSliding)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    //        amountOfJumoLeft--;
    //    }
    //    else if (isWallSliding && moveInputDir == 0 && canJump)//�����ƶ�ʱ
    //    {
    //        isWallSliding = false;
    //        amountOfJumoLeft--;
    //        Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDir.x * -facingDirection, wallHopForce * wallHopDir.y);
    //        rb.AddForce(forceToAdd, ForceMode2D.Impulse);//ģʽ��Ϊ�������
    //    }
    //    else if ((isWallSliding || isTouchingWall) && moveInputDir!=0 && canJump)//�����ƶ�ʱ
    //    {
    //        isWallSliding = false;
    //        amountOfJumoLeft--;
    //        Vector2 forceToAdd = new Vector2(wallHopForce * wallJumpDir.x * -moveInputDir, wallHopForce * wallJumpDir.y);
    //        rb.AddForce(forceToAdd, ForceMode2D.Impulse);//ģʽ��Ϊ�������
    //    }
    //}
    private void CheckJump()//�ж�����ǽ����ƽ����Ծ
    {
        if (jumpTimer > 0)
        {
            if (!isGround && isTouchingWall && moveInputDir != 0 && moveInputDir != facingDirection)
            {
                WallJump();//���ж�ִ����ǽ
            }
            else if (isGround)
            {
                NormalJump();
            }
        }
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;//ִ����Ծʱ����ʱ�����
        }
        if (wallJumpTimer > 0)//����һ��ʱ����ͣ�䴹ֱ�����ƶ�
        {
            if(hasWallJump && moveInputDir == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                hasWallJump= false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJump = false;
            }
            else
            {
                wallJumpTimer-= Time.deltaTime;
            }
        }
    }
    private void NormalJump()//��ͨ��Ծ����
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            amountOfJumpLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }
    private void WallJump()//��ǽ����
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);//����ˮƽ������ٶ�
            isWallSliding = false;
            amountOfJumpLeft = amountOfJump;
            amountOfJumpLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallJumpDir.x * -moveInputDir, wallHopForce * wallJumpDir.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);//ģʽ��Ϊ�������
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJump = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }
    private void ApplyMovement()//�����ٶȣ�����������ƶ�
    {
        if(!isGround && !isWallSliding && moveInputDir == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }        
        else if(canMove && !knockback)
        {
            rb.velocity = new Vector2(moveInputDir * moveSpeed, rb.velocity.y);
        }
        //else if (!isGround && !isWallSliding && moveInputDir != 0)
        //{
        //    Vector2 addForce = new(movementForceInAir * moveInputDir, 0);//���һ����
        //    rb.AddForce(addForce);

        //    if (Mathf.Abs(rb.velocity.x) > moveSpeed)//����ӵ��ٶȹ��󣬽�����Ϊԭ�����ƶ��ٶ�
        //    {
        //        rb.velocity = new Vector2(moveInputDir * moveSpeed, rb.velocity.y);
        //    }
        //}
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        //if (moveInputDir != 0)//�����䳯��
        //{
        //   rb.transform.localScale = new Vector3(moveInputDir, 1, 1);
        //}
    }
    public int GetFacingDirection()
    {
        return facingDirection;
    } 
    public bool GetDashStatus()//���س��״̬�����ڳ��ʱ�����޷��˺���ɫ
    {
        return isDashing;
    }
    public void Knockback(int direction)
    {//���ý�ɫ����
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }
    private void CheckKnockback()
    {
        if(Time.time >= (knockbackStartTime + knockbackDuration) && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    public void OpenBag()//�򿪱���
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpening = !isOpening;
            bag.SetActive(isOpening);
        }
    }

}
