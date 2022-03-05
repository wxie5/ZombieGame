using UnityEngine;
using UnityEngine.UI;

// This script is wrote by Jiacheng Sun
public class InGameUIBase : MonoBehaviour
{
    [SerializeField] protected Text game_message;
    [SerializeField] protected Text[] bullet_message;
    [SerializeField] protected Text[] props_message_Offset;
    [SerializeField] protected Text[] props_message_shotRate;
    [SerializeField] protected Text[] props_message_AmmoCapacity;
    [SerializeField] protected Text[] props_message_Damage;
    [SerializeField] protected Text[] props_message_MoveSpeed;

    public void ClearGmaeMessage()
    {
        game_message.text = string.Empty;
    }

    public void ChangeGameMessage(string gameMessage)
    {
        game_message.text = gameMessage;
    }

    public void changeBulletMessage(string bulletMessage, int playerNumber = 0)
    {
        bullet_message[playerNumber].text = bulletMessage;
    }
    public void ChangePropsMessage_Offset(string message, int playerNumber = 0)
    {
        props_message_Offset[playerNumber].text = message;
    }
    public void ChangePropsMessage_ShotRate(string message, int playerNumber = 0)
    {
        props_message_shotRate[playerNumber].text = message;
    }
    public void ChangePropsMessage_AmmoCapacity(string message, int playerNumber = 0)
    {
        props_message_AmmoCapacity[playerNumber].text = message;
    }
    public void ChangePropsMessage_Damage(string message, int playerNumber = 0)
    {
        props_message_Damage[playerNumber].text = message;
    }
    public void ChangePropsMessage_MoveSpeed(string message, int playerNumber = 0)
    {
        props_message_MoveSpeed[playerNumber].text = message;
    }
}
