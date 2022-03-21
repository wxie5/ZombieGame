using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Factory;
//This script is create and wrote by Jiacheng Sun & Bolun Ruan
public class StoryMode1Manager : ModeManagerBase
{
    private StoryModePlayerUI storyModePlayerUI;

    //win condition
    private int collectItem = 0;
    [SerializeField] private GameObject[] collect_Item;
    [SerializeField] private AudioClip pickItem;
    private bool firstWin = true;
    protected override void GetUIComponent()
    {
        storyModePlayerUI = this.GetComponent<StoryModePlayerUI>();
    }
    protected override void UpdateUI()
    {
        UpdatePropsInfo();
        UpdateBulletInfo();
    }
    protected override void Update()
    {
        CollectItem();
        base.Update();
    }
    protected override bool EndCondition()
    {
        return AllNonAIPlayerDead() || WinCondition();
    }
    protected override bool WinCondition()
    {
        return firstWin && AllItemCollected();
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
    private void CollectItem()
    {
        for (int i = 0; i < m_PlayerInstance.Length; i++)
        {
            if (collect_Item[0].activeSelf == true)
            {
                if (Vector3.Distance(m_PlayerInstance[i].transform.position, collect_Item[0].transform.position) < 2f)
                {
                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        collect_Item[0].SetActive(false);
                        collectItem++;
                        AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance[i].transform.position);
                    }
                }
            }
            if (collect_Item[1].activeSelf == true)
            {
                if (Vector3.Distance(m_PlayerInstance[i].transform.position, collect_Item[1].transform.position) < 2f)
                {
                    if (Input.GetKeyDown(KeyCode.U))
                    {
                        collect_Item[1].SetActive(false);
                        collectItem++;
                        AudioSource.PlayClipAtPoint(pickItem, m_PlayerInstance[i].transform.position);
                    }
                }
            }
        }
    }
    protected override void SpawnZombies() // Summon zombies one by one
    {
        if (!AllItemCollected())
        {
            base.SpawnZombies();
        }
    }
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        UnableAllPlayers();
        storyModePlayerUI.StartChat();
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("It's time for me to return to base now that I've completed my forest patrol."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("dudududu.... I'm not sure why no one is connected, but I have to get back."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("What's going on here appears to be a major conflict."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Hello!!!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Is there anyone here?"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("What is that, exactly? Zombies?"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("The entire city is broadcasting. "));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Warning: The biological virus has been spilled and spread; "));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("please go to the nearest shelter as soon as possible. The location is..."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("It seems that zombies have attacked our base."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I need to get the essential supplies and head to the shelter as soon as possible."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I think I need more firepower to fight with these monsters."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("And food, of course."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("ALCOHOL!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Nothing is more important than alcohol at a time like this!"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("No time to waste, zombies can come anytime!"));
        storyModePlayerUI.AfterChat();

        storyModePlayerUI.ChangeGameMessage("Try to find: " + "\n\n\n " + "Gun, Food and ALCOHOL!");
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
            DestroyAllZombie();
            firstWin = false;
            UnableAllPlayers();
            storyModePlayerUI.StartChat();
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Finally!"));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I got all I need now!"));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("These monsters are getting crazy!"));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I need to get out of here!"));
            storyModePlayerUI.AfterChat();

            storyModePlayerUI.ChangeGameMessage("Get ready for the next scene!");
            UnableAllPlayers();
            yield return m_EndWait;
            SwitchToScene("StoryMode2");
        }
    }
    private bool AllItemCollected()
    {
        if(collectItem!=2)
        {
            return false;
        }
        for(int i=0;i<playerStats.Length;i++)
        {
            if(!playerStats[i].IsSingleWeapon())
            {
                return true;
            }
        }
        return false;
    }
}
