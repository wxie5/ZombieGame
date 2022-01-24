using System;
using UnityEngine;
using Utils.MathTool;

public class PlayerMove : MonoBehaviour
{
    [Header("Set In Inspector")]
    [SerializeField] private PlayerID playerNum = PlayerID.PlayerA;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float playerRotSmoothTime = 0.02f;
    [Range(9.81f, 20f)]
    [SerializeField] private float posGravity = 9.81f;

    //Components
    private CharacterController cc;
    private Animator animator;

    //Movement Variables
    private Vector3 gVelocity;
    private Vector2 inputAxis;
    private float currentSpeed;
    private bool canFreeRotMove = true;

    //Smooth Variables
    private float playerRotSmoothRef;
    private float animationDampTime = 0.2f;
    private float animationDampSpeed = 5f;

    public void Initialize(Action<bool> lockStateChange, Action shot)
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        lockStateChange += OnLockStateChangeHandler;
        shot += OnShotHandler;
    }

    public void PlayerMoveSystem(Vector2 axis, float moveSpeed, float targetMoveSpeed, bool onTargetFront, bool onTargetRight)
    {
        inputAxis = axis;
        //rotate player
        RotatePlayerWithAxis();

        //calculate the magnitude of inputAxis
        CalculateCurrentSpeed(moveSpeed);

        //move player based on the inputAxis
        PlayerPositionMovement();

        //set animation parameter;
        SetMovementAnim(onTargetFront, onTargetRight);

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

    private void CalculateCurrentSpeed(float moveSpeed)
    {
        Vector3 movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
        currentSpeed = inputAxis.magnitude * speed * moveSpeed;
    }

    private void PlayerPositionMovement()
    {
        Vector3 movementVector = Vector3.zero;
        movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
        cc.Move(movementVector * currentSpeed * Time.deltaTime);
    }

    private void SetMovementAnim(bool onTargetFront, bool onTargetRight)
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

    //handle event when player lock/unlock the target
    private void OnLockStateChangeHandler(bool isLockingTarget)
    {
        canFreeRotMove = !isLockingTarget;
        animator.SetBool("IsLockTarget", isLockingTarget);

        if (isLockingTarget)
        {
            speed = 2f;
        }
        else
        {
            speed = 5f;
        }
    }

    //handle event when shot
    private void OnShotHandler()
    {
        animator.ResetTrigger("Shot");
        animator.SetTrigger("Shot");
    }
}

