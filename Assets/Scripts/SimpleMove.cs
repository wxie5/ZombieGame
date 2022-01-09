using UnityEngine;

namespace Prototype.Movement
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

        //Movement Variables
        private Vector3 gVelocity;
        private Vector2 inputAxis;
        private float currentSpeed;

        //Smooth Variables
        private float playerRotSmoothRef;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
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
            Vector3 movementVector = new Vector3(inputAxis.x, 0f, inputAxis.y);
            cc.Move(movementVector * currentSpeed * Time.deltaTime);
        }

        private void SetAnimationParam()
        {
            animator.SetFloat("Speed", currentSpeed);
        }
    }

    public enum Player
    {
        PlayerA,
        PlayerB
    }
}
