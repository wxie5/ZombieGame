using UnityEngine;
using UnityEngine.AI;
using Utils.MathTool;

public class EnemyBehaviour : MonoBehaviour
{
    //Player Transforms
    private Transform[] playersTrans;
    private Transform target;
    private PlayerID targetID;

    //Components
    private NavMeshAgent agent;
    private Animator animator;

    //Timer
    private float attackRateTimer = 0f;

    //keep track of enemy stats (only read from stats, stats never read from behaviour)
    private EnemyStats stats;

    public void Initialize()
    {
        //get all the players' transform in the scene
        GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
        playersTrans = new Transform[playersGO.Length];
        for (int i = 0; i < playersTrans.Length; i++)
        {
            playersTrans[i] = playersGO[i].transform;
        }

        //get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();

        //set the target
        GetNearestPlayer();

        //initialize agent speed
        agent.speed = stats.GetRandomChaseSpeed();
    }

    public void Attack()
    {
        if(target == null) { return; }

        if (DistanceToPlayer() < stats.AttackRange && attackRateTimer >= stats.AttackRate)
        {
            //attack
            animator.SetTrigger("Attack");

            //print("Attack");
            //target.GetComponent<SimpleCombat>().TakeDamage(m_attack_damage);

            attackRateTimer = 0f;
        }

        attackRateTimer = MathTool.TimerAddition(attackRateTimer, stats.AttackRate);
    }

    public void IdleOrChase()
    {
        if(target == null)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.SetDestination(target.position);
        }
        animator.SetFloat("ChaseSpeed", agent.velocity.magnitude);
    }

    private float DistanceToPlayer()
    {
        if (target == null)
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
        for (int i = 0; i < playersTrans.Length; i++)
        {
            float sqrMag = (playersTrans[i].position - transform.position).sqrMagnitude;
            if (sqrMag < curMinSqrMag)
            {
                curMinSqrMag = sqrMag;
                nearestIdx = i;
            }
        }

        if (nearestIdx != -1)
        {
            target = playersTrans[nearestIdx];
            targetID = target.GetComponent<PlayerStats>().ID;
        }
        else
        {
            target = null;
            targetID = PlayerID.None;
        }
    }

    public void Die()
    {
        //disable nav mesh agent
        agent.enabled = false;

        //set the tag and layer to let player ignore dead enemy
        //also, set rigidbody to kinematic, otherwise it will have some strange rotation (because root motion)
        gameObject.tag = "Dead";
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        GetComponent<Rigidbody>().isKinematic = true;

        //trigger death animation, apply root motion for realistic locomotion
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");
        animator.SetBool("IsDead", true);

        //trigger on dead event
        //onDead.Invoke();
    }

    public void GetHit()
    {
        //choose a random hit animation
        int randomIdx = UnityEngine.Random.Range(0, 3);
        animator.SetFloat("HitIdx", randomIdx);

        //play hit animation
        animator.ResetTrigger("GetHit");
        animator.SetTrigger("GetHit");

        //reset velocity, change chase speed
        agent.velocity = Vector3.zero;
        agent.speed = stats.GetRandomChaseSpeed();
    }
}
