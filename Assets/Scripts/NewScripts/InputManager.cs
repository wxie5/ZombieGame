using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static float PlayerA_Horizontal()
    {
        return Input.GetAxisRaw("Horizontal1");
    }

    public static float PlayerA_Vertical()
    {
        return Input.GetAxisRaw("Vertical1");
    }

    public static float PlayerB_Horizontal()
    {
        return Input.GetAxisRaw("Horizontal2");
    }

    public static float PlayerB_Vertical()
    {
        return Input.GetAxisRaw("Vertical2");
    }

    public static bool PlayerA_Attack()
    {
        return Input.GetKey(KeyCode.J);
    }

    public static bool PlayerA_Switch()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
}
