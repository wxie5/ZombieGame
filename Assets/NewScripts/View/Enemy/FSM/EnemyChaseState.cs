using System;
using View.EnemyView;
using UnityEngine;

public class EnemyChaseState : BaseState
{
    public EnemyChaseState(EnemyBaseView view) : base(view)
    {
    }

    public override Type StateUpdate()
    {
        // if null during chasing, back to idle
        if (view.Target == null) { return typeof(EnemyIdleState); }

        // change to attack
        if (view.InStopDisRange()) { return typeof(EnemyAttackState); }

        // chase state
        view.Agent.SetDestination(view.Target.position);
        view.Anim.SetFloat("ChaseSpeed", view.Agent.velocity.magnitude);
        view.AttackTimer -= Time.deltaTime;
        return typeof(EnemyChaseState);
    }
}
