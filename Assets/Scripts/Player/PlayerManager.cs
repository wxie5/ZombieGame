using UnityEngine;
using System;

//This script is created and wrote by Wei Xie
[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerManager : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerBehaviour behaviour;

    public PlayerID playerID
    {
        get { return stats.ID; }
    }

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        behaviour = GetComponent<PlayerBehaviour>();

        stats.Initialize();
        behaviour.Initialize();
    }

    private void Update()
    {
        if (stats.IsDead) { return; }

        //Get Input based on player
        float hori, verti;
        bool attack, switchGun, reload, pick;
        if (stats.ID == PlayerID.PlayerA)
        {
            hori = InputManager.PlayerA_Horizontal;
            verti = InputManager.PlayerA_Vertical;
            attack = InputManager.PlayerA_Attack;
            switchGun = InputManager.PlayerA_Switch;
            reload = InputManager.PlayerA_Reload;
            pick = InputManager.PlayerA_Pick;
        }
        else
        {
            hori = InputManager.PlayerB_Horizontal;
            verti = InputManager.PlayerB_Vertical;
            attack = InputManager.PlayerB_Attack;
            switchGun = InputManager.PlayerB_Switch;
            reload = InputManager.PlayerB_Reload;
            pick = InputManager.PlayerB_Pick;
        }
        Vector3 inputAxis = new Vector2(hori, verti).normalized;

        // Movement
        behaviour.PlayerMoveSystem(inputAxis);

        //Combat
        behaviour.PlayerShotSystem(attack);

        //Weapon Switch
        behaviour.PlayerWeaponSwitchSystem(switchGun);

        //Reload
        behaviour.PlayerReloadSystem(reload);

        //Pick Weapon
        behaviour.PlayerPickUpSystem(pick);
    }

    public void GetHit(float damage)
    {
        behaviour.PlayerGetHit(damage);
    }
    
    public void OnHealthChangeAdd(Action<float, float> listener)
    {
        stats.onHealthChange += listener;
    }

    public void OnHealthChangeRemove(Action<float, float> listener)
    {
        stats.onHealthChange -= listener;
    }

    public void OnUpdateAmmoInfoAdd(Action<int, int> listener, bool addThenInvoke)
    {
        stats.onUpdateAmmoInfo += listener;

        if(addThenInvoke)
        {
            stats.onUpdateAmmoInfo.Invoke(stats.CurrentCartridgeCap, stats.CurrentRestAmmo);
        }
    }

    public void OnUpdateAmmoInfoRemove(Action<int, int> listener)
    {
        stats.onUpdateAmmoInfo -= listener;
    }
}

public enum PlayerID
{
    PlayerA,
    PlayerB,
    None
}
