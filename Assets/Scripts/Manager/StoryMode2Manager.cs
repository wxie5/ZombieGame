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
    private bool firstWin = true;

    //stage
    private int stage = 0;
    public int Stage
    {
        get { return stage; }
        set { stage = value; }
    }
    private bool arriveDoor = false;
    public bool ArriveDoor
    {
        get { return arriveDoor; }
        set { arriveDoor = value; }
    }

    protected override void GetUIComponent()
    {
        storyModePlayerUI = this.GetComponent<StoryModePlayerUI>();
    }
    protected override void UpdateUI()
    {
        UpdatePropsInfo();
        UpdateBulletInfo();
    }
    protected override bool EndCondition()
    {
        return AllNonAIPlayerDead() || WinCondition();
    }
    protected override bool WinCondition()
    {
        return firstWin && arriveDoor;
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
    protected override void SpawnZombies() // Summon zombies one by one, location will chanege base on the stage
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
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        UnableAllPlayers();
        storyModePlayerUI.StartChat();
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("This is CRAZY!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Luckily I still have enough alcohol"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Uh, I dropped the rifle on the ground, I better to hold it steady!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("The shelter is on the far right."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Hope there won't be more zombies!"));

        storyModePlayerUI.AfterChat();
        storyModePlayerUI.ChangeGameMessage("Go to the shelter on the far right!");
        yield return base.GameStarting();
    }
    protected override IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        storyModePlayerUI.ClearGmaeMessage();
        return base.ZombieSpawning();
    }
    protected override IEnumerator GameEnding()
    {
        if (AllNonAIPlayerDead())
        {
            storyModePlayerUI.ChangeGameMessage("YOU Dead!");
            yield return base.GameEnding();
        }
        else
        {
            firstWin = false;
            DestroyAllZombie();
            UnableAllPlayers();
            storyModePlayerUI.StartChat();
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("There are more and more zombies..."));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Not sure how many survivors there are."));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Anyway, hurry to the shelter!"));
            storyModePlayerUI.AfterChat();
            storyModePlayerUI.ChangeGameMessage("Get ready for the next scene!");
            yield return m_EndWait;
            SwitchToScene("StoryMode3");
        }
    }
}
