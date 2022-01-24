using UnityEngine;
using Utils.MathTool;

public class EnemyStats : MonoBehaviour
{
    [Header("Set In Inspector")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private int score = 5;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float[] chaseSpeeds = new float[] { 3f };

    private float currentHealth;
    private bool isDead;

    #region Attribute Fields
    public bool IsDead
    {
        get { return isDead; }
    }

    public float AttackDamage
    {
        get { return attackDamage; }
    }

    public float AttackRange
    {
        get { return attackRange; }
    }

    public float AttackRate
    {
        get { return attackRate; }
    }
    public int Score
    {
        get { return score; }
    }
    #endregion;

    //Initialization (called by manager)
    public void Initialize()
    {
        //check if all data is valid
        if(maxHealth <= 0f) { maxHealth = EnemyConst.MAX_HEALTH; }
        if(AttackDamage <= 0f) { attackDamage = EnemyConst.ATTACK_DAMAGE; }
        if(score <= 0) { score = EnemyConst.SCORE; }
        if(attackRate <= 0f) { attackRate = EnemyConst.ATTACK_RATE; }
        if(attackRange <= 0f) { attackRange = EnemyConst.ATTACK_RANGE; }
        if(chaseSpeeds.Length == 0)
        {
            chaseSpeeds = new float[] { EnemyConst.CHASE_SPEED };
        }

        //set private variable(s)
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduce enemy's current health, if go below zero, set isDead to true
    /// </summary>
    /// <param name="amount">health reduction</param>
    public void ReduceHealth(float amount)
    {
        if (isDead) { return; }

        currentHealth = MathTool.NonNegativeSub(currentHealth, amount);

        if(currentHealth <= 0f) { isDead = true; }
    }

    public float GetRandomChaseSpeed()
    {
        int randomIdx = Random.Range(0, chaseSpeeds.Length);
        return chaseSpeeds[randomIdx];
    }

}
