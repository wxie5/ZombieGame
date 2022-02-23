using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Factory;
//This script is create and wrote by Jiacheng Sun & Bolun Ruan
public class StoryMode1Manager : Singleton<StoryMode1Manager>
{
    [SerializeField] private Transform[] m_SpawnPoint;
    [SerializeField] private GameObject m_PlayerPerfab;
    [SerializeField] private Transform m_PlayerSpawnPoint;
    [SerializeField] private GameObject m_CamaraCenter;
    private GameObject m_PlayerInstance;

    [SerializeField] private float m_StartDelay = 1f;
    [SerializeField] private float m_EndDelay = 1f;
    [SerializeField] private int m_numberOfZombies;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;

    private int m_currentSpawningzombieNumber; // The Zombie Number when Spawn the Zombie

    [SerializeField] private float m_ZombieSpawnInterval = 0.5f;

    //components
    private PlayerManager playermanager;
    private PlayerStats playerStats;
    private StorySinglePlayerUI singlePlayerUI;

    //win condition
    private int collectItem;
    [SerializeField] private GameObject[] collect_Item;
    [SerializeField] private AudioClip pickItem;
    private bool firstWin;
    void Start()
    {

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        collectItem = 0;
        firstWin = true;
        SpawnPlayer();  //Set the player's starting position
        playermanager = m_PlayerInstance.GetComponent<PlayerManager>();
        playerStats = m_PlayerInstance.GetComponent<PlayerStats>();
        singlePlayerUI = this.GetComponent<StorySinglePlayerUI>();
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        singlePlayerUI.changeBulletMessage(playerStats.AmmoInfo());
        UpdatePropsInfo();
        CollectItem();
        if (playerStats.IsDead)
        {
            StartCoroutine(GameEnding());
        }
        if(AllItemCollected() && firstWin)
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
    private void CollectItem()
    {
        if (collect_Item[0].activeSelf == true)
        {
            if (Vector3.Distance(m_PlayerInstance.transform.position, collect_Item[0].transform.position) < 2f)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    collect_Item[0].SetActive(false);
                    collectItem++;
                    AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance.transform.position);
                }
            }
        }
        if (collect_Item[1].activeSelf == true)
        { 
            if (Vector3.Distance(m_PlayerInstance.transform.position, collect_Item[1].transform.position) < 2f)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    collect_Item[1].SetActive(false);
                    collectItem++;
                    AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance.transform.position);
                }
            }
        }
    }
    private void SpawnZombies() // Summon zombies one by one
    {
        if (!AllItemCollected())
        {
            int spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
            if (Random.Range(0, 100) < 20)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if(Random.Range(0, 100) < 40)
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
        yield return StartCoroutine(BeforeEnding());
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator BeforeStarting() //The game starts, showing the UI prompt
    {
        playermanager.Enable(false);
        singlePlayerUI.StartChat();
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("It's time for me to return to base now that I've completed my forest patrol."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("dudududu.... I'm not sure why no one is connected, but I have to get back."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("What's going on here appears to be a major conflict."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Hello!!!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Is there anyone here?"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("What is that, exactly? Zombies?"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("The entire city is broadcasting. "));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Warning: The biological virus has been spilled and spread; "));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("please go to the nearest shelter as soon as possible. The location is..."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("It seems that zombies have attacked our base."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I need to get the essential supplies and head to the shelter as soon as possible."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I think I need more firepower to fight with these monsters."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("And food, of course."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("ALCOHOL!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Nothing is more important than alcohol at a time like this!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("No time to waste, zombies can come anytime!"));
        singlePlayerUI.AfterChat();
    }
    private IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        singlePlayerUI.ChangeGameMessage("Try to find: " + "\n\n\n " + "Gun, Food and ALCOHOL!");
        yield return m_StartWait;
    }

    private IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        singlePlayerUI.ClearGmaeMessage();
        playermanager.Enable(true);
        while (m_currentSpawningzombieNumber<m_numberOfZombies)
        {
            SpawnZombies();
            yield return new WaitForSeconds(m_ZombieSpawnInterval);
        }
    }
    private IEnumerator GamePlaying() //The player advances to the next stage after defeating all zombies
    {
        while (!AllItemCollected() && !playerStats.IsDead)
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
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Finally!"));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I got all I need now!"));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("These monsters are getting crazy!"));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I need to get out of here!"));
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

    private bool AllItemCollected()
    {
        return (!playerStats.IsSingleWeapon()) && collectItem == 2;
    }

    private void DestroyAllZombie()
    {
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        for(int i = 0; i<allEnemy.Length; i++)
        {
            GameObject.Destroy(allEnemy[i]);
        }
    }
}
