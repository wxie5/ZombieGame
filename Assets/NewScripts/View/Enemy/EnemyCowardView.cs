using System;
using System.Collections.Generic;

namespace View.EnemyView
{
    public class EnemyCowardView : EnemyBaseView
    {
        protected override void StateMachineInit()
        {
            states = new Dictionary<Type, AbstractState>();
            states.Add(typeof(CowardIdleState), new CowardIdleState(this));
            states.Add(typeof(CowardFleeState), new CowardFleeState(this));

            curState = states[typeof(CowardIdleState)];
        }
    }
}
