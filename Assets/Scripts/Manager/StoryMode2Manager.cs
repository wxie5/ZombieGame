using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Factory;
//This script is create and wrote by Jiacheng Sun
public class StoryMode2Manager : Singleton<StoryMode2Manager>
{
    [SerializeField] private Transform[] m_SpawnPoint0;
    [SerializeField] private Transform[] m_SpawnPoint1;
    [SerializeField] private Transform[] m_SpawnPoint2;
    [SerializeField] private Transform[] m_SpawnPoint3;
    [SerializeField] private GameObject m_PlayerPerfab;
    [SerializeField] private Transform m_PlayerSpawnPoint;
    [SerializeField] private GameObject m_CamaraCenter;
    [HideInInspector] public GameObject m_PlayerInstance;

    [SerializeField] private float m_StartDelay = 1f;
    [SerializeField] private float m_EndDelay = 1f;
    [SerializeField] private int m_numberOfZombies;

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
        UpdatePropsInfo();
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
    private void UpdatePropsInfo()
    {
        singlePlayerUI.ChangePropsMessage_AmmoCapacity(playerStats.Props_info_AmmoCap());
        singlePlayerUI.ChangePropsMessage_Damage(playerStats.Props_info_Damage());
        singlePlayerUI.ChangePropsMessage_MoveSpeed(playerStats.Props_info_MoveSpeed());
        singlePlayerUI.ChangePropsMessage_Offset(playerStats.Props_info_Offset());
        singlePlayerUI.ChangePropsMessage_ShotRate(playerStats.Props_info_ShotRate());
    }
    private void SpawnZombies() // Summon zombies one by one
    {
        if (!arriveDoor)
        {
            int spawn_point_number;
            if (stage == 0)
                {
                spawn_point_number = Random.Range(0, m_SpawnPoint0.Length);
                if (Random.Range(0, 100) < 20)
                {
                    GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint0[spawn_point_number].position);
                }
                else if (Random.Range(0, 100) < 40)
                {
                    GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_SpawnPoint0[spawn_point_number].position);
                }
                else if (Random.Range(0, 100) < 60)
                {
                    GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint0[spawn_point_number].position);
                }
                else if (Random.Range(0, 100) < 80)
                {
                    GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint0[spawn_point_number].position);
                }
                else
                {
                    GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint0[spawn_point_number].position);
                }
             }
             if (stage == 1)
             {
                 spawn_point_number = Random.Range(0, m_SpawnPoint1.Length);
                 if (Random.Range(0, 100) < 20)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint1[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 40)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_SpawnPoint1[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 60)
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint1[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 80)
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint1[spawn_point_number].position);
                 }
                 else
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint1[spawn_point_number].position);
                 }
             }
             if (stage == 2)
             {
                 spawn_point_number = Random.Range(0, m_SpawnPoint2.Length);
                 if (Random.Range(0, 100) < 20)
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint2[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 40)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_SpawnPoint2[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 60)
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint2[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 80)
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint2[spawn_point_number].position);
                 }
                 else
                 {
                     GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint2[spawn_point_number].position);
                 }
             }
             if (stage == 3)
             {
                 spawn_point_number = Random.Range(0, m_SpawnPoint3.Length);
                 if (Random.Range(0, 100) < 20)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint3[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 40)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_SpawnPoint3[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 60)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint3[spawn_point_number].position);
                 }
                 else if (Random.Range(0, 100) < 80)
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint3[spawn_point_number].position);
                 }
                 else
                 {
                    GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint3[spawn_point_number].position);
                 }
            }
        }
        m_currentSpawningzombieNumber++;
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
            while (m_currentSpawningzombieNumber < m_numberOfZombies)
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
        DestroyAllZombie();
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
            singlePlayerUI.ChangeGameMessage("TO BE CONTINUE");
            playermanager.Enable(false);
            yield return m_EndWait;
            SceneManager.LoadScene("GameStartUi");
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
    private void DestroyAllZombie()
    {
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemy.Length; i++)
        {
            GameObject.Destroy(allEnemy[i]);
        }
    }
}
