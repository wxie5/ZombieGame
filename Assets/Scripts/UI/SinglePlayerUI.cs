using UnityEngine;
using UnityEngine.UI;

//Controll the UI in SinglePlayer Mode
// This script is wrote by Jiacheng Sun
public class SinglePlayerUI : MonoBehaviour
{
    private int score;
    private int props_amount_DamageIncrease = 0;
    private int props_amount_MoveSpeedIncrease = 0;
    private int props_amount_BulletNumberIncrease = 0;
    private int props_amount_OffSetDecrease = 0;
    private int props_amount_ShotRateDecrease = 0;
    private int props_max_amount;

    [SerializeField] private Text scoreMessage;
    [SerializeField] private Text game_message;
    [SerializeField] private Text bullet_message;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreMessage.text = "Current Score: " + score;
    }
    public void ChangeScore(int amount)
    {
        score = amount;
    }
    public int GetScore()
    {
        return score;
    }
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
    public void Show_final_score()
    {

    }

    public void updateBulletInfo(int current, int total)
    {
        bullet_message.text = "   "+current + "/" + total;
    }
}
