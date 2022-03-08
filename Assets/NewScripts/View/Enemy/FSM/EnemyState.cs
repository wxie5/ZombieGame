using System;
using View.EnemyView;

public abstract class AbstractState
{
    public abstract Type StateUpdate();
}

public abstract class BaseState :AbstractState
{
    protected EnemyBaseView view;
    public BaseState(EnemyBaseView view)
    {
        this.view = view;
    }
}

public abstract class ZombieState : AbstractState
{
    protected EnemyZombieView view;
    public ZombieState(EnemyZombieView view)
    {
        this.view = view;
    }
}
public abstract class BoomerState : AbstractState
{
    protected EnemyBoomerView view;
    public BoomerState(EnemyBoomerView view)
    {
        this.view = view;
    }
}

public abstract class PosionState : AbstractState
{
    protected EnemyPosionView view;

    public PosionState(EnemyPosionView view)
    {
        this.view = view;
    }
}
