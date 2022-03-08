using UnityEngine;
using UnityEngine.AI;
using Model.EnemyModel;
using System.Collections.Generic;
using System;

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
        protected Transform[] playerTrans;
        protected EnemyBaseModel model;

        protected EndlessModeManager endlessModeManager;

        // AI state
        protected bool isDead;

        // finite state machine
        protected Dictionary<Type, AbstractState> states;
        protected AbstractState curState;

        // timer
        protected float attackTimer;

        // Local UI
        protected HPBar hpbar;

        // view delegate
        public delegate void ProjectileEvent(Vector3 vec, Quaternion dir, float dmg);
        public delegate void VFXEvent(Vector3 vec);
        public delegate void PositionEvent(Vector3 vec);

        public event PositionEvent OnDead;

        #region Attributes
        public bool IsDead
        {
            get { return isDead; }
        }
        public Transform Target
        {
            get { return target; }
        }
        public NavMeshAgent Agent
        {
            get { return agent; }
        }
        public EnemyBaseModel Model
        {
            get { return model; }
        }
        public float AttackTimer
        {
            get { return attackTimer; }
            set { attackTimer = value; }
        }
        public Animator Anim
        {
            get { return animator; }
        }
        #endregion

        public virtual void SetUp(EnemyBaseModel m)
        {
            if (GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>())
            {
                endlessModeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EndlessModeManager>();
            }

            model = m;

            agent.speed = model.CurMoveSpeed;
            agent.stoppingDistance = model.StopDistance;
            playerTrans = GetAllPlayerTrans();
            attackTimer = model.AttackRateDefault;

            StateMachineInit();
        }

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            curGetPlayerTimer = 0f;
            isDead = false;
            hpbar = GetComponent<HPBar>();
        }

        protected virtual void Update()
        {
            if (!agent.enabled) { return; }

            Type newStateType = curState.StateUpdate();

            if (newStateType != curState.GetType())
            {
                curState = states[newStateType];
            }

            if (curGetPlayerTimer < 0f)
            {
                GetNearestPlayer();
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

        protected virtual void StateMachineInit()
        {
            states = new Dictionary<Type, AbstractState>();
            states.Add(typeof(EnemyIdleState), new EnemyIdleState(this));
            states.Add(typeof(EnemyChaseState), new EnemyChaseState(this));
            states.Add(typeof(EnemyAttackState), new EnemyAttackState(this));

            curState = states[typeof(EnemyIdleState)];
        }

        public virtual void AttackView()
        {
        }

        public virtual void GetHitView(float damage)
        {
            float remainHealth = model.CurHealth - damage;
            if (remainHealth > 0f)
            {
                model.CurHealth = remainHealth;
                HitView();
            }
            else
            {
                model.CurHealth = 0f;
                model.IsDead = true;
                DieView();
            }
        }
        private void HitView()
        {
            //choose a random hit animation
            int randomIdx = UnityEngine.Random.Range(0, 3);
            animator.SetFloat("HitIdx", randomIdx);

            //play hit animation
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");

            //reset velocity, change chase speed
            agent.velocity = Vector3.zero;
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

        // Deal damage to single target
        public void DealDamageSingle()
        {
            if (target == null) { return; }

            //recheck distance, if in range, deal damage to target player
            if (InAttackRange())
            {
                target.GetComponent<PlayerManager>().GetHit(model.CurDmg);
            }
        }

        // Deal damage to all players in range
        public void DealDamageAll()
        {
            if (playerTrans.Length != 0)
            {
                foreach (Transform trans in playerTrans)
                {
                    if (trans == null) { continue; }
                    if (Vector3.Distance(trans.position, transform.position) < model.AttackRange)
                    {
                        trans.GetComponent<PlayerManager>().GetHit(model.CurDmg);
                    }
                }
            }
        }

        public bool InAttackRange()
        {
            if (target == null) { return false; }
            return Vector3.Distance(target.position, transform.position) < model.AttackRange;
        }

        public bool InAlertRange()
        {
            if (target == null) { return false; }
            return Vector3.Distance(target.position, transform.position) < model.AlertDistance;
        }

        public bool InStopDisRange()
        {
            if (target == null) { return false; }
            // here I add a small offset, so it can change to attack state
            return Vector3.Distance(target.position, transform.position) < (model.StopDistance + 0.5f);
        }

        private Transform[] GetAllPlayerTrans()
        {
            //get all the players' transform in the scene
            GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
            Transform[] playerTrans = new Transform[playersGO.Length];
            for (int i = 0; i < playerTrans.Length; i++)
            {
                playerTrans[i] = playersGO[i].transform;
            }

            return playerTrans;
        }

        public void GetNearestPlayer()
        {
            int nearestIdx = -1;
            float curMinSqrMag = Mathf.Infinity;

            // if playeTrans is not set, get all players' transforms
            if (this.playerTrans == null)
            {
                this.playerTrans = GetAllPlayerTrans();
            }

            // find nearest enemy by using loop (compare square mag instead of distance for optimization)
            for (int i = 0; i < this.playerTrans.Length; i++)
            {
                // if player is dead or player is destroyed, skip this iteration
                if (this.playerTrans[i].tag == "Dead" || this.playerTrans[i] == null)
                {
                    continue;
                }

                // otherwise, update the nearest target we found
                float sqrMag = (this.playerTrans[i].position - transform.position).sqrMagnitude;
                if (sqrMag < curMinSqrMag)
                {
                    curMinSqrMag = sqrMag;
                    nearestIdx = i;
                }
            }

            // if found nearest target, set to target, otherwise to null
            if (nearestIdx != -1)
            {
                target = this.playerTrans[nearestIdx];
            }
            else
            {
                target = null;
            }
        }

        protected virtual void OnDestroy()
        {
            // unsubscribe model events
            model.OnCurHealthChange -= HPBarChange;
            model.OnDead -= HPBarHide;
            if (endlessModeManager != null)
            {
                model.OnDead -= endlessModeManager.onDeadAddScore;
            }
            model.OnDead -= Factory.GameFactoryManager.Instance.EnemyFact.OndeadHandler;

            // let GC clean up all the things that are not monobehaviour
            model = null;
        }
    }
}
