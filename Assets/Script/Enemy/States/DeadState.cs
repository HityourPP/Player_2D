using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    public DeadState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_DeadState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Object.Instantiate(stateData.deathBloodParticle, entity.aliveGo.transform.position, stateData.deathBloodParticle.transform.rotation);
        Object.Instantiate(stateData.deathChunkParticle, entity.aliveGo.transform.position, stateData.deathChunkParticle.transform.rotation);
    
        entity.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
