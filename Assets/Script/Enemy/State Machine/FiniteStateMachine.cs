using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine//����״̬�������ڸ��ĸ���״̬
{
    public State currentState { get; private set; }
    
    public void Initialize(State startingState)//��ʼ��
    {
        currentState = startingState;
        currentState.Enter();
    }
    public void ChangeState(State newState)//�ı�״̬
    {
        currentState.Exit();//���˳���ǰ״̬
        currentState = newState;
        currentState.Enter();//�����µ�״̬
    }

}
