using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State//定义各种状态
{
    protected FiniteStateMachine stateMachine;//protected本质上为私有，但继承这个类的也能访问
    protected Entity entity;

    protected float startTime;
    protected string animBoolName;//动画器参数中的bool名字
    public State(FiniteStateMachine stateMachine, Entity entity, string animBoolName)//使用构造函数初始化
    {
        this.stateMachine = stateMachine;
        this.entity = entity;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        startTime = Time.time;//在调用该函数就记录时间
        entity.anim.SetBool(animBoolName, true);//设置对应动画器参数
        DoChecks();
    }
    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);//退出设为false
    }
    public virtual void LogicUpdate()//逻辑更新函数
    {

    }
    public virtual void PhysicsUpdate()//物理更新函数
    {
        DoChecks();
    }
    public virtual void DoChecks()//由于检测状态在多个子类中都存在，所以这里在基类中添加虚函数
    {

    }
}
