using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour//该脚本用于实现在动画中获取到状态机的消息
{
    public AttackState attackState;
    private void TriggerAttack()//这两个函数用于在动画中添加事件
    {
        attackState.TriggerAttack();
    }
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
