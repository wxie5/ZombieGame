using UnityEngine;

[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerAI : MonoBehaviour
{
    private EndlessModeManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 BestPositonForAI()
    {

        Vector3 bestPostion = CurrentPositionForAI();

        //Follow player
        if (Vector3.Distance(gameManager.PlayerInstance[0].transform.position, CurrentPositionForAI()) > 5.0f)
        {
            bestPostion = gameManager.PlayerInstance[0].transform.position;
        }

        //check if there is a props
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        if (props.Length > 0)
        {
            float min_distance = Vector3.Distance(props[0].transform.position, gameManager.PlayerInstance[1].transform.position);
            int props_index = 0;
            for (int i = 1; i < props.Length; i++)
            {
                if (Vector3.Distance(props[i].transform.position, gameManager.PlayerInstance[1].transform.position) < min_distance)
                {
                    min_distance = Vector3.Distance(props[i].transform.position, gameManager.PlayerInstance[1].transform.position);
                    props_index = i;
                }
            }
            bestPostion = props[props_index].transform.position;
        }

        //if only equip handgun check if there is a weapon
        if (gameManager.PlayerInstance[1].GetComponent<PlayerStats>().IsSingleWeapon())
        {
            GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
            if (weapon.Length > 0)
            {
                float min_distance = Vector3.Distance(weapon[0].transform.position, gameManager.PlayerInstance[1].transform.position);
                int weapon_index = 0;
                for (int i = 1; i < weapon.Length; i++)
                {
                    if (Vector3.Distance(weapon[i].transform.position, gameManager.PlayerInstance[1].transform.position) < min_distance)
                    {
                        min_distance = Vector3.Distance(weapon[i].transform.position, gameManager.PlayerInstance[1].transform.position);
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
        return gameManager.PlayerInstance[1].transform.position;
    }
    public bool AI_EnemyInAttackRange()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < zombies.Length; i++)
        {
            if (Vector3.Distance(zombies[i].transform.position, gameManager.PlayerInstance[1].transform.position) <= 8)
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
        if (gameManager.PlayerInstance[1].GetComponent<PlayerStats>().CurrentCartridgeCap < gameManager.PlayerInstance[1].GetComponent<PlayerStats>().CurrentCartridgeCapacity)
        {
            if (gameManager.PlayerInstance[1].GetComponent<PlayerStats>().CurrentRestAmmo > 0)
            {
                if (gameManager.AllZombieDead())
                {
                    return true;
                }
                if (gameManager.PlayerInstance[1].GetComponent<PlayerStats>().CurrentCartridgeCap <= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool AI_SwitchWeapon()
    {
        return gameManager.PlayerInstance[1].GetComponent<PlayerStats>().AI_TimeToSwitchWeapon();
    }
    public bool AI_WeaponNearBy()
    {
        GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < weapon.Length; i++)
        {
            if (Vector3.Distance(weapon[i].transform.position, gameManager.PlayerInstance[1].GetComponent<PlayerStats>().transform.position) < 1.5f)
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
                if (Physics.Raycast(CurrentPositionForAI(), new Vector3(1, 0, 0), 1)) //wall on right
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
