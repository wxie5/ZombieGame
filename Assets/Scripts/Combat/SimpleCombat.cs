using UnityEngine;
using System;

namespace Prototype.Combat
{
    public class SimpleCombat : MonoBehaviour
    {
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private LayerMask lockableLayer;
        [SerializeField] private float autoLoseTargetTime = 1f;

        public Action<bool> onLockStateChange;

        private Transform target;
        private Collider[] enemies;

        private bool isLockingTarget = false;
        private bool onTargetRight;
        private bool onTargetFront;

        private float autoLoseTargetTimer = 0f;

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
        }

        private void Update()
        {
            Attack();
            AutoUnlockTarget();
            RotateToTarget();
        }

        private void UpdateTarget()
        {
            enemies = Physics.OverlapSphere(transform.position, attackRange, lockableLayer);
            float shortestDis = Mathf.Infinity;
            int currentNearestIdx = -1;
            for (int i = 0; i < enemies.Length; i++)
            {
                float sqrMag = (enemies[i].transform.position - transform.position).sqrMagnitude;
                if (sqrMag < shortestDis)
                {
                    shortestDis = sqrMag;
                    currentNearestIdx = i;
                }
            }

            if (currentNearestIdx != -1)
            {
                target = enemies[currentNearestIdx].transform;
            }
        }

        private void Attack()
        {
            //since UpdateTarget is not called in update, sometimes we may get null reference
            // (for example, target enemy is dead, but we haven't update it into enemies array)
            // later it may cause more problem when the scripts become more complicated
            // the best solution is to keep track of the data of target, as soon as it is dead, we change target state
            if (target == null) { return; }

            if (Input.GetKeyDown(KeyCode.J) && enemies.Length > 0)
            {
                //print("Shoot!");
                if (!isLockingTarget)
                {
                    ChangeLockState();
                }
                autoLoseTargetTimer = 0f;
            }
        }

        private void AutoUnlockTarget()
        {
            if (!isLockingTarget) { return; }

            if(autoLoseTargetTimer > autoLoseTargetTime)
            {
                ChangeLockState();
            }
            autoLoseTargetTimer += Time.deltaTime;
        }

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
                transform.rotation = lookRot;

                onTargetRight = Vector3.Dot(dir, Vector3.left) > 0f ? true : false;
                onTargetFront = Vector3.Dot(dir, Vector3.back) > 0f ? true : false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
