using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 6f;//冲锋速度
    public float chargeTime = 1f;//冲锋时间
}
