using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Factory;
// Manage the game process and UI in Multiplayer Endless Mode
//This script is create and wrote by Jiacheng Sun and Bolun Ruan

public class AIMultiplayerEndlessModeManager : ModeManagerBase
{
    private int m_numberOfWaves = 0;
    private int m_score = 0;

    private EndlessModePlayerUI endlessModePlayerUI;


    protected override void Start()
    {
        base.Start();
        endlessModePlayerUI = this.GetComponent<EndlessModePlayerUI>();
        StartCoroutine(GameLoop());
    }
    protected override void Update()
    {
        UpdateUI();
        base.Update();
    }
    private void UpdateUI()
    {
        UpdateScore();
        UpdateBulletInfo();
        UpdatePropsInfo();
    }
    private void UpdatePropsInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            endlessModePlayerUI.ChangePropsMessage_AmmoCapacity(playerStats[i].Props_info_AmmoCap(), i);
            endlessModePlayerUI.ChangePropsMessage_Damage(playerStats[i].Props_info_Damage(), i);
            endlessModePlayerUI.ChangePropsMessage_MoveSpeed(playerStats[i].Props_info_MoveSpeed(),i);
            endlessModePlayerUI.ChangePropsMessage_Offset(playerStats[i].Props_info_Offset(),i);
            endlessModePlayerUI.ChangePropsMessage_ShotRate(playerStats[i].Props_info_ShotRate(),i);
        }

    }
    private void UpdateBulletInfo()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            endlessModePlayerUI.changeBulletMessage(playerStats[i].AmmoInfo(),i);
        }
    }
    protected override void SpawnZombies() // Summon zombies one by one
    {
        int spawn_point_number;
        spawn_point_number = Random.Range(0, m_SpawnPoint.Length);
        if (m_numberOfWaves < 3)
        {
            GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
        }
        else if (m_numberOfWaves < 5)
        {
            if (Random.Range(0, 100) < 50)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else if (m_numberOfWaves < 7)
        {
            if (Random.Range(0, 100) < 40)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 70)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else if (m_numberOfWaves < 10)
        {
            if (Random.Range(0, 100) < 25)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 50)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateTank(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 75)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiatePosion(m_SpawnPoint[spawn_point_number].position);
            }
            else
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateRunner(m_SpawnPoint[spawn_point_number].position);
            }
        }
        else
        {
            if (Random.Range(0, 100) < 20)
            {
                GameFactoryManager.Instance.EnemyFact.InstantiateZombie(m_SpawnPoint[spawn_point_number].position);
            }
            else if (Random.Range(0, 100) < 40)
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
        }
    }
    protected override IEnumerator GameStarting() //The game starts, showing the UI prompt
    {
        m_numberOfZombies += Random.Range(1, 3);
        m_numberOfWaves++;
        if(m_numberOfWaves%5 == 0)
        {
            for (int i = 0; i < playerStats.Length; i++)
            {
                playerStats[i].ChangeMaximumPropsNumber(1);
            }
        }
        endlessModePlayerUI.ChangeGameMessage("Game Start!" + "\n\n\n " +"Wave: " + m_numberOfWaves + "\n\n\n " + m_numberOfZombies + "  Zombies are coming!");
        return base.GameStarting();
    }
    protected override IEnumerator ZombieSpawning() //Start spawning zombies, zombies will appear every corresponding time interval
    {
        endlessModePlayerUI.ClearGmaeMessage();
        return base.ZombieSpawning();
    }
    protected override IEnumerator BeforeEnding() //Give player 5 seconds to pick up props
    {
        if (!AllPlayerDead())
        {
            int counter = 5;
            endlessModePlayerUI.ClearGmaeMessage();
            while (counter > 0)
            {
                endlessModePlayerUI.ChangeGameMessage("The next wave of zombies will arrive in: " + "\n\n\n" + counter + " s!");
                counter -= 1;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    protected override IEnumerator GameEnding() //Defeat all zombies and the game is over
    {
        endlessModePlayerUI.ChangeGameMessage("YOU LOSE!" + "\n\n\n" + "Your final score: " + m_score);
        return GameEnding();
    }

    private void UpdateScore()
    {
        endlessModePlayerUI.ChangeScore(m_score);
    }
    public void onDeadAddScore(int score)
    {
        m_score += score;
    }
    public void AddScore(int amount)
    {
        m_score += amount;
    }
    public Vector3 BestPositonForAI()
    {

        Vector3 bestPostion = CurrentPositionForAI();

        //Follow player
        if (Vector3.Distance(m_PlayerInstance[0].transform.position, CurrentPositionForAI()) > 5.0f)
        {
            bestPostion = m_PlayerInstance[0].transform.position;
        }

        //check if there is a props
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        if (props.Length > 0)
        {
            float min_distance = Vector3.Distance(props[0].transform.position, m_PlayerInstance[1].transform.position);
            int props_index = 0;
            for (int i = 1; i < props.Length; i++)
            {
                if (Vector3.Distance(props[i].transform.position, m_PlayerInstance[1].transform.position) < min_distance)
                {
                    min_distance = Vector3.Distance(props[i].transform.position, m_PlayerInstance[1].transform.position);
                    props_index = i;
                }
            }
            bestPostion = props[props_index].transform.position;
        }

        //if only equip handgun check if there is a weapon
        if (playerStats[1].IsSingleWeapon())
        {
            GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
            if (weapon.Length > 0)
            {
                float min_distance = Vector3.Distance(weapon[0].transform.position, m_PlayerInstance[1].transform.position);
                int weapon_index = 0;
                for (int i = 1; i < weapon.Length; i++)
                {
                    if (Vector3.Distance(weapon[i].transform.position, m_PlayerInstance[1].transform.position) < min_distance)
                    {
                        min_distance = Vector3.Distance(weapon[i].transform.position, m_PlayerInstance[1].transform.position);
                        weapon_index = i;
                    }
                }
                bestPostion = weapon[weapon_index].transform.position;
            }
        }

        //Escape when very close to zombie
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        if (zombies.Length > 0)
        {
            int zombiesOnTop = 0;
            int zombiesOnBottom = 0;
            int zombiesOnLeft = 0;
            int zombiesOnRight = 0;
            int zombiesInRange = 0;
            Vector3 movePosition = CurrentPositionForAI();
            for (int i = 0; i < zombies.Length; i++)
            {
                if (Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI()) < 5)
                {
                    zombiesInRange++;
                    if ((zombies[i].transform.position - CurrentPositionForAI()).x > 0)
                    {
                        zombiesOnRight++;
                    }
                    if ((zombies[i].transform.position - CurrentPositionForAI()).x < 0)
                    {
                        zombiesOnLeft++;
                    }
                    if ((zombies[i].transform.position - CurrentPositionForAI()).z < 0)
                    {
                        zombiesOnBottom++;
                    }
                    if ((zombies[i].transform.position - CurrentPositionForAI()).z > 0)
                    {
                        zombiesOnTop++;
                    }
                }
            }
            if (zombiesOnTop > zombiesOnBottom)
            {
                movePosition += new Vector3(0, 0, -1);
            }
            else
            {
                movePosition += new Vector3(0, 0, 1);
            }
            if (zombiesOnLeft > zombiesOnRight)
            {

                movePosition += new Vector3(1, 0, 0);
            }
            else
            {
                movePosition += new Vector3(-1, 0, 0);
            }
            if (zombiesInRange > 0)
            {
                bestPostion = movePosition;
            }
        }
        return bestPostion;
    }
    public Vector3 CurrentPositionForAI()
    {
        return m_PlayerInstance[1].transform.position;
    }
    public bool AI_EnemyInAttackRange()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i< zombies.Length; i++)
        {
            if(Vector3.Distance(zombies[i].transform.position,m_PlayerInstance[1].transform.position) <= 8)
            {
                /*RaycastHit hitInfo;
                if (Physics.Raycast(CurrentPositionForAI(), zombies[i].transform.position - CurrentPositionForAI(), out hitInfo, 8))
                {
                    if (hitInfo.collider.gameObject.tag != "Environment")
                    {
                        return true;
                    }
                }*/

                return true;
            }
        }
        return false;
    }
    public bool AI_AmmoNotFull()
    {
        if (playerStats[1].CurrentCartridgeCap < playerStats[1].CurrentCartridgeCapacity)
        {
            if (playerStats[1].CurrentRestAmmo > 0)
            {
                if (AllZombieDead())
                {
                    return true;
                }
                if (playerStats[1].CurrentCartridgeCap <= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool AI_SwitchWeapon()
    {
        return playerStats[1].AI_TimeToSwitchWeapon();
    }
    public bool AI_WeaponNearBy()
    {
        GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i< weapon.Length; i++)
        {
            if(Vector3.Distance(weapon[i].transform.position, m_PlayerInstance[1].transform.position) < 1.5f)
            {
                return true;
            }
        }
        return false;
    }
    public Vector2 AIMoveInput()
    {

        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);
        Vector2 upleft = new Vector2(-0.7f, 0.7f);
        Vector2 upright = new Vector2(0.7f, 0.7f);
        Vector2 downleft = new Vector2(-0.7f, -0.7f);
        Vector2 downright = new Vector2(0.7f, -0.7f);
        Vector2 still = new Vector2(0, 0);

        Vector3 dir = BestPositonForAI() - CurrentPositionForAI();
        RaycastHit hitInfo;
        if (Physics.Raycast(CurrentPositionForAI(), dir, out hitInfo, 1))
        {            
            if (hitInfo.transform.gameObject.tag == "Environment")
            {
                Debug.Log(1);
                if (Physics.Raycast(CurrentPositionForAI(),new Vector3(1, 0, 0), 1)) //wall on right
                {
                    return up;
                }
                if (Physics.Raycast(CurrentPositionForAI(), new Vector3(-1, 0, 0), 1)) //wall on left
                {
                    return down;
                }
                if (Physics.Raycast(CurrentPositionForAI(), new Vector3(0, 0, 1), 1)) //wall on top
                {
                    return left;
                }
                if (Physics.Raycast(CurrentPositionForAI(), new Vector3(0, 0, -1), 1)) //wall on botton
                {
                    return right;
                }
            }
        }
        if (dir.magnitude < 0.2f)
        {
            return still;
        }
        else
        {
            if (dir.x > 0.1)//right
            {
                if (dir.z > 0.1)
                {
                    return upright;
                }
                else if (dir.z < -0.1)
                {
                    return downright;
                }
                else
                {
                    return right;
                }
            }
            else if (dir.x < -0.1)//left
            {
                if (dir.z > 0.1)
                {
                    return upleft;
                }
                else if (dir.z < -0.1)
                {
                    return downleft;
                }
                else
                {
                    return left;
                }
            }
            else //up or down
            {
                if (dir.z > 0.1)
                {
                    return up;
                }
                else if (dir.z < -0.1)
                {
                    return down;
                }
                else
                {
                    return still;
                }
            }
        }
    }
}
