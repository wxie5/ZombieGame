using UnityEngine;

[RequireComponent(typeof(PlayerStats), typeof(PlayerBehaviour))]
public class PlayerAI : MonoBehaviour
{
    private EndlessModeManager gameManager;

    private bool isEscaping = false;
    private float escapingTimer = 0f;
    private int attackrange = 8;
    private int alertRange = 4;
    private int escapeRange = 1;
    private Vector3 escapeTargetPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
    }
    public Vector3 BestPositonForAI() //priority from bottom to top
    {
        //keep escaping
        if (isEscaping)
        {
            if (escapingTimer > 0)
            {
                escapingTimer -= Time.deltaTime;
                return EscapePosition();
            }
            else
            {
                isEscaping = false;
                escapeTargetPosition = Vector3.zero;
            }
        }
        //Default Position
        Vector3 bestPostion = CurrentPositionForAI();

        //follow player1
        if (Vector3.Distance(CurrentPositionForAI(), gameManager.PlayerInstance[0].transform.position) > 5)
        {
            bestPostion = gameManager.PlayerInstance[0].transform.position;
        }

        //when no zombies Alive
        if (NumberOfEnemy(enemyPositionStatus.Alive) == 0)
        {
            //get props
            if (ClosestProp() != null)
            {
                bestPostion = ClosestProp().transform.position;
            }
            //get weapon
            if (ClosestWeapon() != null)
            {
                bestPostion = ClosestWeapon().transform.position;
            }

        }
        //There are zombies alive but no zombies in AlertRange
        else if (NumberOfEnemy(enemyPositionStatus.InAttackRange) == 0)
        {
            //Chase Zombie to attack
            bestPostion = ClosestEnemy().transform.position;
            //get props
            if (ClosestProp() != null)
            {
                bestPostion = ClosestProp().transform.position;
            }
            //get weapon
            if (ClosestWeapon() != null)
            {
                bestPostion = ClosestWeapon().transform.position;
            }
        }

        //There are zombies in AlertRange
        else
        {
            //when no zombies in EscapeRange
            if (NumberOfEnemy(enemyPositionStatus.InEscapeRange) == 0)
            {
                //Leave Zombie Away
                Vector3 movePosition = CurrentPositionForAI();
                if (NumberOfEnemy(enemyPositionStatus.Alert_Top) > NumberOfEnemy(enemyPositionStatus.Alert_Bottom))
                {
                    movePosition += new Vector3(0, 0, -1);
                }
                else
                {
                    movePosition += new Vector3(0, 0, 1);
                }
                if (NumberOfEnemy(enemyPositionStatus.Alert_Left) > NumberOfEnemy(enemyPositionStatus.Alert_Right))
                {

                    movePosition += new Vector3(1, 0, 0);
                }
                else
                {
                    movePosition += new Vector3(-1, 0, 0);
                }
                bestPostion = movePosition;
            }
            //There are zombies in EscapeRange
            else
            {
                Escape();
                bestPostion = EscapePosition();
            }
        }
        return bestPostion;
    }
    public Vector3 CurrentPositionForAI()
    {
        return this.transform.position;
    }
    public bool AI_EnemyInAttackRange()
    {
        if (isEscaping || NumberOfEnemy(enemyPositionStatus.Alive) == 0)
        {
            return false;
        }
        //shot if no building in the middle
        RaycastHit hitInfo;
        if (Physics.Raycast(CurrentPositionForAI() + new Vector3(0f, 0.8f, 0f), ClosestEnemy().transform.position - CurrentPositionForAI(), out hitInfo, 8))
        {
            if (hitInfo.collider.gameObject.tag != "Environment")
            {
                return true;
            }
        }
        return false;
    }
    public bool AI_AmmoNotFull()
    {
        if (this.GetComponent<PlayerStats>().CurrentCartridgeCap < this.GetComponent<PlayerStats>().CurrentCartridgeCapacity)
        {
            if (this.GetComponent<PlayerStats>().CurrentRestAmmo > 0)
            {
                if(escapingTimer<1f && isEscaping)
                {
                    return true;
                }
                if (gameManager.AllZombieDead())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool AI_WeaponNearBy()
    {
        GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < weapon.Length; i++)
        {
            if (Vector3.Distance(weapon[i].transform.position, this.GetComponent<PlayerStats>().transform.position) < 1.5f)
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

        Vector2 moveDir;

        float offset = 0.1f;
        if (dir.magnitude < offset)
        {
            return still;
        }
        else
        {
            if (dir.x > offset)//right
            {
                if (dir.z > offset)
                {
                    moveDir = upright;
                }
                else if (dir.z < -offset)
                {
                    moveDir = downright;
                }
                else
                {
                    moveDir = right;
                }
            }
            else if (dir.x < -offset)//left
            {
                if (dir.z > offset)
                {
                    moveDir = upleft;
                }
                else if (dir.z < -offset)
                {
                    moveDir = downleft;
                }
                else
                {
                    moveDir = left;
                }
            }
            else //up or down
            {
                if (dir.z > offset)
                {
                    moveDir = up;
                }
                else if (dir.z < -offset)
                {
                    moveDir = down;
                }
                else
                {
                    moveDir = still;
                }
            }
        }
        return moveDir;
    }
    public bool AI_TimeToSwitchWeapon()
    {
        if (this.GetComponent<PlayerStats>().IsSingleWeapon())
        {
            return false;
        }
        if (this.GetComponent<PlayerStats>().CurrentGunIndex != 0) //Not HandGun
        {
            if (this.GetComponent<PlayerStats>().CurrentCartridgeCap + this.GetComponent<PlayerStats>().CurrentRestAmmo == 0)
            {
                return true;
            }
        }
        else
        {
            if (this.GetComponent<PlayerStats>().CartridgeCaps[1] + this.GetComponent<PlayerStats>().AmmoCaps[1] > 0)
            {
                return true;
            }
        }
        return false;
    }
    private void Escape()
    {
        isEscaping = true;
        escapingTimer = 2;
        //Find a best position for escape
        //check if there is a 
    }
    private GameObject ClosestEnemy()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        int closestZombieIndex = 0;
        float minDistance = 999f;
        //find cloest zombie
        for (int i = 0; i < zombies.Length; i++)
        {
            if (Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI()) < minDistance)
            {
                minDistance = Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI());
                closestZombieIndex = i;
            }
        }
        return zombies[closestZombieIndex];
    }
    private GameObject ClosestProp()
    {
        GameObject[] props = GameObject.FindGameObjectsWithTag("Props");
        if (props.Length > 0)
        {
            float min_distance = Vector3.Distance(props[0].transform.position, this.transform.position);
            int props_index = 0;
            for (int i = 1; i < props.Length; i++)
            {
                if (Vector3.Distance(props[i].transform.position, this.transform.position) < min_distance)
                {
                    min_distance = Vector3.Distance(props[i].transform.position, this.transform.position);
                    props_index = i;
                }
            }
            return props[props_index];
        }
        //follow player1
        else
        {
            return null;
        }
    }
    private GameObject ClosestWeapon()
    {
        if (this.GetComponent<PlayerStats>().IsSingleWeapon())
        {
            GameObject[] weapon = GameObject.FindGameObjectsWithTag("Weapon");
            if (weapon.Length > 0)
            {
                float min_distance = Vector3.Distance(weapon[0].transform.position, this.transform.position);
                int weapon_index = 0;
                for (int i = 1; i < weapon.Length; i++)
                {
                    if (Vector3.Distance(weapon[i].transform.position, this.transform.position) < min_distance)
                    {
                        min_distance = Vector3.Distance(weapon[i].transform.position, this.transform.position);
                        weapon_index = i;
                    }
                }
                return weapon[weapon_index];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    private int NumberOfEnemy(enemyPositionStatus status)
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        int zombiesInAttackRange = 0;
        int zombiesInAlertRange = 0;
        int zombiesInEscapeRange = 0;
        int zombiesOnTop = 0;
        int zombiesOnBottom = 0;
        int zombiesOnLeft = 0;
        int zombiesOnRight = 0;
        for (int i = 0; i < zombies.Length; i++)
        {
            if (Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI()) < attackrange)
            {
                zombiesInAttackRange++;
                if (Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI()) < alertRange)
                {
                    zombiesInAlertRange++;
                    {
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
                        if (Vector3.Distance(zombies[i].transform.position, CurrentPositionForAI()) < escapeRange)
                        {
                            zombiesInEscapeRange++;
                        }
                    }
                }
            }
        }
        if (status == enemyPositionStatus.Alive)
        {
            return zombies.Length;
        }
        else if (status == enemyPositionStatus.InAttackRange)
        {
            return zombiesInAttackRange;
        }
        else if (status == enemyPositionStatus.InAlertRange)
        {
            return zombiesInAlertRange;
        }
        else if (status == enemyPositionStatus.Alert_Left)
        {
            return zombiesOnLeft;
        }
        else if (status == enemyPositionStatus.Alert_Right)
        {
            return zombiesOnRight;
        }
        else if (status == enemyPositionStatus.Alert_Top)
        {
            return zombiesOnTop;
        }
        else if (status == enemyPositionStatus.Alert_Bottom)
        {
            return zombiesOnBottom;
        }
        else
        {
            return zombiesInEscapeRange;
        }
    }
    private enum enemyPositionStatus
    {
        Alive,
        InAttackRange,
        InAlertRange,
        Alert_Left,
        Alert_Right,
        Alert_Top,
        Alert_Bottom,
        InEscapeRange
    }
    private Vector3 EscapePosition()
    {
        if(escapeTargetPosition != Vector3.zero)
        {
            escapeTargetPosition = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
        }
        return escapeTargetPosition - CurrentPositionForAI();
    }
}
