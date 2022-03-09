using System;
using View.EnemyView;

public class CowardIdleState : BaseState
{
    public CowardIdleState(EnemyBaseView view) : base(view)
    {
    }

    public override Type StateUpdate()
    {
        // transition to chase state
        if (view.InAlertRange())
        {
            view.Agent.isStopped = false;
            return typeof(CowardFleeState);
        }

        // idle state
        view.Agent.isStopped = true;
        return typeof(CowardIdleState);
    }
}
