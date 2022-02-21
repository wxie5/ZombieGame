using Controller.EnemyController;
using UnityEngine;

namespace View.EnemyView
{
    public class EnemyZombieView : EnemyBaseView
    {
        private Transform target;

        private EnemyBaseController<EnemyZombieView> controller;
        public void SetUp(EnemyBaseController<EnemyZombieView> cc)
        {
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

        private void Update()
        {
            if (!agent.enabled) { return; }

            switch(curState)
            {
                case EnemyState.Attack:
                    controller.AttackLogic(transform.position, target, false);
                    break;
                case EnemyState.Chase:
                    controller.ChaseLogic(transform.position, target, true);
                    break;
                case EnemyState.Idle:
                    controller.IdleLogic(transform.position, target);
                    break;
            }

            if(target != null)
            {
                agent.SetDestination(target.position);
                animator.SetFloat("ChaseSpeed", agent.velocity.magnitude);
            }

            if(curGetPlayerTimer < 0f)
            {
                controller.GetNearestPlayer(transform.position, out target);
                curGetPlayerTimer = getPlayerFreq;
            }
            curGetPlayerTimer -= Time.deltaTime;

        }

        private void AttackView()
        {
            animator.SetTrigger("Attack");
        }

        private void ChangeToIdle()
        {
            agent.isStopped = true;
            curState = EnemyState.Idle;
        }

        private void ChangeToAttack()
        {
            agent.isStopped = false;
            curState = EnemyState.Attack;
        }

        private void ChangeToChase()
        {
            agent.isStopped = false;
            curState = EnemyState.Chase;
        }

        public override void GetHitView(float damage)
        {
            isDead = controller.GetHitLogic(damage);

            Debug.Log("Take Damage: " + damage);

            if (isDead)
            {
                DieView();
            }
            else
            {
                HitView();
            }
        }

        private void DieView()
        {
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
        }

        private void HitView()
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

        #region Animation Events
        public void OnAnimationAttackPoint()
        {
            controller.DealDamageLogic(transform.position, target);
        }
        #endregion
    }
}
