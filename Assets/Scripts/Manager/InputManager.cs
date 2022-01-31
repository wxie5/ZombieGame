using UnityEngine;
//This script is create and wrote by Wei Xie
public class InputManager : MonoBehaviour
{
    public static float PlayerA_Horizontal
    {
        get { return Input.GetAxisRaw("Horizontal1"); }
    }

    public static float PlayerA_Vertical
    {
        get { return Input.GetAxisRaw("Vertical1"); }
    }

    public static float PlayerB_Horizontal
    {
        get { return Input.GetAxisRaw("Horizontal2"); }
    }

    public static float PlayerB_Vertical
    {
        get { return Input.GetAxisRaw("Vertical2"); }
    }

    public static bool PlayerA_Attack
    {
        get { return Input.GetKey(KeyCode.J); }
    }
    public static bool PlayerB_Attack
    {
        get { return Input.GetKey(KeyCode.Keypad1); }
    }

    public static bool PlayerA_Switch
    {
        get { return Input.GetKeyDown(KeyCode.K); }
    }
    public static bool PlayerB_Switch
    {
        get { return Input.GetKeyDown(KeyCode.Keypad2); }
    }

    public static bool PlayerA_Reload
    {
        get { return Input.GetKeyDown(KeyCode.L); }
    }

    public static bool PlayerB_Reload
    {
        get { return Input.GetKeyDown(KeyCode.Keypad3); }
    }

    public static bool PlayerA_Pick
    {
        get { return Input.GetKeyDown(KeyCode.U); }
    }

    public static bool PlayerB_Pick
    {
        get { return Input.GetKeyDown(KeyCode.Keypad4); }
    }
}
