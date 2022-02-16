using UnityEngine;
using UnityEngine.UI;

//This script is created and wrote by Jiacheng Sun

//This script is used to control UI display in single player Mode.
namespace Manager
    {
        public class UIManager : Singleton<UIManager>
        {
        [SerializeField] private Text gameMessage;
        [SerializeField] private Text scoreMessage;
        [SerializeField] private Text bulletMessage;

        public void ChangeGameMessage(string message)
        {
            gameMessage.text = message;
        }
        public void ChangeScoreMessage(string score)
        {
            scoreMessage.text = score;
        }
        public void ChangebulletMessage(string bullet)
        {
            bulletMessage.text = bullet;
        }
    }
}

