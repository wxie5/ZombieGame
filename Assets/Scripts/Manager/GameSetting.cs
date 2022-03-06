using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Wrote by Jiacheng Sun
public class GameSetting : Singleton<GameSetting>
{
    public void Save()
    {
        SaveObject saveObject = new SaveObject
        {
            PlayerA_Move_Up_Key = InputManager.Instance.get_PlayerA_Move_Up_Key(),
            PlayerA_Move_Down_Key = InputManager.Instance.get_PlayerA_Move_Down_Key(),
            PlayerA_Move_Left_Key = InputManager.Instance.get_PlayerA_Move_Left_Key(),
            PlayerA_Move_Right_Key = InputManager.Instance.get_PlayerA_Move_Right_Key(),
            PlayerA_Attack_Key = InputManager.Instance.get_PlayerA_Attack_Key(),
            PlayerA_Switch_Key = InputManager.Instance.get_PlayerA_Switch_Key(),
            PlayerA_Reload_Key = InputManager.Instance.get_PlayerA_Reload_Key(),
            PlayerA_Pick_Key = InputManager.Instance.get_PlayerA_Pick_Key(),

            PlayerB_Move_Up_Key = InputManager.Instance.get_PlayerB_Move_Up_Key(),
            PlayerB_Move_Down_Key = InputManager.Instance.get_PlayerB_Move_Down_Key(),
            PlayerB_Move_Left_Key = InputManager.Instance.get_PlayerB_Move_Left_Key(),
            PlayerB_Move_Right_Key = InputManager.Instance.get_PlayerB_Move_Right_Key(),
            PlayerB_Attack_Key = InputManager.Instance.get_PlayerB_Attack_Key(),
            PlayerB_Switch_Key = InputManager.Instance.get_PlayerB_Switch_Key(),
            PlayerB_Reload_Key = InputManager.Instance.get_PlayerB_Reload_Key(),
            PlayerB_Pick_Key = InputManager.Instance.get_PlayerB_Pick_Key(),
        };
        Debug.Log(InputManager.Instance.get_PlayerA_Reload_Key());
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            InputManager.Instance.Set_PlayerA_Move_Up_Key(saveObject.PlayerA_Move_Up_Key);
            InputManager.Instance.Set_PlayerA_Move_Down_Key(saveObject.PlayerA_Move_Down_Key);
            InputManager.Instance.Set_PlayerA_Move_Left_Key(saveObject.PlayerA_Move_Left_Key);
            InputManager.Instance.Set_PlayerA_Move_Right_Key(saveObject.PlayerA_Move_Right_Key);
            InputManager.Instance.Set_PlayerA_Attack_Key(saveObject.PlayerA_Attack_Key);
            InputManager.Instance.Set_PlayerA_Switch_Key(saveObject.PlayerA_Switch_Key);
            InputManager.Instance.Set_PlayerA_Reload_Key(saveObject.PlayerA_Reload_Key);
            InputManager.Instance.Set_PlayerA_Pick_Key(saveObject.PlayerA_Pick_Key);

            InputManager.Instance.Set_PlayerB_Move_Up_Key(saveObject.PlayerB_Move_Up_Key);
            InputManager.Instance.Set_PlayerB_Move_Down_Key(saveObject.PlayerB_Move_Down_Key);
            InputManager.Instance.Set_PlayerB_Move_Left_Key(saveObject.PlayerB_Move_Left_Key);
            InputManager.Instance.Set_PlayerB_Move_Right_Key(saveObject.PlayerB_Move_Right_Key);
            InputManager.Instance.Set_PlayerB_Attack_Key(saveObject.PlayerB_Attack_Key);
            InputManager.Instance.Set_PlayerB_Switch_Key(saveObject.PlayerB_Switch_Key);
            InputManager.Instance.Set_PlayerB_Reload_Key(saveObject.PlayerB_Reload_Key);
            InputManager.Instance.Set_PlayerB_Pick_Key(saveObject.PlayerB_Pick_Key);
            Debug.Log(saveObject.PlayerA_Reload_Key);
            Debug.Log(saveObject.PlayerA_Move_Up_Key);
            Debug.Log(InputManager.Instance.get_PlayerA_Reload_Key());
        }
    }
    private class SaveObject
    {
        public KeyCode PlayerA_Move_Up_Key;
        public KeyCode PlayerA_Move_Down_Key;
        public KeyCode PlayerA_Move_Left_Key;
        public KeyCode PlayerA_Move_Right_Key;
        public KeyCode PlayerA_Attack_Key;
        public KeyCode PlayerA_Switch_Key;
        public KeyCode PlayerA_Reload_Key;
        public KeyCode PlayerA_Pick_Key;

        public KeyCode PlayerB_Move_Up_Key;
        public KeyCode PlayerB_Move_Down_Key;
        public KeyCode PlayerB_Move_Left_Key;
        public KeyCode PlayerB_Move_Right_Key;
        public KeyCode PlayerB_Attack_Key;
        public KeyCode PlayerB_Switch_Key;
        public KeyCode PlayerB_Reload_Key;
        public KeyCode PlayerB_Pick_Key;
    }
}
