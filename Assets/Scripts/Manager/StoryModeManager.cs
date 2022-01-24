using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryModeManager : MonoBehaviour
{
    public GameObject[] m_zombiePrefab;
    public Transform[] m_SpawnPoint;
    public bool m_RandomSpawn;
    public ZombieManager[] m_Zombies;
    public Text m_game_message;
    public GameObject m_PlayerPerfab;
    public Transform m_PlayerSpawnPoint;
    public GameObject m_CamaraCenter;
    [HideInInspector] public GameObject m_PlayerInstance;

    public float m_StartDelay = 1f;
    public float m_EndDelay = 1f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

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

        StartCoroutine(GameLoop());
    }

    private void SpawnZombies() // Summon zombies one by one
    {
        if (m_RandomSpawn) //If set random spawn, random types of zombies will be randomly summoned from each spawn point.
        {
            int zombie_number;
            int spawn_point_number;
            zombie_number = Random.Range(0, m_zombiePrefab.Length);
            spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
            m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint[spawn_point_number]) as GameObject;
        }
        else //Otherwise, the specified zombie will be spawned at the specified location
        {
            m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_Zombies[m_currentSpawningzombieNumber].m_ZombieType, m_Zombies[m_currentSpawningzombieNumber].m_SpawnPoint) as GameObject;
        }
        m_currentSpawningzombieNumber++;
    } 

    private void SpawnPlayer()
    {
        m_PlayerInstance = Instantiate(m_PlayerPerfab, m_PlayerSpawnPoint) as GameObject;
        m_CamaraCenter.GetComponent<SimpleCamFollow>().playerTrans = m_PlayerInstance.transform; //Set the camera position
    }
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(ZombieSpawning());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(BeforeEnding());
        yield return StartCoroutine(GameEnding());
    }


    private IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        playerbehaviour.enabled = false;
        m_game_message.text = "Game Start!" + "\n\n\n " + m_Zombies.Length + "  Zombies are coming!";

        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        m_game_message.text = string.Empty;
        playerbehaviour.enabled = true;
        while (m_currentSpawningzombieNumber<m_Zombies.Length)
        {
            SpawnZombies();
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
                m_game_message.text = "The game will end in: " + "\n\n\n" + counter + " s!";
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        if (playerStats.IsDead)
        {
            m_game_message.text = "YOU Dead!";
        }
        else
        {
            m_game_message.text = "YOU WIN!";
        }
        playerbehaviour.enabled = false;
        yield return m_EndWait;
    }

    private bool AllZombieDead() //Check the isDead property of all zombies, if all are dead then the player wins.
    {
        if(m_currentSpawningzombieNumber < m_Zombies.Length)
        {
            return false;
        }
        for(int i = 0; i < m_Zombies.Length; i++)
        {
            if (!m_Zombies[i].m_Instance.GetComponent<EnemyStats>().IsDead)
            {
                return false;
            }
        }
        return true;
    }
}
