using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;

    private NavMeshAgent agent;
    private bool hasTarget = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updatePosition = false;
        //agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTrans == null) { return; }

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
        Vector3 rootPos = animator.rootPosition;
        rootPos.y = agent.nextPosition.y;
        transform.position = rootPos;
        agent.nextPosition = transform.position;

        //offset / time to get the true speed of animator, and apply to navmeshAgent
        agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
    }
}
