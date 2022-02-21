using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class StorySinglePlayerUI : MonoBehaviour
{
    private int props_amount_DamageIncrease = 0;
    private int props_amount_MoveSpeedIncrease = 0;
    private int props_amount_BulletNumberIncrease = 0;
    private int props_amount_OffSetDecrease = 0;
    private int props_amount_ShotRateDecrease = 0;
    private int props_max_amount;

    [SerializeField] private Text game_message;
    [SerializeField] private Text bullet_message;
    [SerializeField] private Text chat_box_message;
    [SerializeField] private GameObject chat_box;
    [SerializeField] private GameObject bullet_info;


    public void Change_props_amount(Props.PropsType type)
    {
        if(type == Props.PropsType.DamageIncrease)
        {
            props_amount_DamageIncrease++;
        }
        if(type == Props.PropsType.MoveSpeedIncrease)
        {
            props_amount_MoveSpeedIncrease++;
        }
        if(type == Props.PropsType.BulletNumberIncrease)
        {
            props_amount_BulletNumberIncrease++;
        }
        if(type == Props.PropsType.OffSetDecrease)
        {
            props_amount_OffSetDecrease++;
        }
        if (type == Props.PropsType.ShotRateDecrease)
        {
            props_amount_ShotRateDecrease++;
        }
    }

    public void Change_props_max_amount(int amount)
    {
        props_max_amount = amount;
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
    public void StartChat() //Need to call before chating!
    {
        bullet_info.SetActive(false);
        chat_box.SetActive(true);
    }
    public IEnumerator PlayerChatMessage(string message)
    {
        chat_box_message.text = message;
        yield return new WaitForSeconds(0.5f);
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
    public void updateBulletInfo(int current, int total)
    {
        bullet_message.text = "   "+current + "/" + total;
    }
}
