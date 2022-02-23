using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
// Manage the game process and UI in Multiplayer Endless Mode
//This script is create and wrote by Jiacheng Sun and Bolun Ruan
public class MultiplayerEndlessModeManager : Singleton<MultiplayerEndlessModeManager>
{
    public Transform[] m_SpawnPoint;
    [SerializeField] private GameObject camCenter;
    [SerializeField] private GameObject[] m_PlayerPerfab;
    [SerializeField] private Transform[] m_PlayerSpawnPoint;

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
    private GameObject m_Player0Instance;
    private GameObject m_Player1Instance;
    private PlayerManager player0manager;
    private PlayerManager player1manager;
    private PlayerStats player0Stats;
    private PlayerStats player1Stats;
    private MultiPlayerUI multiPlayerUI;


    void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnPlayer();  //Set the player's starting position
        multiPlayerUI = this.GetComponent<MultiPlayerUI>();
        StartCoroutine(GameLoop());
    }
    private void Update()
    {
        UpdateUI();
        if (AllPlayerDead())
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
        multiPlayerUI.ChangePropsMessage_AmmoCapacity(0, player0Stats.Props_info_AmmoCap());
        multiPlayerUI.ChangePropsMessage_AmmoCapacity(1, player1Stats.Props_info_AmmoCap());
        multiPlayerUI.ChangePropsMessage_Damage(0, player0Stats.Props_info_Damage());
        multiPlayerUI.ChangePropsMessage_MoveSpeed(0, player0Stats.Props_info_MoveSpeed());
        multiPlayerUI.ChangePropsMessage_Offset(0, player0Stats.Props_info_Offset());
        multiPlayerUI.ChangePropsMessage_ShotRate(0, player0Stats.Props_info_ShotRate());
        multiPlayerUI.ChangePropsMessage_Damage(1, player1Stats.Props_info_Damage());
        multiPlayerUI.ChangePropsMessage_MoveSpeed(1, player1Stats.Props_info_MoveSpeed());
        multiPlayerUI.ChangePropsMessage_Offset(1, player1Stats.Props_info_Offset());
        multiPlayerUI.ChangePropsMessage_ShotRate(1, player1Stats.Props_info_ShotRate());

    }
    private void UpdateBulletInfo()
    {
        multiPlayerUI.changeBulletMessage(0, player0Stats.AmmoInfo());
        multiPlayerUI.changeBulletMessage(1, player1Stats.AmmoInfo());
    }
    private void SpawnZombies() // Summon zombies one by one
    {
        int spawn_point_number;
        spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
        if (m_numberOfWaves < 3)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
        }
        else if (m_numberOfWaves < 5)
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
            else if (Random.Range(0, 100) < 70)
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
        m_Player0Instance = Instantiate(m_PlayerPerfab[0], m_PlayerSpawnPoint[0]) as GameObject;
        m_Player1Instance = Instantiate(m_PlayerPerfab[1], m_PlayerSpawnPoint[1]) as GameObject;
        player0manager = m_Player0Instance.GetComponent<PlayerManager>();
        player0Stats = m_Player0Instance.GetComponent<PlayerStats>();
        player0manager.Enable(false);
        player1manager = m_Player1Instance.GetComponent<PlayerManager>();
        player1Stats = m_Player1Instance.GetComponent<PlayerStats>();
        player1manager.Enable(false);

        camCenter.GetComponent<MultiplayerCamara>().Player0Trans = m_Player0Instance.transform;
        camCenter.GetComponent<MultiplayerCamara>().Player1Trans = m_Player1Instance.transform;
        camCenter.GetComponent<MultiplayerCamara>().Player0Dead = false;
        camCenter.GetComponent<MultiplayerCamara>().Player1Dead = false;
    }
    private IEnumerator GameLoop()
    {
        while (!AllPlayerDead())
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
        if(m_numberOfWaves%5 == 0)
        {
            player0Stats.ChangeMaximumPropsNumber(1);
            player1Stats.ChangeMaximumPropsNumber(1);
        }
        m_currentSpawningzombieNumber = 0;
        multiPlayerUI.ChangeGameMessage("Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_numberOfZombies + "  Zombies are coming!");

        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        multiPlayerUI.ClearGmaeMessage();
        player0manager.Enable(true);
        player1manager.Enable(true);
        while (m_currentSpawningzombieNumber< m_numberOfZombies)
        {
            SpawnZombies();
            m_currentSpawningzombieNumber++;
            yield return new WaitForSeconds(m_ZombieSpawnInterval);
        }
    }
    private bool AllPlayerDead()
    {
        if(player0Stats.IsDead)
        {
            camCenter.GetComponent<MultiplayerCamara>().Player0Dead = true;
        }
        if (player1Stats.IsDead)
        {
            camCenter.GetComponent<MultiplayerCamara>().Player1Dead = true;
        }
        return player0Stats.IsDead && player1Stats.IsDead;
    }
    private IEnumerator GamePlaying() //The player advances to the next stage after defeating all zombies
    {
        while (!AllZombieDead() && !AllPlayerDead())
        {
            yield return null;
        }
    }

    private IEnumerator BeforeEnding() //Give player 5 seconds to pick up props
    {
        if (!AllPlayerDead())
        {
            int counter = 5;
            multiPlayerUI.ClearGmaeMessage();
            while (counter > 0)
            {
                multiPlayerUI.ChangeGameMessage("The next wave of zombies will arrive in: " + "\n\n\n" + counter + " s!");
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        multiPlayerUI.ChangeGameMessage("YOU LOSE!" + "\n\n\n" + "Your final score: " + m_score);
        player0manager.Enable(false);
        player1manager.Enable(false);
        yield return m_EndWait;
        SceneManager.LoadScene("GameStartUi");
    }

    private bool AllZombieDead() //Check the isDead property of all zombies, if all are dead then the player wins and add score as well.
    {
        return GameFactoryManager.Instance.EnemyFact.GetZombieCount() == 0;
    }

    private void UpdateScore()
    {
        multiPlayerUI.ChangeScore(m_score);
    }

    public void onDeadAddScore(int score)
    {
        m_score += score;
    }
    public void AddScore(int amount)
    {
        m_score += amount;
    }
}
