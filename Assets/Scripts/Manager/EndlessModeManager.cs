using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
// Manage the game process and UI in Endless Mode
//This script is create and wrote by Jiacheng Sun
public class EndlessModeManager : ModeManagerBase
{
    private int m_numberOfWaves = 0;
    private int m_score = 0;
    private EndlessModePlayerUI endlessModePlayerUI;

    protected override void Start()
    {
        base.Start();
        endlessModePlayerUI = gameObject.GetComponent<EndlessModePlayerUI>();
        StartCoroutine(GameLoop());
    }
    protected override void Update()
    {
        UpdateUI();
        base.Update();
    }

    private void UpdateUI()
    {
        UpdateScore();
        UpdateBulletInfo();
        UpdatePropsInfo();
    }
    protected override void SpawnZombies() // Summon zombies one by one
    {
        int spawn_point_number;
        spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
        if(m_numberOfWaves < 3)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
        }
        else if(m_numberOfWaves < 5)
        {
            if (Random.Range(0, 100) < 50)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else if (m_numberOfWaves < 7)
        {
            if (Random.Range(0, 100) < 40)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if(Random.Range(0, 100) < 70)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
            else 
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else if (m_numberOfWaves < 10)
        {
            if (Random.Range(0, 100) < 25)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 50)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 75)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else
        {
            if (Random.Range(0, 100) < 20)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 40)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 60)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 80)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
        }
    } 
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        m_numberOfZombies += Random.Range(1, 3);
        m_numberOfWaves++;
        if (m_numberOfWaves % 5 == 0)
        {
            playerStats[0].ChangeMaximumPropsNumber(1);
        }
        endlessModePlayerUI.ChangeGameMessage("Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_numberOfZombies + "  Zombies are coming!");
        return base.GameStarting();
    }
    protected override IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        endlessModePlayerUI.ClearGmaeMessage();
        return base.ZombieSpawning();
    }
    protected override IEnumerator BeforeEnding() //Give player 5 seconds to pick up props
    {
        if (!AllPlayerDead())
        {
            int counter = 5;
            endlessModePlayerUI.ClearGmaeMessage();
            while (counter > 0)
            {
                endlessModePlayerUI.ChangeGameMessage("The next wave of zombies will arrive in: " + "\n\n\n" + counter + " s!");
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
    }
    protected override IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        endlessModePlayerUI.ChangeGameMessage("YOU DEAD!" + "\n\n\n" + "Your final score: " + m_score);
        return base.GameEnding();
    }
    public void AddScore(int amount)
    {
        m_score += amount;
    }
    private void UpdatePropsInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            endlessModePlayerUI.ChangePropsMessage_AmmoCapacity(playerStats[i].Props_info_AmmoCap(),i);
            endlessModePlayerUI.ChangePropsMessage_Damage(playerStats[i].Props_info_Damage(), i);
            endlessModePlayerUI.ChangePropsMessage_MoveSpeed(playerStats[i].Props_info_MoveSpeed(), i);
            endlessModePlayerUI.ChangePropsMessage_Offset(playerStats[i].Props_info_Offset(), i);
            endlessModePlayerUI.ChangePropsMessage_ShotRate(playerStats[i].Props_info_ShotRate(), i);
        }
    }
    private void UpdateBulletInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            endlessModePlayerUI.changeBulletMessage(playerStats[i].AmmoInfo(),i);
        }
    }
    private void UpdateScore()
    {
        endlessModePlayerUI.ChangeScore(m_score);
    }
    public void onDeadAddScore(int score)
    {
        m_score += score;
    }
}
