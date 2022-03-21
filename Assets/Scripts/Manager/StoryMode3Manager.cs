using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
//This script is create and wrote by Jiacheng Sun
public class StoryMode3Manager : ModeManagerBase
{
    //components
    private StoryModePlayerUI storyModePlayerUI;

    protected override void GetUIComponent()
    {
        storyModePlayerUI = this.GetComponent<StoryModePlayerUI>();
    }
    protected override void UpdateUI()
    {
        UpdatePropsInfo();
        UpdateBulletInfo();
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
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        UnableAllPlayers();
        storyModePlayerUI.StartChat();
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("really? There is no one here?"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("What is that in the sky?"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Helicopter? !"));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("hey, I think they saw me, at least, they saw the smoke."));
        yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I need to get rid of the zombies here so they can land."));

        storyModePlayerUI.AfterChat();
        storyModePlayerUI.ChangeGameMessage("Defeat all zombies!");
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
            yield return m_EndWait;
            yield return base.GameEnding();
        }
        else
        {
            UnableAllPlayers();
            storyModePlayerUI.StartChat();
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("I don't think there should be more zombies here."));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Hey! I'm here!"));
            yield return StartCoroutine(storyModePlayerUI.PlayerChatMessage("Great, the helicopter has landed!"));
            storyModePlayerUI.AfterChat();
            storyModePlayerUI.ChangeGameMessage("Congrats on escaping the city!");
            yield return m_EndWait;
            storyModePlayerUI.ChangeGameMessage("But that doesn't mean you are safe.");
            yield return m_EndWait;
            SwitchToScene("GameStartUI");
            yield return m_EndWait;
        }
    }
}
