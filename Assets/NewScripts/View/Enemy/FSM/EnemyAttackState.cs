using System;
using View.EnemyView;
using UnityEngine;

public class EnemyAttackState : BaseState
{
    public EnemyAttackState(EnemyBaseView view) : base(view)
    {
    }

    public override Type StateUpdate()
    {
        // change to idle
        if (view.Target == null) { return typeof(EnemyIdleState); }

        // change to chase
        if (!view.InAttackRange()) { return typeof(EnemyChaseState); }

        // attack state
        if (view.AttackTimer <= 0f)
        {
            view.AttackView();
            view.AttackTimer = view.Model.CurAttackRate;
        }
        view.Agent.SetDestination(view.Target.position);
        view.Anim.SetFloat("ChaseSpeed", view.Agent.velocity.magnitude);
        view.AttackTimer -= Time.deltaTime;
        return typeof(EnemyAttackState);
    }
}
