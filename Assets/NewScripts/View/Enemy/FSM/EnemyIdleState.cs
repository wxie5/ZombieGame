using System;
using View.EnemyView;

public class EnemyIdleState : BaseState
{
    public EnemyIdleState(EnemyBaseView view) : base(view)
    {
    }

    public override Type StateUpdate()
    {
        // transition to chase state
        if (view.InAlertRange())
        {
            view.Agent.isStopped = false;
            return typeof(EnemyChaseState);
        }

        // idle state
        view.Agent.isStopped = true;
        return typeof(EnemyIdleState);
    }
}
