using UnityEngine;
//This script is create and wrote by Wei Xie
public class InputManager : Singleton<InputManager>
{
    [SerializeField] private KeyCode PlayerA_Move_Up_Key = KeyCode.W;
    [SerializeField] private KeyCode PlayerA_Move_Down_Key = KeyCode.S;
    [SerializeField] private KeyCode PlayerA_Move_Left_Key = KeyCode.A;
    [SerializeField] private KeyCode PlayerA_Move_Right_Key = KeyCode.D;
    [SerializeField] private KeyCode PlayerA_Attack_Key = KeyCode.J;
    [SerializeField] private KeyCode PlayerA_Switch_Key = KeyCode.K;
    [SerializeField] private KeyCode PlayerA_Reload_Key = KeyCode.L;
    [SerializeField] private KeyCode PlayerA_Pick_Key = KeyCode.U;

    [SerializeField] private KeyCode PlayerB_Move_Up_Key = KeyCode.UpArrow;
    [SerializeField] private KeyCode PlayerB_Move_Down_Key = KeyCode.DownArrow;
    [SerializeField] private KeyCode PlayerB_Move_Left_Key = KeyCode.LeftArrow;
    [SerializeField] private KeyCode PlayerB_Move_Right_Key = KeyCode.RightArrow;
    [SerializeField] private KeyCode PlayerB_Attack_Key = KeyCode.Keypad1;
    [SerializeField] private KeyCode PlayerB_Switch_Key = KeyCode.Keypad2;
    [SerializeField] private KeyCode PlayerB_Reload_Key = KeyCode.Keypad3;
    [SerializeField] private KeyCode PlayerB_Pick_Key = KeyCode.Keypad4;
    public float PlayerA_Horizontal()
    {
        float hori = 0;
        if(Input.GetKey(PlayerA_Move_Right_Key))
        {
            hori+=1;
        }
        if (Input.GetKey(PlayerA_Move_Left_Key))
        {
            hori-=1;
        }
        return hori;
    }
    public float PlayerA_Vertical()
    {
        float vert = 0;
        if (Input.GetKey(PlayerA_Move_Up_Key))
        {
            vert += 1;
        }
        if (Input.GetKey(PlayerA_Move_Down_Key))
        {
            vert -= 1;
        }
        return vert;
    }
    public float PlayerB_Horizontal()
    {
        float hori = 0;
        if(Input.GetKey(PlayerB_Move_Right_Key))
        {
            hori+=1;
        }
        if (Input.GetKey(PlayerB_Move_Left_Key))
        {
            hori-=1;
        }
        return hori;
    }
    public float PlayerB_Vertical()
    {
        float vert = 0;
        if (Input.GetKey(PlayerB_Move_Up_Key))
        {
            vert += 1;
        }
        if (Input.GetKey(PlayerB_Move_Down_Key))
        {
            vert -= 1;
        }
        return vert;
    }
    public bool PlayerA_Attack
    {
        get { return Input.GetKey(PlayerA_Attack_Key); }
    }
    public bool PlayerB_Attack
    {
        get { return Input.GetKey(PlayerB_Attack_Key); }
    }
    public bool PlayerA_Switch
    {
        get { return Input.GetKeyDown(PlayerA_Switch_Key); }
    }
    public bool PlayerB_Switch
    {
        get { return Input.GetKeyDown(PlayerB_Switch_Key); }
    }
    public bool PlayerA_Reload
    {
        get { return Input.GetKeyDown(PlayerA_Reload_Key); }
    }
    public bool PlayerB_Reload
    {
        get { return Input.GetKeyDown(PlayerB_Reload_Key); }
    }
    public bool PlayerA_Pick
    {
        get { return Input.GetKeyDown(PlayerA_Pick_Key); }
    }
    public bool PlayerB_Pick
    {
        get { return Input.GetKeyDown(PlayerB_Pick_Key); }
    }

    public void Set_PlayerA_Move_Up_Key(KeyCode key)
    {
        PlayerA_Move_Up_Key = key;
    }
    public void Set_PlayerA_Move_Down_Key(KeyCode key)
    {
        PlayerA_Move_Down_Key = key;
    }
    public void Set_PlayerA_Move_Left_Key(KeyCode key)
    {
        PlayerA_Move_Left_Key = key;
    }
    public void Set_PlayerA_Move_Right_Key(KeyCode key)
    {
        PlayerA_Move_Right_Key = key;
    }
    public void Set_PlayerA_Attack_Key(KeyCode key)
    {
        PlayerA_Attack_Key = key;
    }
    public void Set_PlayerA_Switch_Key(KeyCode key)
    {
        PlayerA_Switch_Key = key;
    }
    public void Set_PlayerA_Reload_Key(KeyCode key)
    {
        PlayerA_Reload_Key = key;
    }
    public void Set_PlayerA_Pick_Key(KeyCode key)
    {
        PlayerA_Pick_Key = key;
    }
    public void Set_PlayerB_Move_Up_Key(KeyCode key)
    {
        PlayerB_Move_Up_Key = key;
    }
    public void Set_PlayerB_Move_Down_Key(KeyCode key)
    {
        PlayerB_Move_Down_Key = key;
    }
    public void Set_PlayerB_Move_Left_Key(KeyCode key)
    {
        PlayerB_Move_Left_Key = key;
    }
    public void Set_PlayerB_Move_Right_Key(KeyCode key)
    {
        PlayerB_Move_Right_Key = key;
    }
    public void Set_PlayerB_Attack_Key(KeyCode key)
    {
        PlayerB_Attack_Key = key;
    }
    public void Set_PlayerB_Switch_Key(KeyCode key)
    {
        PlayerB_Switch_Key = key;
    }
    public void Set_PlayerB_Reload_Key(KeyCode key)
    {
        PlayerB_Reload_Key = key;
    }
    public void Set_PlayerB_Pick_Key(KeyCode key)
    {
        PlayerB_Pick_Key = key;
    }
    public KeyCode get_PlayerA_Move_Up_Key()
    {
        return PlayerA_Move_Up_Key;
    }
    public KeyCode get_PlayerA_Move_Down_Key()
    {
        return PlayerA_Move_Down_Key;
    }
    public KeyCode get_PlayerA_Move_Left_Key()
    {
        return PlayerA_Move_Left_Key;
    }
    public KeyCode get_PlayerA_Move_Right_Key()
    {
        return PlayerA_Move_Right_Key;
    }
    public KeyCode get_PlayerA_Attack_Key()
    {
        return PlayerA_Attack_Key;
    }
    public KeyCode get_PlayerA_Switch_Key()
    {
        return PlayerA_Switch_Key;
    }
    public KeyCode get_PlayerA_Reload_Key()
    {
        return PlayerA_Reload_Key;
    }
    public KeyCode get_PlayerA_Pick_Key()
    {
        return PlayerA_Pick_Key;
    }
    public KeyCode get_PlayerB_Move_Up_Key()
    {
        return PlayerB_Move_Up_Key;
    }
    public KeyCode get_PlayerB_Move_Down_Key()
    {
        return PlayerB_Move_Down_Key;
    }
    public KeyCode get_PlayerB_Move_Left_Key()
    {
        return PlayerB_Move_Left_Key;
    }
    public KeyCode get_PlayerB_Move_Right_Key()
    {
        return PlayerB_Move_Right_Key;
    }
    public KeyCode get_PlayerB_Attack_Key()
    {
        return PlayerB_Attack_Key;
    }
    public KeyCode get_PlayerB_Switch_Key()
    {
        return PlayerB_Switch_Key;
    }
    public KeyCode get_PlayerB_Reload_Key()
    {
        return PlayerB_Reload_Key;
    }
    public KeyCode get_PlayerB_Pick_Key()
    {
        return PlayerB_Pick_Key;
    }
}
