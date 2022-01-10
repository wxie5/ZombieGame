using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    //for simplicity, I direcly reduce maxHealth for this prototype
    //in the real project, we need to set an additional "currentHealth" variable
    [SerializeField] private float maxHealth;

    //Player Transform
    private Transform playerTrans;

    //Components
    private NavMeshAgent agent;
    private Animator animator;

    //Enemy Properties
    private bool hasTarget = false;
    private bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
    }

    private void Start()
    {
        playerTrans = GameObject.FindWithTag("Player").transform;

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
        if (playerTrans == null) { return; }

        //set target, start chasing
        if (!hasTarget)
        {
            hasTarget = true;
            animator.SetBool("HasTarget", hasTarget);
        }
        else
        {
            agent.SetDestination(playerTrans.position);
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
