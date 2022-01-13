using UnityEngine;
using Prototype.Tool;
using Prototype.Combat;

/// <summary>
/// currently this class also act as the animator controller script
/// /// </summary>
namespace Prototype.move
{
    public class SimpleMove : MonoBehaviour
    {
        [Header("Set In Inspector")]
        [SerializeField] private Player playerNum = Player.PlayerA;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float playerRotSmoothTime = 0.02f;
        [Range(9.81f, 20f)]
        [SerializeField] private float posGravity = 9.81f;

        //Components
        private CharacterController cc;
        private Animator animator;
        private SimpleCombat combatController;

        //Movement Variables
        private Vector3 gVelocity;
        private Vector2 inputAxis;
        private float currentSpeed;
        private bool canFreeRotMove = true;

        //Smooth Variables
        private float playerRotSmoothRef;
        private float animationDampTime = 0.2f;
        private float animationDampSpeed = 5f;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            combatController = GetComponent<SimpleCombat>();

            combatController.onLockStateChange += OnLockStateChangeHandler;
            combatController.onShot += OnShotHandler;
        }

        private void Update()
        {
            //receive input base on player number
            GetNormalizedInputAxis();

            //rotate player
            RotatePlayerWithAxis();

            //calculate the magnitude of inputAxis
            CalculateCurrentSpeed();

            //move player based on the inputAxis
            PlayerMovement();

            //set animation parameter;
            SetAnimationParam();

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

        private void GetNormalizedInputAxis()
        {
            float hori, verti;
            if (playerNum == Player.PlayerA)
            {
                hori = Input.GetAxisRaw("Horizontal1");
                verti = Input.GetAxisRaw("Vertical1");
            }
            else
            {
                hori = Input.GetAxisRaw("Horizontal2");
                verti = Input.GetAxisRaw("Vertical2");
            }

            inputAxis = new Vector2(hori, verti).normalized;
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
            Vector3 movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
            currentSpeed = inputAxis.magnitude * speed;
        }

        private void PlayerMovement()
        {
            Vector3 movementVector = Vector3.zero;
            movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
            cc.Move(movementVector * currentSpeed * Time.deltaTime);
        }

        private void SetAnimationParam()
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
                    if (combatController.OnTargetFront)
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
                    if (combatController.OnTargetRight)
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

    public enum Player
    {
        PlayerA,
        PlayerB
    }
}