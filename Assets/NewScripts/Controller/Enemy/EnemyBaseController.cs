using UnityEngine;
using Model.EnemyModel;

namespace Controller.EnemyController
{
    public class EnemyBaseController<V> : IEnemyController<V>
    {
        protected V view;
        protected EnemyBaseModel model;

        protected Transform[] playerTrans;
        protected float attackTimer;

        public delegate void VoidEvent();
        public event VoidEvent OnStateChangeToIdle;
        public event VoidEvent OnStateChangeToChase;
        public event VoidEvent OnStateChangeToAttack;
        public event VoidEvent OnAttack;

        // this is only called for event unsubscription, view should never touch model unless unsubscribe events
        public EnemyBaseModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public EnemyBaseController(V view, EnemyBaseModel model)
        {
            this.view = view;
            this.model = model;

            this.playerTrans = GetAllPlayerTrans();
            this.attackTimer = model.AttackRateDefault;
        }
        
        public void ModelToViewInit(ref float agentSpeed, ref float stopDis)
        {
            agentSpeed = model.CurMoveSpeed;
            stopDis = model.StopDistance;
        }

        #region Main Logic
        public void AttackLogic(Vector3 curPos, Transform target, bool isBoom) 
        {
            if (!isBoom)
            {
                // Change to idle
                if (target == null)
                {
                    OnStateChangeToIdle?.Invoke();
                }

                // Attack behaviour
                if (InAttackRange(curPos, target) && attackTimer <= 0f)
                {
                    OnAttack?.Invoke();
                    attackTimer = model.CurAttackRate;
                }

                // Change to chase
                if (!InAttackRange(curPos, target))
                {
                    OnStateChangeToChase?.Invoke();
                }
            }
            else
            {
                if(attackTimer <= 0)
                {
                    OnAttack?.Invoke();
                    attackTimer = model.CurAttackRate;
                }
            }

            // timer --
            attackTimer -= Time.deltaTime;
        }

        public void IdleLogic(Vector3 curPos, Transform target) 
        {
            // if null, do nothing
            if(target == null) { return; }

            // Change to chase
            if (InAlertRange(curPos, target))
            {
                OnStateChangeToChase?.Invoke();
            }
        }

        public void ChaseLogic(Vector3 curPos, Transform target, bool isTimerDecre) 
        {
            // if null during chasing, back to idle
            if (target == null)
            {
                OnStateChangeToIdle?.Invoke();
            }

            // Change to attack
            if (InStopDisRange(curPos, target))
            {
                OnStateChangeToAttack?.Invoke();
            }

            if(isTimerDecre)
            {
                // timer --
                attackTimer -= Time.deltaTime;
            }
        }

        // Deal damage to single target
        public void DealDamageLogic(Vector3 curPos, Transform target)
        {
            if (target == null) { return; }

            //recheck distance, if in range, deal damage to target player
            if (InAttackRange(curPos, target))
            {
                target.GetComponent<PlayerManager>().GetHit(model.CurDmg);
            }
        }

        // Deal damage to all players in range (maybe damage enemy too, later)
        public void DealDamageLogic(Vector3 curPos)
        {
            if(playerTrans.Length != 0)
            {
                foreach(Transform trans in playerTrans)
                {
                    if(trans == null) { continue; }
                    if(Vector3.Distance(trans.position, curPos) < model.AttackRange)
                    {
                        trans.GetComponent<PlayerManager>().GetHit(model.CurDmg);
                    }
                }
            }
        }

        public float DealProjectileLogic()
        {
            return model.CurDmg;
        }

        public bool GetHitLogic(float damage)
        {
            float remainHealth = model.CurHealth - damage;
            if(remainHealth > 0f)
            {
                model.CurHealth = remainHealth;
                return false;
            }
            else
            {
                model.CurHealth = 0f;
                model.IsDead = true;
                return true;
            }
        }
        #endregion

        #region Small Helper Function
        private bool InAttackRange(Vector3 curPos, Transform target)
        {
            if(target == null) { return false; }
            return Vector3.Distance(target.position, curPos) < model.AttackRange;
        }

        private bool InAlertRange(Vector3 curPos, Transform target)
        {
            if (target == null) { return false; }
            return Vector3.Distance(target.position, curPos) < model.AlertDistance;
        }

        private bool InStopDisRange(Vector3 curPos, Transform target)
        {
            if (target == null) { return false; }
            // here I add a small offset, so it can change to attack state
            return Vector3.Distance(target.position, curPos) < (model.StopDistance + 0.5f);
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

        public void GetNearestPlayer(Vector3 curPos, out Transform target)
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
                float sqrMag = (this.playerTrans[i].position - curPos).sqrMagnitude;
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

        public void ResetCoolDown()
        {
            attackTimer = model.CurAttackRate;
        }
        #endregion

    }

    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }
}
