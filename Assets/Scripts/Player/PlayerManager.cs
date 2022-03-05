using UnityEngine;
using UnityEngine.AI;
using System;

//This script is created and wrote by Wei Xie
[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerManager : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerBehaviour behaviour;
    private bool enable;
    private AIMultiplayerEndlessModeManager gameManager;

    public PlayerID playerID
    {
        get { return stats.ID; }
    }

    public void Enable(bool e)
    {
        enable = e;
    }

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        behaviour = GetComponent<PlayerBehaviour>();

        stats.Initialize();
        behaviour.Initialize();
        if (AIMultiplayerEndlessModeManager.Instance != null)
        {
            gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AIMultiplayerEndlessModeManager>();
        }
    }

    private void Update()
    {
        if (stats.IsDead) { return; }
        if (!enable) { return; }

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
        else if (stats.ID == PlayerID.PlayerB)
        {
            hori = InputManager.PlayerB_Horizontal;
            verti = InputManager.PlayerB_Vertical;
            attack = InputManager.PlayerB_Attack;
            switchGun = InputManager.PlayerB_Switch;
            reload = InputManager.PlayerB_Reload;
            pick = InputManager.PlayerB_Pick;
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
        else //AI
        {
            behaviour.PlayerPickUpSystem(gameManager.AI_WeaponNearBy());
            behaviour.PlayerWeaponSwitchSystem(gameManager.AI_SwitchWeapon());
            behaviour.PlayerMoveSystem(gameManager.AIMoveInput());
            behaviour.PlayerReloadSystem(gameManager.AI_AmmoNotFull());
            behaviour.PlayerShotSystem(gameManager.AI_EnemyInAttackRange());
        }
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
    AI,
    None
}
