using UnityEngine;
using Utils.MathTool;

public class PlayerStats : MonoBehaviour
{
    //Some buff multipliers that the player comes with, the initial value is 1
    //which will be increased by the effect of props. The value in the game is the multiplier * actual value.
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxMoveSpeed = 5f;
    [Range(0.2f, 0.5f)]
    [SerializeField] private float targetingSpeedRatio = 0.4f;
    [SerializeField] private PlayerID id;

    [Header("Set Gun's ScriptableObject")]
    [SerializeField] private Gun[] gunSO;

    [Header("Audio sources")]
    [SerializeField] private AudioClip getHurt;
    [SerializeField] private AudioClip getRecover;
    [SerializeField] private AudioClip dead;

    //basic stats
    private float currentHealth;
    private float currentShotRange;
    private float currentShotRate;
    private float currentDamage;
    private float currentMoveSpeed;
    private int currentBulletNumber = 1;
    private Vector2 currentShotOffset;
    private bool isDead = false;

    //Weapons
    private Gun currentGun;
    private GunType permanentGunType;
    private GunType pickUpGunType;

    //multipliers
    private float shotRateMulti = 1f; //The shoot rate multipliers, which is faster and faster from 1->0.
    private float damageRateMulti = 1f;
    private float moveSpeedMulti = 1f;
    private float shotOffsetMulti = 1f; //The offset when shooting, which is more and more accurate from 1->0.

    #region Attribute Fields
    public PlayerID ID
    {
        get { return id; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float CurrentShotRate
    {
        get { return currentShotRate; }
    }

    public float CurrentDamage
    {
        get { return currentDamage; }
    }

    public float CurrentShotRange
    {
        get { return currentShotRange; }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
    }

    public float CurrentTargetingMoveSpeed
    {
        get { return currentMoveSpeed * targetingSpeedRatio; }
    }

    public int BulletNumber
    {
        get { return currentBulletNumber; }
    }

    public Vector2 CurrentShotOffset
    {
        get { return currentShotOffset; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    public Gun CurrentGun
    {
        get { return currentGun; }
    }
    #endregion

    public void Initialize()
    {
        currentHealth = maxHealth;
        currentMoveSpeed = moveSpeedMulti * maxMoveSpeed;

        //for test only
        permanentGunType = GunType.Handgun;
        pickUpGunType = GunType.AssaultRifle;

        foreach (Gun g in gunSO)
        {
            if (g.gunType == permanentGunType)
            {
                currentGun = g;
            }
        }

        UpdatePlayerCombatStats();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = MathTool.NonNegativeSub(currentHealth, amount);
        AudioSource.PlayClipAtPoint(getHurt, gameObject.transform.position);
        if(currentHealth == 0)
        {
            isDead = true;
            AudioSource.PlayClipAtPoint(dead, gameObject.transform.position);
        }
    }

    public void Recover(float amount)
    {
        currentHealth = MathTool.NonOverflowAdd(currentHealth, amount, maxHealth);
        AudioSource.PlayClipAtPoint(getRecover, gameObject.transform.position);
    }


    //Here are some functions that interact with the props class that can affect the multipliers value.
    public void ChangeShotRate(float amount)
    {
        shotRateMulti = MathTool.NonNegativeSub(shotRateMulti, amount);

        currentShotRate = shotRateMulti * currentGun.shotRate;
    }

    public void ChangeDamage(float amount)
    {
        damageRateMulti = MathTool.NonNegativeSub(damageRateMulti, -amount);

        currentDamage = damageRateMulti * currentGun.damage;
    }

    public void ChangeMoveSpeed(float amount)
    {
        moveSpeedMulti = MathTool.NonNegativeSub(moveSpeedMulti, -amount);

        currentMoveSpeed = moveSpeedMulti * maxMoveSpeed;
    }

    public void ChangeBulletNumber(int amount)
    {
        currentBulletNumber = MathTool.NonNegativeSub(currentBulletNumber, -amount);
    }

    public void ChangeOffset(float amount)
    {
        shotOffsetMulti = MathTool.NonNegativeSub(shotOffsetMulti, amount);

        currentShotOffset = shotOffsetMulti * currentGun.offset;
    }

    public void SwitchGunUpdateState()
    {
        GunType targetGunType = default(GunType);
        if(currentGun.gunType == permanentGunType)
        {
            targetGunType = pickUpGunType;
        }
        else
        {
            targetGunType = permanentGunType;
        }

        foreach(Gun g in gunSO)
        {
            if(g.gunType == targetGunType)
            {
                currentGun = g;
            }
        }

        UpdatePlayerCombatStats();
    }

    public void PickGunUpdateState(GunType newGunType)
    {
        //TODO: change pickUpGunType to the new picked up gun

        UpdatePlayerCombatStats();
    }

    private void UpdatePlayerCombatStats()
    {
        currentShotRate = currentGun.shotRate * shotRateMulti;
        currentShotRange = currentGun.shotRange;
        currentDamage = currentGun.damage * damageRateMulti;
        currentShotOffset = currentGun.offset * shotOffsetMulti;
    }
}
