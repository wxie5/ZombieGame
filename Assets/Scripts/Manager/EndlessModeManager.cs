using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
// Manage the game process and UI in Endless Mode
//This script is create and wrote by Jiacheng Sun
public class EndlessModeManager : Singleton<EndlessModeManager>
{
    public Transform[] m_SpawnPoint;
    [SerializeField] private GameObject m_PlayerPerfab;
    [SerializeField] private Transform m_PlayerSpawnPoint;
    [SerializeField] private GameObject m_CamaraCenter;
    [HideInInspector] public GameObject m_PlayerInstance;

    [SerializeField] private float m_StartDelay = 1f;
    [SerializeField] private float m_EndDelay = 1f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_numberOfWaves = 0;
    [HideInInspector] public int m_score = 0;

    private int m_numberOfZombies = 5;
    private int m_currentSpawningzombieNumber; // The Zombie Number when Spawn the Zombie
    [SerializeField] private float m_ZombieSpawnInterval = 0.5f;

    //components
    private PlayerManager playermanager;
    private PlayerStats playerStats;
    private SinglePlayerUI singlePlayerUI;


    void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnPlayer();  //Set the player's starting position
        playermanager = m_PlayerInstance.GetComponent<PlayerManager>();
        playerStats = m_PlayerInstance.GetComponent<PlayerStats>();
        singlePlayerUI = gameObject.GetComponent<SinglePlayerUI>();
        playermanager.Enable(false);
        StartCoroutine(GameLoop());
    }
    private void Update()
    {
        UpdateUI();
        if (playerStats.IsDead)
        {
            StartCoroutine(GameEnding());
        }
    }

    private void UpdateUI()
    {
        UpdateScore();
        UpdateBulletInfo();
        UpdatePropsInfo();
    }
    private void UpdatePropsInfo()
    {
        singlePlayerUI.ChangePropsMessage_AmmoCapacity(playerStats.Props_info_AmmoCap());
        singlePlayerUI.ChangePropsMessage_Damage(playerStats.Props_info_Damage());
        singlePlayerUI.ChangePropsMessage_MoveSpeed(playerStats.Props_info_MoveSpeed());
        singlePlayerUI.ChangePropsMessage_Offset(playerStats.Props_info_Offset());
        singlePlayerUI.ChangePropsMessage_ShotRate(playerStats.Props_info_ShotRate());
    }
    private void UpdateBulletInfo()
    {
        singlePlayerUI.changeBulletMessage(playerStats.AmmoInfo());
    }
    private void SpawnZombies() // Summon zombies one by one
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

    private void SpawnPlayer()
    {
        m_PlayerInstance = Instantiate(m_PlayerPerfab, m_PlayerSpawnPoint) as GameObject;
        m_CamaraCenter.GetComponent<SimpleCamFollow>().PlayerTrans = m_PlayerInstance.transform; //Set the camera position
    }
    private IEnumerator GameLoop()
    {
        while (!m_PlayerInstance.GetComponent<PlayerStats>().IsDead)
        {
            yield return StartCoroutine(GameStarting());
            yield return StartCoroutine(ZombieSpawning());
            yield return StartCoroutine(GamePlaying());
            yield return StartCoroutine(BeforeEnding());
        }
    }


    private IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        m_numberOfZombies += Random.Range(1, 3);
        m_numberOfWaves++;
        if (m_numberOfWaves % 5 == 0)
        {
            playerStats.ChangeMaximumPropsNumber(1);
        }
        m_currentSpawningzombieNumber = 0;
        singlePlayerUI.ChangeGameMessage("Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_numberOfZombies + "  Zombies are coming!");

        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        singlePlayerUI.ClearGmaeMessage();
        playermanager.Enable(true);
        while (m_currentSpawningzombieNumber< m_numberOfZombies)
        {
            SpawnZombies();
            m_currentSpawningzombieNumber++;
            yield return new WaitForSeconds(m_ZombieSpawnInterval);
        }
    }
    private IEnumerator GamePlaying() //The player advances to the next stage after defeating all zombies
    {
        while (!AllZombieDead() && !playerStats.IsDead)
        {
            yield return null;
        }
    }

    private IEnumerator BeforeEnding() //Give player 5 seconds to pick up props
    {
        if (!playerStats.IsDead)
        {
            int counter = 5;
            singlePlayerUI.ClearGmaeMessage();
            while (counter > 0)
            {
                singlePlayerUI.ChangeGameMessage("The next wave of zombies will arrive in: " + "\n\n\n" + counter + " s!");
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        singlePlayerUI.ChangeGameMessage("YOU DEAD!" + "\n\n\n" + "Your final score: " + m_score);
        playermanager.Enable(false);
        yield return m_EndWait;
        SceneManager.LoadScene("GameStartUi");
    }

    private bool AllZombieDead() //Check the isDead property of all zombies, if all are dead then the player wins and add score as well.
    {
        return GameFactoryManager.Instance.EnemyFact.GetZombieCount() == 0;
    }
    public void AddScore(int amount)
    {
        m_score += amount;
    }

    private void UpdateScore()
    {
        singlePlayerUI.ChangeScore(m_score);
    }

    public void onDeadAddScore(int score)
    {
        m_score += score;
    }
}
