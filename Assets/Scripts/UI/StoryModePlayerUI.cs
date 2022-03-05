using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class StoryModePlayerUI : InGameUIBase
{
    [SerializeField] private Text chat_box_message;
    [SerializeField] private GameObject chat_box;
    [SerializeField] private GameObject bullet_info;

    public void changeBulletMessage(string bulletMessage)
    {
        bullet_message[0].text = bulletMessage;
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
}
