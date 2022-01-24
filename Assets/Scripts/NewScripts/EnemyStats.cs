using UnityEngine;
using Utils.MathTool;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float attackSpeed;

    private float currentHealth;

    private bool isDead;

    public void ReduceHealth(float amount)
    {
        if (isDead) { return; }

        currentHealth = MathTool.NonNegativeSub(currentHealth, amount);

        if(currentHealth <= 0f) { isDead = true; }
    }
}
