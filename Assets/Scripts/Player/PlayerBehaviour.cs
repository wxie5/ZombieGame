using UnityEngine;
using Utils.MathTool;
using View.EnemyView;
using System;

//This script is create and wrote by Wei Xie
//Modified by Jiacheng Sun
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerRotSmoothTime = 0.02f;
    [Range(9.81f, 20f)]
    [SerializeField] private float posGravity = 9.81f;

    [Header("Combat Properties")]
    [SerializeField] private Transform shotTrans;
    [SerializeField] private Transform gunSlotTrans;

    [Header("Scene Interaction")]
    [SerializeField] private float pickUpRange = 1.5f;

    [Header("Target Lock Properties")]
    [SerializeField] private float lockRange = 10f;
    [SerializeField] private float autoLoseTargetDelay = 1f;

    [Header("Layer Mask: Select All That Apply")]
    [SerializeField] private LayerMask lockableLayer;
    [SerializeField] private LayerMask bulletHitableLayer;
    [SerializeField] private LayerMask weaponLayer;

    [Header("VFX")]
    [SerializeField] private GameObject shotParticlePrefab;
    [SerializeField] private GameObject bulletTrailPrefab;
    [SerializeField] private GameObject bloodParticlePrefab;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip handGunShot;
    [SerializeField] private AudioClip raffieShot;
    [SerializeField] private AudioClip switchWeapon;
    [SerializeField] private AudioClip reload;


    //Components
    private CharacterController cc;
    private Animator animator;
    private Transform target;
    private ParticleSystem shotParticle;
    private Collider[] enemies;

    //Movement Variables
    private Vector3 gVelocity;
    private Vector2 inputAxis;
    private float targetSpeed;
    private bool canFreeRotMove = true;
    private float currentSpeed;

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

    //keep track of stats (only read from stats, stats never read from behaviour)
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

        //Initialize Gun
        InitGun();

        //Initialize Speed
        currentSpeed = stats.CurrentMoveSpeed;
    }

    public void PlayerMoveSystem(Vector2 axis)
    {
        inputAxis = axis;
        //rotate player
        RotatePlayerWithAxis();

        //calculate the magnitude of inputAxis
        CalculateTargetSpeed();

        //move player based on the inputAxis
        PlayerPositionMovement();

        //set animation parameter;
        SetMovementAnim();

        //gravity simulation
        ApplyGravity();
    }

    public void PlayerShotSystem(bool hitShotButton)
    {
        TimerAddition();

        if (IsState(1, "Reload")) { return; }

        if (hitShotButton)
        {
            Attack();
        }
        AutoUnlockTarget();
        RotateToTarget();
    }

    public void PlayerWeaponSwitchSystem(bool hitSwitchButton)
    {
        if(IsState(1, "Reload")) { return; }

        if (hitSwitchButton)
        {
            SwitchGun();
        }
    }

    public void PlayerReloadSystem(bool hitReloadButton)
    {
        if(hitReloadButton)
        {
            Reload();
        }
    }

    public void PlayerPickUpSystem(bool hitPickUpBotton)
    {
        if(hitPickUpBotton)
        {
            PickGun();
        }
    }

    public void PlayerGetHit(float damage)
    {
        if (stats.IsDead) { return; }

        stats.TakeDamage(damage);

        if (stats.IsDead)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            //some get hit effects
        }
    }

    /// <summary>
    /// check if we are currently in a state
    /// </summary>
    /// <param name="layerID"></param>
    /// <param name="stateName"></param>
    /// <returns></returns>
    private bool IsState(int layerID, string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(layerID).IsName(stateName);
    }

    private void Reload()
    {
        if(stats.CurrentRestAmmo <= 0) { return; }

        AudioSource.PlayClipAtPoint(reload, gameObject.transform.position);
        animator.SetTrigger("Reload");

        stats.UpdateReloadData();
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

    private void CalculateTargetSpeed()
    {
        targetSpeed = currentSpeed * inputAxis.magnitude;
    }

    private void PlayerPositionMovement()
    {
        Vector3 movementVector = Vector3.zero;
        movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
        cc.Move(movementVector * targetSpeed * Time.deltaTime);
    }

    private void SetMovementAnim()
    {
        if (canFreeRotMove)
        {
            animator.SetFloat("Speed", targetSpeed, animationDampTime, Time.deltaTime * animationDampSpeed);
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
            if (enemies[i] == null || enemies[i].GetComponent<EnemyBaseView>().IsDead) { continue; }

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
        if (shotRateTimer >= stats.CurrentShotRate && stats.CurrentCartridgeCap > 0)
        {
            // lock on enemy if currently not
            if (!isLockingTarget)
            {
                ChangeLockState();
            }

            if (stats.CurrentGun.gunType == GunType.Handgun)
            {
                AudioSource.PlayClipAtPoint(handGunShot, gameObject.transform.position);
            }
            if (stats.CurrentGun.gunType == GunType.AssaultRifle)
            {
                AudioSource.PlayClipAtPoint(raffieShot, gameObject.transform.position);
            }

            // shot and generate shot VFX
            Vector3 hitPos = Shot();
            GenerateBulletTrail(hitPos);
            GenerateShotParticle();

            // reset timer
            autoLoseTargetDelayTimer = 0f;
            shotRateTimer = 0f;

            //animator setting
            animator.ResetTrigger("Shot");
            animator.SetTrigger("Shot");

            //update stats
            stats.ReduceAmmo(1);

            //auto reload
            if (stats.CurrentCartridgeCap <= 0)
            {
                Reload();
            }
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
                hit.collider.GetComponent<EnemyBaseView>().GetHitView(stats.CurrentDamage);

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
            currentSpeed = stats.CurrentTargetingMoveSpeed;
        }
        else
        {
            currentSpeed = stats.CurrentMoveSpeed;
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

    public void PickGun()
    {
        //check sphere to get the nearest gun gameobject, getcomponent to get guninfo
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickUpRange, weaponLayer);

        if(colliders.Length == 0) { return; }

        int nearestIdx = -1;
        float nearestSqrDis = Mathf.Infinity;
        for(int i = 0; i < colliders.Length; i++)
        {
            float newSqrDis = (colliders[i].transform.position - transform.position).sqrMagnitude;
            if (newSqrDis < nearestSqrDis)
            {
                nearestIdx = i;
                nearestSqrDis = newSqrDis;
            }
        }

        Gun pickedGunInfo = colliders[nearestIdx].GetComponent<GunItem>().GunItemInfo;

        //update stats
        stats.PickGunUpdateState(pickedGunInfo);

        //initialize gun
        InitGun();

        //destroy gun gameobject on the world
        Destroy(colliders[nearestIdx].gameObject);
    }

    private void SwitchGun()
    {
        if(!stats.IsSingleWeapon())
        {
            AudioSource.PlayClipAtPoint(switchWeapon, gameObject.transform.position);
        }
        stats.SwitchGunUpdateState();
        InitGun();
    }

    private void InitGun()
    {
        //remove old gun, instantiate new gun
        //may change the implementation later (disable not in used guns, only leave one active)
        Gun currentGun = stats.CurrentGun;
        if (gunSlotTrans.childCount > 0)
        {
            Destroy(gunSlotTrans.GetChild(0).gameObject);
        }
        GameObject newGun = Instantiate(currentGun.gunPrefab, gunSlotTrans.position, Quaternion.identity, gunSlotTrans);
        newGun.transform.localPosition = currentGun.handPosition;
        newGun.transform.localRotation = Quaternion.Euler(currentGun.handRotation);

        //change animation
        float currentWeaponID = (float)currentGun.weaponID;
        animator.SetFloat("WeaponID", currentWeaponID);
    }

    //for debug only, show player's lock range in the scene (edit mode, not play mode)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }

}
