using UnityEngine;
using Model.EnemyModel;

namespace Controller.EnemyController
{
    public class EnemyBaseController : IEnemyController
    {
    }

    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }
}
