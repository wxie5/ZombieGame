using UnityEngine;
using System;

[RequireComponent(typeof(EnemyStats), typeof(EnemyBehaviour))]
public class EnemyManager : MonoBehaviour
{
    private EnemyStats stats;
    private EnemyBehaviour behaviour;

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

    #region Animation Events
    // Animation Event: Triggered at the frame when zombie's arm swing down
    public void OnAnimationAttackPoint()
    {
        behaviour.DealDamage();
    }
    #endregion

    public void GetHit(float weaponDamage)
    {
        behaviour.EnemyGetHit(weaponDamage);
    }

    public bool IsAttackable()
    {
        return !stats.IsDead;
    }

    public void OnAfterTakeDamageAdd(Action<float, float> listener)
    {
        behaviour.onAfterTakeDamage += listener;
    }

    public void OnAfterTakeDamageRemove(Action<float, float> listener)
    {
        behaviour.onAfterTakeDamage -= listener;
    }

    public void OnDeadAdd(Action listener)
    {
        behaviour.onDead += listener;
    }
    public void OnDeadRemove(Action listener)
    {
        behaviour.onDead -= listener;
    }
}
