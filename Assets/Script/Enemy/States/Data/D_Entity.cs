using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new EntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHealth = 30f;

    public float damageHopSpeed = 10f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDiatance = 0.4f;
    public float groundCheckDistance = 0.3f;

    public float maxAgroDistance = 4f;
    public float minAgroDistance = 3f;

    public float stunResistance = 3f;//��ѣ����,�������������ʱ�������뵽��ѣ״̬
    public float stunRecoveryTime = 2f;//��ѣ�����ָ�ʱ�䣬�������ʱ����������ѣ��������

    public float closeRangeActionDistance = 1f;

    public GameObject hitParticle;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
