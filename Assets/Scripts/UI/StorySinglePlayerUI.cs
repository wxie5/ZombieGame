using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class StorySinglePlayerUI : MonoBehaviour
{
    [SerializeField] private Text game_message;
    [SerializeField] private Text bullet_message;
    [SerializeField] private Text chat_box_message;
    [SerializeField] private GameObject chat_box;
    [SerializeField] private GameObject bullet_info;
    [SerializeField] private Text props_message_Offset;
    [SerializeField] private Text props_message_shotRate;
    [SerializeField] private Text props_message_AmmoCapacity;
    [SerializeField] private Text props_message_Damage;
    [SerializeField] private Text props_message_MoveSpeed;

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
    public void StartChat() //Need to call before chating!
    {
        bullet_info.SetActive(false);
        chat_box.SetActive(true);
    }
    public IEnumerator PlayerChatMessage(string message)
    {
        chat_box_message.text = message;
        yield return new WaitForSeconds(0.2f);
        while (!(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            yield return null;
        }
    }
    public void AfterChat() //Need to call after chating!
    {
        chat_box.SetActive(false);
        bullet_info.SetActive(true);
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
