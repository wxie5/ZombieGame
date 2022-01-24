using UnityEngine;
using System;
using Utils.MathTool;
using UnityEngine.UI;

namespace Prototype.Combat
{
    public class SimpleCombat : MonoBehaviour
    {
        [Header("Combat Properties")]
        [SerializeField] private float weaponAttackRange = 20f;
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private float shotRate = 0.2f;
        [SerializeField] private Transform shotTrans;
        [SerializeField] private Vector2 weaponOffset = new Vector2(20f, 10f);

        [Header("Target Lock Properties")]
        [SerializeField] private float lockRange = 10f;
        [SerializeField] private float autoLoseTargetDelay = 1f;

        [Header("Layer Mask: Select All That Apply")]
        [SerializeField] private LayerMask lockableLayer;
        [SerializeField] private LayerMask bulletHitableLayer;

        [Header("VFX")]
        [SerializeField] private GameObject shotParticlePrefab;
        [SerializeField] private GameObject bulletTrailPrefab;
        [SerializeField] private GameObject bloodParticlePrefab;

        [Header("Player Status")]
        public Image mask; //mask for HealthBar UI
        public float m_Max_HP = 100;
        private float m_current_HP;
        [HideInInspector] public bool is_dead = false;

        //Components
        private Transform target;
        private ParticleSystem shotParticle;
        private Collider[] enemies;

        //events
        public Action<bool> onLockStateChange;
        public Action onShot;

        private bool isLockingTarget = false;

        //for check where we are compare to target
        private bool onTargetRight;
        private bool onTargetFront;

        //timer
        private float autoLoseTargetDelayTimer = 0f;
        private float shotRateTimer = 0f;

        public bool OnTargetRight
        {
            get { return onTargetRight; }
        }

        public bool OnTargetFront
        {
            get { return onTargetFront; }
        }

        private void Start()
        {
            //since OverlapSphere is expensive, we do not call it in Update
            InvokeRepeating("UpdateTarget", 0f, 0.1f);

            GameObject shotParticleGO = Instantiate(shotParticlePrefab, shotTrans.position, shotTrans.rotation, shotTrans);
            shotParticle = shotParticleGO.GetComponent<ParticleSystem>();
            m_current_HP = m_Max_HP;
        }

        [Obsolete]
        private void Update()
        {
            Attack();
            AutoUnlockTarget();
            RotateToTarget();
            TimerAddition();
            if(m_current_HP <= 0)
            {
                is_dead = true;
            }
        }

        private void UpdateTarget()
        {
            enemies = Physics.OverlapSphere(transform.position, lockRange, lockableLayer);
            float curMinSqrMag = Mathf.Infinity;
            int currentNearestIdx = -1;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemies[i].GetComponent<EnemyManager>().IsAttackable()) { continue; }

                //I choose to use sqrtMag instead of Vector3.Distance
                //because we don't need to know the exact distance between them
                //take square root is a costly process
                float sqrMag = (enemies[i].transform.position - transform.position).sqrMagnitude;
                if (sqrMag < curMinSqrMag)
                {
                    curMinSqrMag = sqrMag;
                    currentNearestIdx = i;
                }
            }

            if (currentNearestIdx != -1)
            {
                target = enemies[currentNearestIdx].transform;
            }
            else
            {
                //if no target, set to null
                target = null;
            }
        }

        private void Attack()
        {
            //since UpdateTarget is not called in update, sometimes we may get null reference
            // (for example, target enemy is dead, but we haven't update it into enemies array)
            // later it may cause more problem when the scripts become more complicated
            // the best solution is to keep track of the data of target, as soon as it is dead, we change target state
            
            //if (target == null) { return; }

            if (Input.GetKey(KeyCode.J) && shotRateTimer >= shotRate)
            {
                //print("Shoot!");
                if (!isLockingTarget)
                {
                    ChangeLockState();
                }

                Vector3 hitPos = Shot();
                GenerateBulletTrail(hitPos);
                GenerateShotParticle();

                //reset timer
                autoLoseTargetDelayTimer = 0f;
                shotRateTimer = 0f;

                onShot.Invoke();
            }

        }

        private Vector3 Shot()
        {
            Vector3 shotDir;

            if (target == null)
            {
                shotDir = shotTrans.forward;
            }
            else
            {
                shotDir = (target.position - transform.position).normalized;
            }

            //offset the shot direction
            float xOffset = UnityEngine.Random.Range(-weaponOffset.x, weaponOffset.x);
            float yOffset = UnityEngine.Random.Range(-weaponOffset.y, weaponOffset.y);
            shotDir = Quaternion.Euler(xOffset, yOffset, 0f) * shotDir;
            
            Ray shotRay = new Ray(shotTrans.position, shotDir);
            Vector3 hitPos;
            RaycastHit hit;
            if (Physics.Raycast(shotRay, out hit, weaponAttackRange, bulletHitableLayer))
            {
                hitPos = hit.point;

                //Attack Enmey Logic Here
                //Because bullet does not has speed, we immediately damage the enemy
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<SimpleAI>().TakeDamage(weaponDamage);

                    //play some blood effect based on the normal direction of hit point
                    GameObject bloodGO = Instantiate(bloodParticlePrefab, hit.point, Quaternion.Euler(hit.normal));
                    Destroy(bloodGO, 3f);
                }
                else if(hit.collider.tag == "Environment")
                {
                    //TODO: maybe add some hit wall effects
                }
            }
            else
            {
                //if nothing is hitable, we just shot as far as we can
                hitPos = shotTrans.position + shotDir * weaponAttackRange;
            }

            return hitPos;
        }

        //a simple glowing bullet trail by using trail renderer
        private void GenerateBulletTrail(Vector3 hitPos)
        {
            TrailRenderer bulletTrail = Instantiate(bulletTrailPrefab, shotTrans.position, shotTrans.rotation).GetComponent<TrailRenderer>();
            bulletTrail.AddPosition(shotTrans.position);
            bulletTrail.transform.position = hitPos;
        }

        //unlock target if player does not keep attacking
        private void AutoUnlockTarget()
        {
            if (!isLockingTarget) { return; }

            if(autoLoseTargetDelayTimer >= autoLoseTargetDelay)
            {
                ChangeLockState();
            }
            autoLoseTargetDelayTimer = MathTool.TimerAddition(autoLoseTargetDelayTimer, autoLoseTargetDelay);
        }

        //event: lock state change
        private void ChangeLockState()
        {
            isLockingTarget = !isLockingTarget;
            onLockStateChange.Invoke(isLockingTarget);
        }

        private void RotateToTarget()
        {
            if (target != null && isLockingTarget)
            {
                Vector3 dir = target.position - transform.position;
                dir = new Vector3(dir.x, 0f, dir.z).normalized;
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * 30f);

                //use dot product to get the player's direction compare to target
                //useful for set the animation
                onTargetRight = Vector3.Dot(dir, Vector3.left) > 0f ? true : false;
                onTargetFront = Vector3.Dot(dir, Vector3.back) > 0f ? true : false;
            }
        }

        private void TimerAddition()
        {
            autoLoseTargetDelayTimer = MathTool.TimerAddition(autoLoseTargetDelayTimer, autoLoseTargetDelay);
            shotRateTimer = MathTool.TimerAddition(shotRateTimer, shotRate);
        }

        //play the shot fire particle
        private void GenerateShotParticle()
        {
            if(shotParticle == null) { return; }

            shotParticle.Play();
        }

        //for debug only, show player's lock range in the scene (edit mode, not play mode)
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lockRange);
        }

        public void TakeDamage(float dmg)
        {
            m_current_HP -= dmg;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_current_HP);
        }
    }
}
