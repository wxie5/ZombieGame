using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//This script is create and wrote by Jiacheng Sun
public class StoryMode2Manager : MonoBehaviour
{
    private static StoryMode2Manager instance;
    public static StoryMode2Manager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = (StoryMode2Manager)this;
        }
        else
        {
            Debug.LogError("More Than One Instance of Singleton!");
        }
    }

    [SerializeField] private GameObject[] m_zombiePrefab;
    [SerializeField] private Transform[] m_SpawnPoint0;
    [SerializeField] private Transform[] m_SpawnPoint1;
    [SerializeField] private Transform[] m_SpawnPoint2;
    [SerializeField] private Transform[] m_SpawnPoint3;
    [SerializeField] private bool m_RandomSpawn;
    [SerializeField] private ZombieManager[] m_Zombies;
    [SerializeField] private GameObject m_PlayerPerfab;
    [SerializeField] private Transform m_PlayerSpawnPoint;
    [SerializeField] private GameObject m_CamaraCenter;
    [HideInInspector] public GameObject m_PlayerInstance;

    [SerializeField] private float m_StartDelay = 1f;
    [SerializeField] private float m_EndDelay = 1f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_currentSpawningzombieNumber; // The Zombie Number when Spawn the Zombie

    [SerializeField] private float m_ZombieSpawnInterval = 1f;

    //components
    private PlayerManager playermanager;
    private PlayerStats playerStats;
    private StorySinglePlayerUI singlePlayerUI;

    //stage
    private int stage;
    private bool firstWin;
    private bool arriveDoor;
    void Start()
    {
        stage = 0;
        firstWin = true;
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        SpawnPlayer();  //Set the player's starting position
        playermanager = m_PlayerInstance.GetComponent<PlayerManager>();
        playerStats = m_PlayerInstance.GetComponent<PlayerStats>();
        singlePlayerUI = this.GetComponent<StorySinglePlayerUI>();
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        Debug.Log(stage);
        Debug.Log(arriveDoor);
        singlePlayerUI.changeBulletMessage(playerStats.AmmoInfo());
        if (playerStats.IsDead)
        {
            StartCoroutine(GameEnding());
        }
        if(firstWin && arriveDoor)
        {
            StartCoroutine(BeforeEnding());
            firstWin = false;
        }
    }
   
    private void SpawnZombies() // Summon zombies one by one
    {
        if (!arriveDoor)
        {
            if (m_RandomSpawn) //If set random spawn, random types of zombies will be randomly summoned from each spawn point.
            {
                int zombie_number;
                int spawn_point_number;
                zombie_number = Random.Range(0, m_zombiePrefab.Length);
                if (stage == 0)
                {
                    spawn_point_number = Random.Range(0, m_SpawnPoint0.Length);
                    m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint0[spawn_point_number]) as GameObject;
                }
                if (stage == 1)
                {
                    spawn_point_number = Random.Range(0, m_SpawnPoint1.Length);
                    m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint1[spawn_point_number]) as GameObject;
                }
                if (stage == 2)
                {
                    spawn_point_number = Random.Range(0, m_SpawnPoint2.Length);
                    m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint2[spawn_point_number]) as GameObject;
                }
                if (stage == 3)
                {
                    spawn_point_number = Random.Range(0, m_SpawnPoint3.Length);
                    m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_zombiePrefab[zombie_number], m_SpawnPoint3[spawn_point_number]) as GameObject;
                }
            }
            else //Otherwise, the specified zombie will be spawned at the specified location
            {
                m_Zombies[m_currentSpawningzombieNumber].m_Instance = Instantiate(m_Zombies[m_currentSpawningzombieNumber].m_ZombieType, m_Zombies[m_currentSpawningzombieNumber].m_SpawnPoint) as GameObject;
            }
            m_currentSpawningzombieNumber++;
        }
    } 

    private void SpawnPlayer()
    {
        m_PlayerInstance = Instantiate(m_PlayerPerfab, m_PlayerSpawnPoint) as GameObject;
        m_CamaraCenter.GetComponent<SimpleCamFollow>().PlayerTrans = m_PlayerInstance.transform; //Set the camera position
    }
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(BeforeStarting());
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(ZombieSpawning());
        yield return StartCoroutine(GamePlaying());
    }

    private IEnumerator BeforeStarting() //The game starts, showing the UI prompt
    {
        playermanager.Enable(false);
        singlePlayerUI.StartChat();
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("This is CRAZY!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Luckily I still have enough alcohol"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Uh, I dropped the rifle on the ground, I better to hold it steady!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("The shelter is on the far right."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Hope there won't be more zombies!"));

        singlePlayerUI.AfterChat();
    }
    private IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        singlePlayerUI.ChangeGameMessage("Go to the shelter on the far right!");
        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        if(!arriveDoor)
        {
            singlePlayerUI.ClearGmaeMessage();
            playermanager.Enable(true);
            while (m_currentSpawningzombieNumber < m_Zombies.Length)
            {
                SpawnZombies();
                yield return new WaitForSeconds(m_ZombieSpawnInterval);
            }
        }
    }
    private IEnumerator GamePlaying() //The player advances to the next stage after defeating all zombies
    {
        while (!arriveDoor && !playerStats.IsDead)
        {
            yield return null;
        }
    }

    private IEnumerator BeforeEnding() 
    {
        for (int i = 0; i < m_Zombies.Length; i++)
        {
            Destroy(m_Zombies[i].m_Instance);
        }
        if (!playerStats.IsDead)
        {
            playermanager.Enable(false);
            singlePlayerUI.StartChat();
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("There are more and more zombies..."));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Not sure how many survivors there are."));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Anyway, hurry to the shelter!"));
            singlePlayerUI.AfterChat();
        }
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator GameEnding()
    {
        if (playerStats.IsDead)
        {
            singlePlayerUI.ChangeGameMessage("YOU Dead!");
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("GameStartUi");
            }
            yield return m_EndWait;
        }
        else
        {
            singlePlayerUI.ChangeGameMessage("Get ready for the next scene!");
            playermanager.Enable(false);
            yield return m_EndWait;
            SceneManager.LoadScene("Story Mode 2");
        }
    }
    public void changeStage(int s)
    {
        stage = s;
    }
    public void ArriveDoor(bool a)
    {
        arriveDoor = a;
    }
}
