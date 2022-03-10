using UnityEngine;
using Factory;
using System.Collections;
using UnityEngine.SceneManagement;

public class ModeManagerBase : Singleton<ModeManagerBase>
{
    [SerializeField] protected Transform[] m_ZombieSpawnPoint; // spawn points for zombies
    [SerializeField] protected GameObject[] m_PlayerPerfab;

    [SerializeField] protected Transform[] m_PlayerSpawnPoint;
    protected GameObject[] m_PlayerInstance;
    public GameObject[] PlayerInstance
    {
        get { return m_PlayerInstance; }
    }

    [SerializeField] protected float m_StartDelay = 1f;
    [SerializeField] protected float m_EndDelay = 1f;
    protected WaitForSeconds m_StartWait;
    protected WaitForSeconds m_EndWait;

    //components
    protected PlayerManager[] playermanager;
    protected PlayerStats[] playerStats;

    [SerializeField] protected int m_numberOfZombies = 5;
    protected int m_currentSpawningzombieNumber; // The Zombie Number when Spawn the Zombie

    [SerializeField] protected float m_ZombieSpawnInterval = 0.5f;

    protected virtual void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        SpawnPlayer();  //Set the player's starting position
        GameSetting.Instance.Load();
        GetUIComponent();
        StartCoroutine(GameLoop());
    }
    protected virtual void Update()
    {
        UpdateUI();
        if (EndCondition())
        {
            StartCoroutine(GameEnding());
        }
    }
    protected virtual void SpawnPlayer()
    {
        m_PlayerInstance = new GameObject[m_PlayerPerfab.Length];
        playermanager = new PlayerManager[m_PlayerPerfab.Length];
        playerStats = new PlayerStats[m_PlayerPerfab.Length];
        for (int i = 0; i < m_PlayerPerfab.Length; i++)
        {
            m_PlayerInstance[i] = Instantiate(m_PlayerPerfab[i], m_PlayerSpawnPoint[i]) as GameObject;
            playermanager[i] = m_PlayerInstance[i].GetComponent<PlayerManager>();
            playerStats[i] = m_PlayerInstance[i].GetComponent<PlayerStats>();
        }
        UnableAllPlayers();
    }
    protected virtual void SpawnZombies() // Summon zombies one by one
    {
        int spawn_point_number = Random.Range(0, m_ZombieSpawnPoint.Length);
        if (Random.Range(0, 100) < 20)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_ZombieSpawnPoint[spawn_point_number].position);
        }
        else if (Random.Range(0, 100) < 40)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateBoomer(m_ZombieSpawnPoint[spawn_point_number].position);
        }
        else if (Random.Range(0, 100) < 60)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_ZombieSpawnPoint[spawn_point_number].position);
        }
        else if (Random.Range(0, 100) < 80)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_ZombieSpawnPoint[spawn_point_number].position);
        }
        else
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_ZombieSpawnPoint[spawn_point_number].position);
        }
    }
    protected virtual IEnumerator GameLoop()
    {
        while (!EndCondition())
        {
            yield return StartCoroutine(GameStarting());
            yield return StartCoroutine(ZombieSpawning());
            yield return StartCoroutine(GamePlaying());
            yield return StartCoroutine(BeforeEnding());
        }
    }
    protected virtual IEnumerator GameStarting()
    {
        m_currentSpawningzombieNumber = 0;
        yield return m_StartWait;
    }
    protected virtual IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        EnableAllPlayers();
        if (!EndCondition())
        {
            while (m_currentSpawningzombieNumber < m_numberOfZombies)
            {
                SpawnZombies();
                m_currentSpawningzombieNumber++;
                yield return new WaitForSeconds(m_ZombieSpawnInterval);
            }
        }
    }
    protected virtual IEnumerator GamePlaying()
    {
        while (!WinCondition())
        {
            yield return null;
        }
    }
    protected virtual IEnumerator BeforeEnding()
    {
        yield return GameEnding();
    }
    protected virtual IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        UnableAllPlayers();
        yield return m_EndWait;
        SwitchToScene("GameStartUI");
    }
    public bool AllZombieDead() //Check the isDead property of all zombies, if all are dead then the player wins and add score as well.
    {
        return GameFactoryManager.Instance.EnemyFact.GetZombieCount() == 0;
    }
    public bool AllNonAIPlayerDead() //Check the isDead property of all players
    {
        for(int i=0; i < playerStats.Length; i++)
        {
            if(!playerStats[i].IsDead && playerStats[i].ID != PlayerID.AI)
            {
                return false;
            }
        }
        return true;
    }
    protected void UnableAllPlayers()
    {
        for (int i = 0; i < playermanager.Length; i++)
        {
            playermanager[i].Enable(false);
        }
    }
    protected void EnableAllPlayers()
    {
        for (int i = 0; i < playermanager.Length; i++)
        {
            playermanager[i].Enable(true);
        }
    }
    protected void SwitchToScene(string scene)
    {
        GameSetting.Instance.Save();
        SceneManager.LoadScene(scene);
    }
    protected void DestroyAllZombie()
    {
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemy.Length; i++)
        {
            GameObject.Destroy(allEnemy[i]);
        }
    }
    protected virtual bool WinCondition()
    {
        return AllZombieDead();
    }
    protected virtual bool EndCondition()
    {
        return AllNonAIPlayerDead();
    }
    protected virtual void UpdateUI()
    {
    }
    protected virtual void GetUIComponent()
    { }
}
