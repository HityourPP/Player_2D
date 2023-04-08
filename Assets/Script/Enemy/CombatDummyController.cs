using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [Header("���˲���")]
    [SerializeField]
    private bool applyKnockback;
    [SerializeField]
    private float knockBackspeedX, knockBackspeedY, knocbackDuration;
    [SerializeField]
    private float knockDeathSpeedX, knockDeathSpeedY, deathTorque;//deathTorque����Ť�أ�ʹ����Խ�����ת
    private bool knockback;
    private float knockbackStart;
    [SerializeField]
    private GameObject hitParticle;//������Ч
    //��������
    private int playerFacingDir;
    private bool playerOnLeft;
    private float currentHealth;
    //��ȡ��Ӧ����
    private PlayerController playerController;
    private GameObject alive, brokenTop, brokenBottom;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBottom;
    private Animator anim;
    private void Start()
    {
        currentHealth = maxHealth;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        //alive = transform.Find("Alive").gameObject;
        //brokenTop = transform.Find("Broken Top").gameObject;
        //brokenBottom = transform.Find("Broken Bottom").gameObject;
        alive = transform.GetChild(0).gameObject;
        brokenTop = transform.GetChild(1).gameObject;
        brokenBottom = transform.GetChild(2).gameObject;

        anim = alive.GetComponent<Animator>();
        rbAlive = alive.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTop.GetComponent<Rigidbody2D>();
        rbBrokenBottom = brokenBottom.GetComponent<Rigidbody2D>();  

        alive.SetActive(true);
        brokenTop.SetActive(false);
        brokenBottom.SetActive(false);
    }
    private void Update()
    {
        CheckKnockback();
    }
    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;//����ֵ��Ӧ��ɫ���˺�ֵ����Ҫ�ʵ�����

        playerFacingDir = playerController.GetFacingDirection();
        //�����һ���Ƕ����ɹ�����Ч
        Instantiate(hitParticle, anim.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        if (playerFacingDir == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }
        anim.SetBool("playerOnLeft", playerOnLeft);
        anim.SetTrigger("damage");

        if (applyKnockback && currentHealth > 0.0f)
        {//���û���Ч��
            Knockback();
        }
        if(currentHealth <= 0.0f)
        {//����
            Die();
        }
    }
    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockBackspeedX * playerFacingDir, knockBackspeedY);
    }
    private void CheckKnockback()
    {
        if (Time.time >= (knockbackStart + knocbackDuration) && knockback)
        {//����֮�����þ�ֹ
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }
    private void Die()
    {
        alive.SetActive(false);
        brokenTop.SetActive(true);
        brokenBottom.SetActive(true);

        brokenTop.transform.position = alive.transform.position;
        brokenBottom.transform.position = brokenTop.transform.position;

        rbBrokenBottom.velocity = new Vector2(knockBackspeedX * playerFacingDir, knockBackspeedY);
        rbBrokenTop.velocity = new Vector2(knockDeathSpeedX * playerFacingDir, knockDeathSpeedY);
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDir, ForceMode2D.Impulse);//����������ʱ������ת
    }
}
