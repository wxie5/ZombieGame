using UnityEngine;
using System;

[RequireComponent(typeof(EnemyStats), typeof(EnemyBehaviour))]
public class EnemyManager : MonoBehaviour
{
    private EnemyStats stats;
    private EnemyBehaviour behaviour;

    public Action<PlayerID, float> onAttackHit;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        behaviour = GetComponent<EnemyBehaviour>();

        stats.Initialize();
        behaviour.Initialize();
    }

    private void Update()
    {
        if (stats.IsDead) { return; }
        
        behaviour.IdleOrChase();
        behaviour.Attack();
        
    }

    /// <summary>
    /// Animation Event: Triggered at the frame when zombie's arm swing down
    /// </summary>
    public void OnAnimationAttackPoint()
    {
        /*
         * if (target in attack range)
         * {
         *      onAttackHit.Invoke(behaviour.TargetID, stats.AttackDamage);
         * }
        */
    }

    public void GetHit(float weaponDamage)
    {
        stats.ReduceHealth(weaponDamage);

        if(stats.IsDead)
        {
            behaviour.Die();
        }
        else
        {
            behaviour.GetHit();
        }
    }

    public bool IsAttackable()
    {
        return !stats.IsDead;
    }
}
