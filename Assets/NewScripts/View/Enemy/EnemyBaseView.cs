using UnityEngine;
using UnityEngine.AI;
using Controller.EnemyController;

namespace View.EnemyView
{
    public class EnemyBaseView : MonoBehaviour, IEnemyView
    {
        // Timer
        [SerializeField] protected float getPlayerFreq = 0.5f;
        protected float curGetPlayerTimer;

        // Components
        protected NavMeshAgent agent;
        protected Animator animator;
        protected Transform target;
        protected EnemyBaseController controller;

        protected EndlessModeManager endlessModeManager;

        // AI state
        protected EnemyState curState;
        protected bool isDead;

        // Local UI
        protected HPBar hpbar;

        // view delegate
        public delegate void ProjectileEvent(Vector3 vec, Quaternion dir, float dmg);
        public delegate void VFXEvent(Vector3 vec);
        public delegate void PositionEvent(Vector3 vec);

        public event PositionEvent OnDead;
        public bool IsDead
        {
            get { return isDead; }
        }

        public void SetUp(EnemyBaseController cc)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                endlessModeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
            }

            controller = cc;

            controller.OnAttack += AttackView;
            controller.OnStateChangeToAttack += ChangeToAttack;
            controller.OnStateChangeToChase += ChangeToChase;
            controller.OnStateChangeToIdle += ChangeToIdle;

            float agentSpeed = 0f;
            float stopDis = 0f;
            controller.ModelToViewInit(ref agentSpeed, ref stopDis);
            agent.speed = agentSpeed;
            agent.stoppingDistance = stopDis;
        }

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            curState = EnemyState.Idle;
            curGetPlayerTimer = 0f;
            isDead = false;
            hpbar = GetComponent<HPBar>();
        }

        protected virtual void Update()
        {
            if (!agent.enabled) { return; }

            switch (curState)
            {
                case EnemyState.Attack:
                    transform.LookAt(target, Vector3.up);
                    controller.AttackLogic(transform.position, target, false);
                    break;
                case EnemyState.Chase:
                    controller.ChaseLogic(transform.position, target, true);
                    break;
                case EnemyState.Idle:
                    controller.IdleLogic(transform.position, target);
                    break;
            }

            if (target != null)
            {
                agent.SetDestination(target.position);
                animator.SetFloat("ChaseSpeed", agent.velocity.magnitude);
            }

            if (curGetPlayerTimer < 0f)
            {
                controller.GetNearestPlayer(transform.position, out target);
                curGetPlayerTimer = getPlayerFreq;
            }
            curGetPlayerTimer -= Time.deltaTime;

        }

        public void HPBarChange(float curHealth, float maxHealth)
        {
            hpbar.SetFill(curHealth, maxHealth);
        }

        public void HPBarHide(int score)
        {
            hpbar.DisableHPBar();
        }

        protected virtual void AttackView()
        {
        }

        protected virtual void ChangeToIdle()
        {
            agent.isStopped = true;
            curState = EnemyState.Idle;
        }

        protected virtual void ChangeToAttack()
        {
            controller.ResetCoolDown();
            agent.isStopped = false;
            curState = EnemyState.Attack;
        }

        protected virtual void ChangeToChase()
        {
            agent.isStopped = false;
            curState = EnemyState.Chase;
        }

        protected virtual void HitView()
        {
            //choose a random hit animation
            int randomIdx = Random.Range(0, 3);
            animator.SetFloat("HitIdx", randomIdx);

            //play hit animation
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");

            //reset velocity, change chase speed
            agent.velocity = Vector3.zero;
        }

        public virtual void GetHitView(float damage)
        {
            isDead = controller.GetHitLogic(damage);

            if (isDead)
            {
                DieView();
            }
            else
            {
                HitView();
            }
        }

        protected virtual void DieView()
        {
            OnDead?.Invoke(transform.position);

            //disable nav mesh agent
            agent.enabled = false;

            //set the tag and layer to let player ignore dead enemy
            //also, set rigidbody to kinematic, otherwise it will have some strange rotation (because root motion)
            gameObject.tag = GameConst.DEAD_TAG;
            gameObject.layer = GameConst.IGNORE_RAYCAST;
            GetComponent<Rigidbody>().isKinematic = true;

            //trigger death animation, apply root motion for realistic locomotion
            animator.applyRootMotion = true;
            animator.SetTrigger("Die");
            animator.SetBool("IsDead", true);

            // Destroy gameobject after 3 secs
            Destroy(this.gameObject, 3f);
        }

        protected virtual void OnDestroy()
        {
            // unsubscribe model events
            controller.Model.OnCurHealthChange -= HPBarChange;
            controller.Model.OnDead -= HPBarHide;
            if (endlessModeManager != null)
            {
                controller.Model.OnDead -= endlessModeManager.onDeadAddScore;
            }
            controller.Model.OnDead -= Factory.GameFactoryManager.Instance.EnemyFact.OndeadHandler;

            // unsubscribe controller events
            controller.OnAttack -= AttackView;
            controller.OnStateChangeToAttack -= ChangeToAttack;
            controller.OnStateChangeToChase -= ChangeToChase;
            controller.OnStateChangeToIdle -= ChangeToIdle;

            // let GC clean up all the things that are not monobehaviour
            controller.Model = null;
            controller = null;
        }
    }
}
