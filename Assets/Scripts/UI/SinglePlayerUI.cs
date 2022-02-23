using UnityEngine;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class SinglePlayerUI : MonoBehaviour
{

    [SerializeField] private Text scoreMessage;
    [SerializeField] private Text game_message;
    [SerializeField] private Text bullet_message;
    [SerializeField] private Text props_message_Offset;
    [SerializeField] private Text props_message_shotRate;
    [SerializeField] private Text props_message_AmmoCapacity;
    [SerializeField] private Text props_message_Damage;
    [SerializeField] private Text props_message_MoveSpeed;

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

    public void changeBulletMessage(string bulletMessage)
    {
        bullet_message.text = bulletMessage;
    }
    public void ChangePropsMessage_Offset(string message)
    {
        props_message_Offset.text = message;
    }
    public void ChangePropsMessage_ShotRate(string message)
    {
        props_message_shotRate.text = message;
    }
    public void ChangePropsMessage_AmmoCapacity(string message)
    {
        props_message_AmmoCapacity.text = message;
    }
    public void ChangePropsMessage_Damage(string message)
    {
        props_message_Damage.text = message;
    }
    public void ChangePropsMessage_MoveSpeed(string message)
    {
        props_message_MoveSpeed.text = message;
    }
}
