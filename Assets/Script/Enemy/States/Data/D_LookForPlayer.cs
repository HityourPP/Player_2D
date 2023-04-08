using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LookForPlayerStateData", menuName = "Data/State Data/LookForPlayer State")]
public class D_LookForPlayer : ScriptableObject
{
    public int amountOfTurns = 2;//ת��Ѱ�ҵĴ���
    public float timeBetweenTurns = 0.75f;//ת��ʱ����
}
