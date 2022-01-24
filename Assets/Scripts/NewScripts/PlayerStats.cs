using UnityEngine;
using UnityEngine.UI;
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

    //basic stats
    private float currentHealth;
    private float currentShotRange;
    private float currentShotRate;
    private float currentDamage;
    private float currentMoveSpeed;
    private int currentBulletNumber = 1;
    private Vector2 currentShotOffset;
    private bool isDead = false;

    //TODO: private Weapon currentWeapon;

    //multipliers
    private float shotRateMulti = 1; //The shoot rate multipliers, which is faster and faster from 1->0.
    private float damageRateMulti = 1;
    private float moveSpeedMulti = 1;
    private float shotOffsetMulti = 1; //The offset when shooting, which is more and more accurate from 1->0.

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
    #endregion

    public void Initialize()
    {
        currentHealth = maxHealth;
        currentMoveSpeed = moveSpeedMulti * maxMoveSpeed;
        currentShotRate = shotRateMulti * 0.2f;
        currentDamage = damageRateMulti * 10f;
        currentShotRange = 20f;

        currentShotOffset = shotOffsetMulti * new Vector2(10f, 10f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = MathTool.NonNegativeSub(currentHealth, amount);
    }

    public void Recover(float amount)
    {
        currentHealth = MathTool.NonOverflowAdd(currentHealth, amount, maxHealth);
    }


    //Here are some functions that interact with the props class that can affect the multipliers value.
    public void ChangeShotRate(float amount)
    {
        shotRateMulti = MathTool.NonNegativeSub(shotRateMulti, -amount);

        //currentShotRate = shotRateMulti * currentWeapon.shotRate;
    }

    public void ChangeDamage(float amount)
    {
        damageRateMulti = MathTool.NonNegativeSub(damageRateMulti, -amount);

        //currentDamage = damageRateMulti * currentWeapon.damage;
    }

    public void ChangeMoveSpeed(float amount)
    {
        moveSpeedMulti = MathTool.NonNegativeSub(moveSpeedMulti, -amount);

        //currentMoveSpeed = moveSpeedMulti * maxMoveSpeed;
    }

    public void ChangeBulletNumber(int amount)
    {
        currentBulletNumber = MathTool.NonNegativeSub(currentBulletNumber, -amount);
    }

    public void ChangeOffset(float amount)
    {
        shotOffsetMulti += MathTool.NonNegativeSub(shotOffsetMulti, -amount);

        //currentShotOffset = shotOffsetMulti * currentWeapon.offset;
    }
}
