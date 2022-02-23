using UnityEngine;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class MultiPlayerUI : MonoBehaviour
{
    [SerializeField] private Text scoreMessage;
    [SerializeField] private Text game_message;
    [SerializeField] private Text[] bullet_message;
    [SerializeField] private Text[] props_message_Offset;
    [SerializeField] private Text[] props_message_shotRate;
    [SerializeField] private Text[] props_message_AmmoCapacity;
    [SerializeField] private Text[] props_message_Damage;
    [SerializeField] private Text[] props_message_MoveSpeed;

    // Update is called once per frame
    public void ChangeScore(int amount)
    {
        scoreMessage.text = "Current Score: " + amount;
    }
    public void ClearGmaeMessage()
    {
        game_message.text = string.Empty;
    }

    public void ChangeGameMessage(string gameMessage)
    {
        game_message.text = gameMessage;
    }

    public void changeBulletMessage(int playerNumber, string bulletMessage)
    {
        bullet_message[playerNumber].text = bulletMessage;
    }
    public void ChangePropsMessage_Offset(int playerNumber, string message)
    {
        props_message_Offset[playerNumber].text = message;
    }
    public void ChangePropsMessage_ShotRate(int playerNumber, string message)
    {
        props_message_shotRate[playerNumber].text = message;
    }
    public void ChangePropsMessage_AmmoCapacity(int playerNumber, string message)
    {
        props_message_AmmoCapacity[playerNumber].text = message;
    }
    public void ChangePropsMessage_Damage(int playerNumber, string message)
    {
        props_message_Damage[playerNumber].text = message;
    }
    public void ChangePropsMessage_MoveSpeed(int playerNumber, string message)
    {
        props_message_MoveSpeed[playerNumber].text = message;
    }
}
