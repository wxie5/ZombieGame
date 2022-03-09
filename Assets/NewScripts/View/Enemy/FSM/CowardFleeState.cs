using System;
using View.EnemyView;
using UnityEngine;

public class CowardFleeState : BaseState
{
    public CowardFleeState(EnemyBaseView view) : base(view)
    {
    }

    public override Type StateUpdate()
    {
        // change to idle
        if(!view.InAlertRange())
        {
            return typeof(CowardIdleState);
        }

        // flee state
        Vector3 fleeDir = view.Trans.position - view.Target.position;
        Vector3 newPos = fleeDir.normalized + view.Trans.position;
        view.Agent.SetDestination(newPos);
        view.Anim.SetFloat("ChaseSpeed", view.Agent.velocity.magnitude);
        return typeof(CowardFleeState);
    }
}
