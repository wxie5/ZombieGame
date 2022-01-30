public class PlayerHPBar : HPBar
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();

        playerManager.OnAfterTakeDamageAdd(ModifyHP);
    }

    public void ModifyHP(float currentHP, float maxHP)
    {
        SetFill(currentHP, maxHP);
    }

    private void OnDestroy()
    {
        playerManager.OnAfterTakeDamageRemove(SetFill);
    }
}
