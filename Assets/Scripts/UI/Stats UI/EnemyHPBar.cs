public class EnemyHPBar : HPBar
{
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();

        enemyManager.OnAfterTakeDamageAdd(OnAfterTakeDamageHandler);
        enemyManager.OnDeadAdd(OnDeadHandler);
    }

    public void OnAfterTakeDamageHandler(float currentHP, float maxHP)
    {
        SetFill(currentHP, maxHP);
    }

    public void OnDeadHandler()
    {
        this.enabled = false;
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        enemyManager.OnAfterTakeDamageRemove(OnAfterTakeDamageHandler);
        enemyManager.OnDeadRemove(OnDeadHandler);
    }
}
