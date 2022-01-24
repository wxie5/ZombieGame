using UnityEngine;

[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerManager : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerBehaviour behaviour;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        behaviour = GetComponent<PlayerBehaviour>();

        stats.Initialize();
        behaviour.Initialize();
    }

    private void Update()
    {
        //Movement
        float hori, verti;
        if (stats.ID == PlayerID.PlayerA)
        {
            hori = InputManager.PlayerA_Horizontal();
            verti = InputManager.PlayerA_Vertical();
        }
        else
        {
            hori = InputManager.PlayerB_Horizontal();
            verti = InputManager.PlayerB_Vertical();
        }
        Vector3 inputAxis = new Vector2(hori, verti).normalized;
        behaviour.PlayerMoveSystem(inputAxis);

        //Combat
        behaviour.PlayerCombatSystem(InputManager.PlayerA_Attack());

        //Weapon Switch
        behaviour.PlayerWeaponSwitchSystem(InputManager.PlayerA_Switch());
    }
}

public enum PlayerID
{
    PlayerA,
    PlayerB,
    None
}
