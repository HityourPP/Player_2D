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

    public float stunResistance = 3f;//晕眩阻力,当打破这个阻力时，将进入到晕眩状态
    public float stunRecoveryTime = 2f;//晕眩阻力恢复时间，超过这个时间间隔，把晕眩阻力重置

    public float closeRangeActionDistance = 1f;

    public GameObject hitParticle;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
