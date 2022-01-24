using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manage the game process and UI in Endless Mode

public class EndlessModeManager : MonoBehaviour
{
    public GameObject[] m_zombiePrefab;
    public Transform[] m_SpawnPoint;
    private GameObject[] m_Zombies;
    private bool[] m_deadZombies;
    public Text m_game_message;
    public Text m_scoreMessage;
    public GameObject m_PlayerPerfab;
    public Transform m_PlayerSpawnPoint;
    public GameObject m_CamaraCenter;
    [HideInInspector] public GameObject m_PlayerInstance;

    public float m_StartDelay = 1f;
    public float m_EndDelay = 1f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_numberOfWaves = 0;
    [HideInInspector] public int m_score = 0;

    private int m_numberOfZombies = 5;
    private int m_currentSpawningzombieNumber; // The Zombie Number when Spawn the Zombie
    public float m_ZombieSpawnInterval = 0.5f;

    //components
    private PlayerBehaviour playerbehaviour;
    private PlayerStats playerStats;
    


    void Start()
    {

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnPlayer();  //Set the player's starting position
        playerbehaviour = m_PlayerInstance.GetComponent<PlayerBehaviour>();
        playerStats = m_PlayerInstance.GetComponent<PlayerStats>();
        playerbehaviour.enabled = false;
        StartCoroutine(GameLoop());
    }
    private void FixedUpdate()
    {
        m_scoreMessage.text = "Current Score: " + m_score;
        UpdateScore();
        if (playerStats.IsDead)
        {
            StartCoroutine(GameEnding());
        }
    }
    private void SpawnZombies() // Summon zombies one by one
    {
        int zombie_number;
        int spawn_point_number;
        zombie_number = Random.Range(0, m_zombiePrefab.Length);
        spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
        Debug.Log(m_currentSpawningzombieNumber);
        m_Zombies[m_currentSpawningzombieNumber] = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint[spawn_point_number]) as GameObject;
    } 

    private void SpawnPlayer()
    {
        m_PlayerInstance = Instantiate(m_PlayerPerfab, m_PlayerSpawnPoint) as GameObject;
        m_CamaraCenter.GetComponent<SimpleCamFollow>().playerTrans = m_PlayerInstance.transform; //Set the camera position
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
        m_Zombies = new GameObject[m_numberOfZombies];
        m_deadZombies = new bool[m_numberOfZombies];
        m_numberOfWaves++;
        m_currentSpawningzombieNumber = 0;
        m_game_message.text = "Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_Zombies.Length + "  Zombies are coming!";

        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        m_game_message.text = string.Empty;
        playerbehaviour.enabled = true;
        Debug.Log("1");
        while (m_currentSpawningzombieNumber< m_Zombies.Length)
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
            m_game_message.text = string.Empty;
            while (counter > 0)
            {
                m_game_message.text = "The next wave of zombies will arrive in: " + "\n\n\n" + counter + " s!";
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
        for(int i = 0; i<m_numberOfZombies; i++)
        {
            m_Zombies[i].SetActive(false);
        }
    }

    private IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        m_scoreMessage.text = string.Empty;
        m_game_message.text = "YOU DEAD!" + "\n\n\n" + "Your final score: " + m_score;
        playerbehaviour.enabled = false;
        yield return m_EndWait;
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
            if (!m_deadZombies[i] && m_Zombies[i].GetComponent<EnemyStats>().IsDead)
            {
                m_deadZombies[i] = true;
                m_score += m_Zombies[i].GetComponent<EnemyStats>().Score;
            }
        }
    }
}
