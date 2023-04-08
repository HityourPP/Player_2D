using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LookForPlayerStateData", menuName = "Data/State Data/LookForPlayer State")]
public class D_LookForPlayer : ScriptableObject
{
    public int amountOfTurns = 2;//转向寻找的次数
    public float timeBetweenTurns = 0.75f;//转向时间间隔
}
