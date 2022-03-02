//This script is wrote by WeiXie
public class PlayerHPBar : HPBar
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerManager.OnHealthChangeAdd(ModifyHP);
    }

    public void ModifyHP(float currentHP, float maxHP)
    {
        SetFill(currentHP, maxHP);
    }

    private void OnDestroy()
    {
        playerManager.OnHealthChangeRemove(SetFill);
    }
}
