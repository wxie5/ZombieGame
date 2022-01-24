using UnityEngine;
using System;

[RequireComponent(typeof(EnemyStats), typeof(EnemyBehaviour))]
public class EnemyManager : MonoBehaviour
{
    private EnemyStats stats;
    private EnemyBehaviour behaviour;

    public Action<int> onAttackHit;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        behaviour = GetComponent<EnemyBehaviour>();
    }

    /// <summary>
    /// Animation Event: Triggered at the frame when zombie's arm swing down
    /// </summary>
    public void OnAnimationAttackPoint()
    {
        /*
         * if (target in attack range)
         * {
         *      onAttackHit.Invoke();
         * }
        */
    }
}
