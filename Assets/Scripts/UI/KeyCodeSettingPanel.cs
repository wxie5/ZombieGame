using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeSettingPanel : Singleton<KeyCodeSettingPanel>
{
    private ControlKey currentKey;
    
    public void ShowKey(ControlKey key)
    {
        if(currentKey != null)
        {
            currentKey.IsShowing = false;
        }
        currentKey = key;
        key.IsShowing = true;
    }
    public void UnChooseCurrentKey()
    {
        currentKey.IsShowing = false;
        currentKey = null;
    }
    private void OnGUI()
    {
        if(currentKey !=null && Input.anyKeyDown)
        {
            Event e = Event.current;
            if(e.isMouse)
            {
                UnChooseCurrentKey();
            }
            if(e != null && e.isKey && e.keyCode != KeyCode.None && e.keyCode!=KeyCode.Escape)
            {
                KeyCode DownKey = e.keyCode;
                currentKey.UpdateKeyCodeText(DownKey.ToString());
                switch(currentKey.KeyName)
                {
                    case "A_Up":
                        InputManager.Instance.Set_PlayerA_Move_Up_Key(DownKey);
                        break;
                    case "A_Down":
                        InputManager.Instance.Set_PlayerA_Move_Down_Key(DownKey);
                        break;
                    case "A_Left":
                        InputManager.Instance.Set_PlayerA_Move_Left_Key(DownKey);
                        break;
                    case "A_Right":
                        InputManager.Instance.Set_PlayerA_Move_Right_Key(DownKey);
                        break;
                    case "A_Shot":
                        InputManager.Instance.Set_PlayerA_Attack_Key(DownKey);
                        break;
                    case "A_Switch":
                        InputManager.Instance.Set_PlayerA_Switch_Key(DownKey);
                        break;
                    case "A_Reload":
                        InputManager.Instance.Set_PlayerA_Reload_Key(DownKey);
                        break;
                    case "A_Pick":
                        InputManager.Instance.Set_PlayerA_Pick_Key(DownKey);
                        break;
                    case "B_Up":
                        InputManager.Instance.Set_PlayerB_Move_Up_Key(DownKey);
                        break;
                    case "B_Down":
                        InputManager.Instance.Set_PlayerB_Move_Down_Key(DownKey);
                        break;
                    case "B_Left":
                        InputManager.Instance.Set_PlayerB_Move_Left_Key(DownKey);
                        break;
                    case "B_Right":
                        InputManager.Instance.Set_PlayerB_Move_Right_Key(DownKey);
                        break;
                    case "B_Shot":
                        InputManager.Instance.Set_PlayerB_Attack_Key(DownKey);
                        break;
                    case "B_Switch":
                        InputManager.Instance.Set_PlayerB_Switch_Key(DownKey);
                        break;
                    case "B_Reload":
                        InputManager.Instance.Set_PlayerB_Reload_Key(DownKey);
                        break;
                    case "B_Pick":
                        InputManager.Instance.Set_PlayerB_Pick_Key(DownKey);
                        break;
                }
                UnChooseCurrentKey();
            }
        }
    }
}
