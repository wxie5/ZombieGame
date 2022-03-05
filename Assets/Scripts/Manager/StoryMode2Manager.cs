using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
//This script is create and wrote by Jiacheng Sun
public class StoryMode2Manager : ModeManagerBase
{
    [SerializeField] private Transform[] m_SpawnPoint0;
    [SerializeField] private Transform[] m_SpawnPoint1;
    [SerializeField] private Transform[] m_SpawnPoint2;
    [SerializeField] private Transform[] m_SpawnPoint3;

    //components
    private StoryModePlayerUI storyModePlayerUI;

    //stage
    private int stage;
    private bool firstWin;
    private bool arriveDoor;
    protected override void Start()
    {
        stage = 0;
        base.Start();
        firstWin = true;
        storyModePlayerUI = this.GetComponent<StoryModePlayerUI>();
        StartCoroutine(GameLoop());
    }

    protected override void Update()
    {
        UpdatePropsInfo();
        UpdateBulletInfo();
        base.Update();
        if (firstWin && arriveDoor)
        {
            StartCoroutine(BeforeEnding());
            firstWin = false;
        }
    }
    private void UpdateBulletInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            storyModePlayerUI.changeBulletMessage(playerStats[i].AmmoInfo());
        }
    }
    private void UpdatePropsInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            storyModePlayerUI.ChangePropsMessage_AmmoCapacity(playerStats[i].Props_info_AmmoCap());
            storyModePlayerUI.ChangePropsMessage_Damage(playerStats[i].Props_info_Damage());
            storyModePlayerUI.ChangePropsMessage_MoveSpeed(playerStats[i].Props_info_MoveSpeed());
            storyModePlayerUI.ChangePropsMessage_Offset(playerStats[i].Props_info_Offset());
            storyModePlayerUI.ChangePropsMessage_ShotRate(playerStats[i].Props_info_ShotRate());
        }
    }
    protected override void SpawnZombies() // Summon zombies one by one
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
    protected override IEnumerator GameLoop()
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
        UnableAllPlayers();
        storyModePlayerUI.StartChat();
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("This is CRAZY!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Luckily I still have enough alcohol"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Uh, I dropped the rifle on the ground, I better to hold it steady!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("The shelter is on the far right."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Hope there won't be more zombies!"));

        storyModePlayerUI.AfterChat();
    }
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        storyModePlayerUI.ChangeGameMessage("Go to the shelter on the far right!");
        return base.GameStarting();
    }

    protected override IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        if(!arriveDoor)
        {
            storyModePlayerUI.ClearGmaeMessage();
            EnableAllPlayers();
            while (m_currentSpawningzombieNumber < m_numberOfZombies)
            {
                SpawnZombies();
                yield return new WaitForSeconds(m_ZombieSpawnInterval);
            }
        }
    }
    protected override IEnumerator GamePlaying() //The player advances to the next stage after defeating all zombies
    {
        while (!arriveDoor && !AllPlayerDead())
        {
            yield return null;
        }
    }

    protected override IEnumerator BeforeEnding() 
    {
        DestroyAllZombie();
        if (!AllPlayerDead())
        {
            UnableAllPlayers();
            storyModePlayerUI.StartChat();
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("There are more and more zombies..."));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Not sure how many survivors there are."));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Anyway, hurry to the shelter!"));
            storyModePlayerUI.AfterChat();
        }
        yield return StartCoroutine(GameEnding());
    }

    protected override IEnumerator GameEnding()
    {
        if (AllPlayerDead())
        {
            storyModePlayerUI.ChangeGameMessage("YOU Dead!");
            yield return m_EndWait;
            SwitchToScene("GameStartUi");
        }
        else
        {
            storyModePlayerUI.ChangeGameMessage("TO BE CONTINUE");
            UnableAllPlayers();
            yield return m_EndWait;
            SwitchToScene("GameStartUi");
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
