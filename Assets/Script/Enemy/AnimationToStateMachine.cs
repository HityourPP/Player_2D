using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour//�ýű�����ʵ���ڶ����л�ȡ��״̬������Ϣ
{
    public AttackState attackState;
    private void TriggerAttack()//���������������ڶ���������¼�
    {
        attackState.TriggerAttack();
    }
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
