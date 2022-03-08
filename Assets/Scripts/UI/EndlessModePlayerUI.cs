using UnityEngine;
using UnityEngine.UI;

// This script is wrote by Jiacheng Sun
public class EndlessModePlayerUI : InGameUIBase
{
    [SerializeField] private Text scoreMessage;

    // Update is called once per frame
    public void ChangeScore(int amount)
    {
        scoreMessage.text = "Current Score: " + amount;
    }
}
