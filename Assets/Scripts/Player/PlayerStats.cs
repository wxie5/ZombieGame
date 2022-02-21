using UnityEngine;
using Utils.MathTool;
using System;

//This script is created and wrote by Jiacheng Sun
//Modified bt Wei Xie
public class PlayerStats : MonoBehaviour
{
    //Some buff multipliers that the player comes with, the initial value is 1
    //which will be increased by the effect of props. The value in the game is the multiplier * actual value.
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private int gunSlot = 2;
    [Range(0.2f, 0.5f)]
    [SerializeField] private float targetingSpeedRatio = 0.4f;
    [SerializeField] private PlayerID id;

    [Header("Set Gun's ScriptableObject")]
    [SerializeField] private GunItem defaultGun;

    [Header("Set Sound Effect")]
    [SerializeField] private AudioClip getHurt;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip recover;
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
    private Gun[] gunInfos;
    private int currentGunIndex = 0;
    private int[] ammoCaps;
    private int[] cartridgeCaps;


    //multipliers
    private float shotRateMulti = 1f; //The shoot rate multipliers, which is faster and faster from 1->0.
    private float damageRateMulti = 1f;
    private float moveSpeedMulti = 1f;
    private float shotOffsetMulti = 1f; //The offset when shooting, which is more and more accurate from 1->0.

    //Stats Events
    public Action<float, float> onHealthChange;
    public Action<int, int> onUpdateAmmoInfo; //current Ammo in use, left ammo in pack

    #region Attribute Fields
    public PlayerID ID
    {
        get { return id; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
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
        get { return gunInfos[currentGunIndex]; }
    }

    public int CurrentCartridgeCap
    {
        get { return cartridgeCaps[currentGunIndex]; }
    }

    public int CurrentRestAmmo
    {
        get { return ammoCaps[currentGunIndex]; }
    }
    #endregion

    public void Initialize()
    {
        //initialize run time stats
        currentHealth = maxHealth;
        currentMoveSpeed = moveSpeedMulti * maxMoveSpeed;

        //initiailize gun array
        cartridgeCaps = new int[gunSlot];
        ammoCaps = new int[gunSlot];
        gunInfos = new Gun[gunSlot];

        //initialize locked gun
        gunInfos[0] = defaultGun.GunItemInfo;
        UpdateAmmoInfo();

        //update player combat data based on current gun
        UpdatePlayerCombatStats();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = MathTool.NonNegativeSub(currentHealth, amount);

        if (onHealthChange != null) { onHealthChange.Invoke(currentHealth, maxHealth); }
        AudioSource.PlayClipAtPoint(getHurt, gameObject.transform.position);
        if (currentHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(dead, gameObject.transform.position);
            isDead = true;
            gameObject.tag = GameConst.DEAD_TAG;
        }
    }

    public void Recover(float amount)
    {
        currentHealth = MathTool.NonOverflowAdd(currentHealth, amount, maxHealth);
        AudioSource.PlayClipAtPoint(recover, gameObject.transform.position);
        if (onHealthChange != null) { onHealthChange.Invoke(currentHealth, maxHealth); }
    }


    //Here are some functions that interact with the props class that can affect the multipliers value.
    public void ChangeShotRate(float amount)
    {
        shotRateMulti = MathTool.NonNegativeSub(shotRateMulti, amount);

        currentShotRate = shotRateMulti * gunInfos[currentGunIndex].shotRate;
    }

    public void ChangeDamage(float amount)
    {
        damageRateMulti = MathTool.NonNegativeSub(damageRateMulti, -amount);

        currentDamage = damageRateMulti * gunInfos[currentGunIndex].damage;
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

        currentShotOffset = shotOffsetMulti * gunInfos[currentGunIndex].offset;
    }

    public void SwitchGunUpdateState()
    {
        int newGunIndex = MathTool.NextIndexNoOverflow(currentGunIndex, gunInfos.Length);
        if(gunInfos[newGunIndex] == null)
        {
            return;
        }
        else
        {
            currentGunIndex = newGunIndex;
        }

        //Update combat info
        UpdatePlayerCombatStats();
    }
    public bool IsSingleWeapon()
    {
        int counter = 0;
        foreach (Gun g in gunInfos)
        {
            if (g != null)
            {
                counter++;
            }
        }

        if (counter == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void UpdateReloadData()
    {
        int curCartCap = gunInfos[currentGunIndex].cartridgeCapacity;
        int fillNum = curCartCap - cartridgeCaps[currentGunIndex];
        if((ammoCaps[currentGunIndex] - fillNum) >= 0)
        {
            ammoCaps[currentGunIndex] -= fillNum;
            cartridgeCaps[currentGunIndex] = curCartCap;
        }
        else
        {
            cartridgeCaps[currentGunIndex] = ammoCaps[currentGunIndex];
            ammoCaps[currentGunIndex] = 0;
        }

        if (onUpdateAmmoInfo != null) { onUpdateAmmoInfo.Invoke(cartridgeCaps[currentGunIndex], ammoCaps[currentGunIndex]); }
    }

    //pick gun will switch it to current gun
    public void PickGunUpdateState(Gun info)
    {
        int newIdx = -1;
        for(int i = 0; i < gunInfos.Length; i++)
        {
            if(gunInfos[i] == null)
            {
                newIdx = i;
                break;
            }
        }

        if(newIdx != -1)
        {
            //if find empty slop, switch it to current gun
            gunInfos[newIdx] = info;
            currentGunIndex = newIdx;
        }
        else
        {
            //if no slot is empty, drop the current gun, switch to new one
            gunInfos[currentGunIndex] = info;
        }

        UpdateAmmoInfo();
        UpdatePlayerCombatStats();
    }

    private void UpdateAmmoInfo()
    {
        Gun currentGun = gunInfos[currentGunIndex];
        ammoCaps[currentGunIndex] = currentGun.ammoCapacity;
        cartridgeCaps[currentGunIndex] = currentGun.cartridgeCapacity;

        if (onUpdateAmmoInfo != null) { onUpdateAmmoInfo.Invoke(cartridgeCaps[currentGunIndex], ammoCaps[currentGunIndex]); }
    }

    private void UpdatePlayerCombatStats()
    {
        Gun currentGun = gunInfos[currentGunIndex];
        currentShotRate = currentGun.shotRate * shotRateMulti;
        currentShotRange = currentGun.shotRange;
        currentDamage = currentGun.damage * damageRateMulti;
        currentShotOffset = currentGun.offset * shotOffsetMulti;
    }

    /// <summary>
    /// reduce the current gun's ammo number by amount
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>whether we run out of ammo</returns>
    public bool ReduceAmmo(int amount = 1)
    {
        cartridgeCaps[currentGunIndex] -= amount;

        if (onUpdateAmmoInfo != null) { onUpdateAmmoInfo.Invoke(cartridgeCaps[currentGunIndex], ammoCaps[currentGunIndex]); }

        if (cartridgeCaps[currentGunIndex] <= 0)
        {
            return false;
        }

        return true;
    }

    public string AmmoInfo()
    {
        if(cartridgeCaps.Length == 0 || ammoCaps.Length == 0)
        {
            return null;
        }
        return cartridgeCaps[currentGunIndex] + "/" + ammoCaps[currentGunIndex];
        //return "info";
    }
}
