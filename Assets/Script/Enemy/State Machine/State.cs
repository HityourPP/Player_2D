using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State//�������״̬
{
    protected FiniteStateMachine stateMachine;//protected������Ϊ˽�У����̳�������Ҳ�ܷ���
    protected Entity entity;

    protected float startTime;
    protected string animBoolName;//�����������е�bool����
    public State(FiniteStateMachine stateMachine, Entity entity, string animBoolName)//ʹ�ù��캯����ʼ��
    {
        this.stateMachine = stateMachine;
        this.entity = entity;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        startTime = Time.time;//�ڵ��øú����ͼ�¼ʱ��
        entity.anim.SetBool(animBoolName, true);//���ö�Ӧ����������
        DoChecks();
    }
    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);//�˳���Ϊfalse
    }
    public virtual void LogicUpdate()//�߼����º���
    {

    }
    public virtual void PhysicsUpdate()//������º���
    {
        DoChecks();
    }
    public virtual void DoChecks()//���ڼ��״̬�ڶ�������ж����ڣ����������ڻ���������麯��
    {

    }
}
