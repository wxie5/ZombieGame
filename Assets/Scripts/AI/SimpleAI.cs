using UnityEngine;
using UnityEngine.AI;
using Prototype.Tool;

public class SimpleAI : MonoBehaviour
{
    //for simplicity, I direcly reduce maxHealth for this prototype
    //in the real project, we need to set an additional "currentHealth" variable
    [SerializeField] private float maxHealth;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float attackRange = 2f;

    //Player Transforms
    private Transform[] playersTrans;

    private Transform target;

    //Components
    private NavMeshAgent agent;
    private Animator animator;

    //Enemy Properties
    private bool hasTarget = false;
    private bool isDead = false;

    //Timer
    private float attackRateTimer = 0f;

    public bool IsDead
    {
        get { return isDead; }
    }

    private void Start()
    {
        GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
        playersTrans = new Transform[playersGO.Length];
        for(int i = 0; i < playersTrans.Length; i++)
        {
            playersTrans[i] = playersGO[i].transform;
        }

        //currently only called once, later I will change it
        GetNearestPlayer();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    private void Update()
    {
        //if dead, do nothing
        if (isDead) { return; }

        //if no target, do nothing
        if (target == null) { return; }

        //set target, start chasing (for test purpose, we must have target)
        if (!hasTarget)
        {
            hasTarget = true;
            animator.SetBool("HasTarget", hasTarget);
        }

        if(DistanceToPlayer() < attackRange && attackRateTimer >= attackRate)
        {
            //attack
            animator.SetTrigger("Attack");

            print("Attack");

            attackRateTimer = 0f;
        }

        attackRateTimer = MathTool.TimerAddition(attackRateTimer, attackRate);
        agent.SetDestination(target.position);
    }

    private float DistanceToPlayer()
    {
        if(target == null)
        {
            return 0f;
        }
        else
        {
            return Vector3.Distance(transform.position, target.position);
        }
    }

    private void GetNearestPlayer()
    {
        int nearestIdx = -1;
        float curMinSqrMag = Mathf.Infinity;
        for(int i = 0; i < playersTrans.Length; i++)
        {
            float sqrMag = (playersTrans[i].position - transform.position).sqrMagnitude;
            if(sqrMag < curMinSqrMag)
            {
                curMinSqrMag = sqrMag;
                nearestIdx = i;
            }
        }

        if(nearestIdx != -1)
        {
            target = playersTrans[nearestIdx];
        }
        else
        {
            target = null;
        }
    }
    
    private void OnAnimatorMove()
    {
        //rewrite how root motion affect transform.position
        //because we disable nav mesh agent from updating position
        //hence we need to manually set the position

        Vector3 rootPos = animator.rootPosition;
        //ensure vertical position is consistent
        rootPos.y = agent.nextPosition.y;
        transform.position = rootPos;
        agent.nextPosition = transform.position;
        //offset / time to get the true speed of animator, and apply to navmeshAgent
        agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
    }

    /// <summary>
    /// Let the current enemy take some damage
    /// this method is currently messy, I will make it looks better later
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
        if((maxHealth - dmg) <= 0f)
        {
            maxHealth = 0f;
            isDead = true;

            //set the tag and layer to let player ignore dead enemy
            //also, set rigidbody to kinematic, otherwise it will have some strange rotation (because root motion)
            gameObject.tag = "Dead";
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            GetComponent<Rigidbody>().isKinematic = true;

            //trigger is for triggering death animation
            //bool is for ensuring we never go from death animation to GetHit
            //because both GetHit and Death are connected to "Anystate"
            animator.SetTrigger("Die");
            animator.SetBool("IsDead", isDead);
        }
        else
        {
            maxHealth -= dmg;

            //choose a random hit animation
            int randomIdx = Random.Range(0, 3);
            animator.SetFloat("HitIdx", randomIdx);

            //play hit animation
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");
        }
    }
}
