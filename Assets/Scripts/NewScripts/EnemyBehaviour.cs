using UnityEngine;
using UnityEngine.AI;
using Utils.MathTool;
using System;


public class EnemyBehaviour : MonoBehaviour
{
    //for simplicity, I direcly reduce maxHealth for this prototype
    //in the real project, we need to set an additional "currentHealth" variable
    [SerializeField] private float maxHealth;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float attackRange = 2f;

    [SerializeField] private float[] chaseSpeeds;

    public int m_score = 10;
    public float m_attack_damage = 20;

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

    //events
    public Action onDead;

    public bool IsDead
    {
        get { return isDead; }
    }

    private void Start()
    {
        GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
        playersTrans = new Transform[playersGO.Length];
        for (int i = 0; i < playersTrans.Length; i++)
        {
            playersTrans[i] = playersGO[i].transform;
        }

        //get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //currently only called once, later I will change it
        GetNearestPlayer();
        ChangeChaseSpeed();
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
        }

        //has target, chasing
        animator.SetFloat("ChaseSpeed", agent.velocity.magnitude);

        if (DistanceToPlayer() < attackRange && attackRateTimer >= attackRate)
        {
            //attack
            animator.SetTrigger("Attack");

            //print("Attack");
            //target.GetComponent<SimpleCombat>().TakeDamage(m_attack_damage);

            attackRateTimer = 0f;
        }

        attackRateTimer = MathTool.TimerAddition(attackRateTimer, attackRate);
        agent.SetDestination(target.position);
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
        }
        else
        {
            target = null;
        }
    }

    /// <summary>
    /// Let the current enemy take some damage
    /// this method is currently messy, I will make it looks better later
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
        if ((maxHealth - dmg) <= 0f)
        {
            Die();
        }
        else
        {
            maxHealth -= dmg;

            //choose a random hit animation
            int randomIdx = UnityEngine.Random.Range(0, 3);
            animator.SetFloat("HitIdx", randomIdx);

            //play hit animation
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");

            //reset velocity, change chase speed
            agent.velocity = Vector3.zero;
            ChangeChaseSpeed();
        }
    }

    private void Die()
    {
        //disable nav mesh agent
        agent.enabled = false;

        maxHealth = 0f;
        isDead = true;

        //set the tag and layer to let player ignore dead enemy
        //also, set rigidbody to kinematic, otherwise it will have some strange rotation (because root motion)
        gameObject.tag = "Dead";
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        GetComponent<Rigidbody>().isKinematic = true;

        //trigger death animation, apply root motion for realistic locomotion
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");
        animator.SetBool("IsDead", isDead);

        //trigger on dead event
        onDead.Invoke();
    }

    private void ChangeChaseSpeed()
    {
        int randomIdx = UnityEngine.Random.Range(0, chaseSpeeds.Length);
        agent.speed = chaseSpeeds[randomIdx];
    }
}
