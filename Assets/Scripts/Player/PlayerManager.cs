using UnityEngine;
using System;

//This script is created and wrote by Wei Xie
[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerManager : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerBehaviour behaviour;
    private bool enable;
    private PlayerAI playerAI;

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
        if (this.GetComponent<PlayerAI>())
        {
            playerAI = this.GetComponent<PlayerAI>();
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
            hori = InputManager.Instance.PlayerA_Horizontal();
            verti = InputManager.Instance.PlayerA_Vertical();
            attack = InputManager.Instance.PlayerA_Attack;
            switchGun = InputManager.Instance.PlayerA_Switch;
            reload = InputManager.Instance.PlayerA_Reload;
            pick = InputManager.Instance.PlayerA_Pick;
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
            hori = InputManager.Instance.PlayerB_Horizontal();
            verti = InputManager.Instance.PlayerB_Vertical();
            attack = InputManager.Instance.PlayerB_Attack;
            switchGun = InputManager.Instance.PlayerB_Switch;
            reload = InputManager.Instance.PlayerB_Reload;
            pick = InputManager.Instance.PlayerB_Pick;
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
            behaviour.PlayerPickUpSystem(playerAI.AI_WeaponNearBy());
            behaviour.PlayerWeaponSwitchSystem(playerAI.AI_TimeToSwitchWeapon());
            behaviour.PlayerMoveSystem(playerAI.AIMoveInput());
            behaviour.PlayerReloadSystem(playerAI.AI_AmmoNotFull());
            behaviour.PlayerShotSystem(playerAI.AI_EnemyInAttackRange());
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
