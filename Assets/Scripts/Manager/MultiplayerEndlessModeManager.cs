using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
// Manage the game process and UI in Multiplayer Endless Mode
//This script is create and wrote by Jiacheng Sun
public class MultiplayerEndlessModeManager : MonoBehaviour
{
    public GameObject[] m_zombiePrefab;
    public Transform[] m_SpawnPoint;
    private GameObject[] m_Zombies;
    private bool[] m_deadZombies;
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
        UpdateScore();
        UpdateBulletInfo();
        if (AllPlayerDead())
        {
            StartCoroutine(GameEnding());
        }
    }
    private void UpdateBulletInfo()
    {
        multiPlayerUI.changeBulletMessage(0, player0Stats.AmmoInfo());
        multiPlayerUI.changeBulletMessage(1, player1Stats.AmmoInfo());
    }
    private void SpawnZombies() // Summon zombies one by one
    {
        int zombie_number;
        int spawn_point_number;
        if(Random.Range(0,100)<15)
        {
            zombie_number = 0;
        }
        else
        {
            zombie_number = 1;
        }
        spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
        m_Zombies[m_currentSpawningzombieNumber] = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint[spawn_point_number]) as GameObject;
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
        m_Zombies = new GameObject[m_numberOfZombies];
        m_deadZombies = new bool[m_numberOfZombies];
        m_numberOfWaves++;
        m_currentSpawningzombieNumber = 0;
        multiPlayerUI.ChangeGameMessage("Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_Zombies.Length + "  Zombies are coming!");

        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        multiPlayerUI.ClearGmaeMessage();
        player0manager.Enable(true);
        player1manager.Enable(true);
        while (m_currentSpawningzombieNumber< m_Zombies.Length)
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
        for(int i = 0; i<m_numberOfZombies; i++)
        {
            Destroy(m_Zombies[i]);
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
        if(m_currentSpawningzombieNumber < m_numberOfZombies)
        {
            return false;
        }
        for (int i = 0; i < m_Zombies.Length; i++)
        {
            if (!m_Zombies[i].GetComponent<EnemyStats>().IsDead)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateScore()
    {
        for (int i = 0; i < m_Zombies.Length; i++)
        {
            if(m_Zombies[i] == null)
            {
                continue;
            }
            if (!m_deadZombies[i] && m_Zombies[i].GetComponent<EnemyStats>().IsDead)
            {
                m_deadZombies[i] = true;
                m_score += m_Zombies[i].GetComponent<EnemyStats>().Score;
            }
        }
        multiPlayerUI.ChangeScore(m_score);
    }
}
