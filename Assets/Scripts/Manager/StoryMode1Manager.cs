using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//This script is create and wrote by Jiacheng Sun
public class StoryMode1Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_zombiePrefab;
    [SerializeField] private Transform[] m_SpawnPoint;
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

    [SerializeField] private float m_ZombieSpawnInterval = 0.5f;

    //components
    private PlayerManager playermanager;
    private PlayerStats playerStats;
    private StorySinglePlayerUI singlePlayerUI;

    //win condition
    private int collectItem;
    [SerializeField] private GameObject[] collect_Item;
    [SerializeField] private AudioClip pickItem;
    void Start()
    {

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        collectItem = 0;
        SpawnPlayer();  //Set the player's starting position
        playermanager = m_PlayerInstance.GetComponent<PlayerManager>();
        playerStats = m_PlayerInstance.GetComponent<PlayerStats>();
        singlePlayerUI = this.GetComponent<StorySinglePlayerUI>();
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        singlePlayerUI.changeBulletMessage(playerStats.AmmoInfo());
        CollectItem();
        if (playerStats.IsDead)
        {
            StartCoroutine(GameEnding());
        }
    }
    private void CollectItem()
    {
        if (collect_Item[0].activeSelf == true)
        {
            if (Vector3.Distance(m_PlayerInstance.transform.position, collect_Item[0].transform.position) < 2f)
            {
                collect_Item[0].SetActive(false);
                collectItem++;
                AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance.transform.position);
            }
        }
        if (collect_Item[1].activeSelf == true)
        { 
            if (Vector3.Distance(m_PlayerInstance.transform.position, collect_Item[1].transform.position) < 2f)
            {
                collect_Item[1].SetActive(false);
                collectItem++;
                AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance.transform.position);
            }
        }
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
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Zombie virus broke out in the city."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I have been in this city for a week."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Food is running out, rescue is not coming."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I have to figure out a way to get out of this ** place!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("But before that, I have to find three things in town right now."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("They are A, B and C."));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Zombies kept pouring in. Fortunately, my pistol ammunition was still enough, but I don't know if this firepower can stop these monsters!"));
        yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I need to speed up!"));
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
        while (m_currentSpawningzombieNumber<m_Zombies.Length)
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

    private IEnumerator BeforeEnding() //Give player 5 seconds to pick up props
    {
        if (!playerStats.IsDead)
        {
            playermanager.Enable(false);
            singlePlayerUI.StartChat();
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("Finally!"));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("These monsters are getting crazy!"));
            yield return StartCoroutine(singlePlayerUI.PlayerChatMessage("I need to get out of here!"));
            singlePlayerUI.AfterChat();
        }
    }

    private IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        if (playerStats.IsDead)
        {
            singlePlayerUI.ChangeGameMessage("YOU Dead!");
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("GameStartUi");
            }
            yield return new WaitForSeconds(5f);
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
        Debug.Log(playerStats.IsSingleWeapon());
        return !playerStats.IsSingleWeapon() && collectItem == 2;
    }
}
