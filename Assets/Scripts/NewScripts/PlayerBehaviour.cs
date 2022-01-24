using System;
using UnityEngine;
using Utils.MathTool;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PlayerID playerNum = PlayerID.PlayerA;
    [SerializeField] private float playerRotSmoothTime = 0.02f;
    [Range(9.81f, 20f)]
    [SerializeField] private float posGravity = 9.81f;

    [Header("Combat Properties")]
    [SerializeField] private Transform shotTrans;

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

    //Components
    private CharacterController cc;
    private Animator animator;
    private Transform target;
    private ParticleSystem shotParticle;
    private Collider[] enemies;

    //Movement Variables
    private Vector3 gVelocity;
    private Vector2 inputAxis;
    private float currentSpeed;
    private bool canFreeRotMove = true;
    private float speed;

    //Smooth Variables
    private float playerRotSmoothRef;
    private float animationDampTime = 0.2f;
    private float animationDampSpeed = 5f;

    private bool isLockingTarget = false;

    //for check where we are compare to target
    private bool onTargetRight;
    private bool onTargetFront;

    //timer
    private float autoLoseTargetDelayTimer = 0f;
    private float shotRateTimer = 0f;

    //keep track of stats
    private PlayerStats stats;

    public void Initialize()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        GameObject shotParticleGO = Instantiate(shotParticlePrefab, shotTrans.position, shotTrans.rotation, shotTrans);
        shotParticle = shotParticleGO.GetComponent<ParticleSystem>();
        stats = GetComponent<PlayerStats>();

        //since OverlapSphere is expensive, we do not call it in Update
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    public void PlayerMoveSystem(Vector2 axis)
    {
        inputAxis = axis;
        //rotate player
        RotatePlayerWithAxis();

        //calculate the magnitude of inputAxis
        CalculateCurrentSpeed();

        //move player based on the inputAxis
        PlayerPositionMovement();

        //set animation parameter;
        SetMovementAnim();

        //gravity simulation
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (cc.isGrounded && gVelocity.y < 0f)
        {
            gVelocity.y = -1f;
        }
        else
        {
            gVelocity.y -= posGravity * Time.deltaTime;
            cc.Move(gVelocity * Time.deltaTime);
        }
    }

    private void RotatePlayerWithAxis()
    {
        if (!canFreeRotMove)
        {
            return;
        }

        if (inputAxis.magnitude != 0)
        {
            float targetRot = Mathf.Atan2(inputAxis.x, inputAxis.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref playerRotSmoothRef, playerRotSmoothTime);
        }
    }

    private void CalculateCurrentSpeed()
    {
        currentSpeed = speed * inputAxis.magnitude;
    }

    private void PlayerPositionMovement()
    {
        Vector3 movementVector = Vector3.zero;
        movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
        cc.Move(movementVector * currentSpeed * Time.deltaTime);
    }

    private void SetMovementAnim()
    {
        if (canFreeRotMove)
        {
            animator.SetFloat("Speed", currentSpeed, animationDampTime, Time.deltaTime * animationDampSpeed);
        }
        else
        {
            //find the dominant axis (vertical or horizontal)
            //then calculate the movement animation
            if (Mathf.Abs(inputAxis.x) > Mathf.Abs(inputAxis.y))
            {
                float snappedHori = MathTool.NormalizedFloat(inputAxis.x);
                if (onTargetFront)
                {
                    animator.SetFloat("Speed", -snappedHori);
                }
                else
                {
                    animator.SetFloat("Speed", snappedHori);
                }
            }
            else
            {
                float snappedVerti = MathTool.NormalizedFloat(inputAxis.y);
                if (onTargetRight)
                {
                    animator.SetFloat("Speed", -snappedVerti);
                }
                else
                {
                    animator.SetFloat("Speed", snappedVerti);
                }
            }

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

    public void PlayerCombatSystem(bool hitShotButton)
    {
        if (hitShotButton)
        {
            Attack();
        }
        AutoUnlockTarget();
        RotateToTarget();
        TimerAddition();
    }

    private void Attack()
    {
        if (shotRateTimer >= stats.CurrentShotRate)
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

            animator.ResetTrigger("Shot");
            animator.SetTrigger("Shot");
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
        float xOffset = UnityEngine.Random.Range(-stats.CurrentShotOffset.x, stats.CurrentShotOffset.x);
        float yOffset = UnityEngine.Random.Range(-stats.CurrentShotOffset.y, stats.CurrentShotOffset.y);
        shotDir = Quaternion.Euler(xOffset, yOffset, 0f) * shotDir;

        Ray shotRay = new Ray(shotTrans.position, shotDir);
        Vector3 hitPos;
        RaycastHit hit;
        if (Physics.Raycast(shotRay, out hit, stats.CurrentShotRange, bulletHitableLayer))
        {
            hitPos = hit.point;

            //Attack Enmey Logic Here
            //Because bullet does not has speed, we immediately damage the enemy
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<EnemyManager>().GetHit(stats.CurrentDamage);

                //play some blood effect based on the normal direction of hit point
                GameObject bloodGO = Instantiate(bloodParticlePrefab, hit.point, Quaternion.Euler(hit.normal));
                Destroy(bloodGO, 3f);
            }
            else if (hit.collider.tag == "Environment")
            {
                //TODO: maybe add some hit wall effects
            }
        }
        else
        {
            //if nothing is hitable, we just shot as far as we can
            hitPos = shotTrans.position + shotDir * stats.CurrentShotRange;
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

        if (autoLoseTargetDelayTimer >= autoLoseTargetDelay)
        {
            ChangeLockState();
        }
        //autoLoseTargetDelayTimer = MathTool.TimerAddition(autoLoseTargetDelayTimer, autoLoseTargetDelay);
    }

    //lock state change
    private void ChangeLockState()
    {
        isLockingTarget = !isLockingTarget;

        canFreeRotMove = !isLockingTarget;
        animator.SetBool("IsLockTarget", isLockingTarget);

        if (isLockingTarget)
        {
            speed = stats.CurrentTargetingMoveSpeed;
        }
        else
        {
            speed = stats.CurrentMoveSpeed;
        }
    }

    //rotate to the target (smoothly rotate)
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

    //timer addition
    private void TimerAddition()
    {
        if (isLockingTarget)
        {
            autoLoseTargetDelayTimer = MathTool.TimerAddition(autoLoseTargetDelayTimer, autoLoseTargetDelay);
        }
        shotRateTimer = MathTool.TimerAddition(shotRateTimer, stats.CurrentShotRate);
    }

    //play the shot fire particle
    private void GenerateShotParticle()
    {
        if (shotParticle == null) { return; }

        shotParticle.Play();
    }

    //for debug only, show player's lock range in the scene (edit mode, not play mode)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }

}
