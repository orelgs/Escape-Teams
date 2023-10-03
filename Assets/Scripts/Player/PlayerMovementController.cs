using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovementStats))]
public class PlayerMovementController : MonoBehaviour
{
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;
    private PlayerMovementStats movementStats;
    private float currentRotationVelocity;
    private float gravityVelocity;
    public Animator anim;
    public float allowPlayerWalk = 0.1f;
    public float Speed;
    public float InputX;
    public float InputZ;
    [Header("Animation Smoothing")]
    //[Range(0, 1f)]
    //public float HorizontalAnimSmoothTime = 0.2f;
    //[Range(0, 1f)]
    //public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementStats = GetComponent<PlayerMovementStats>();
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && gravityVelocity < 0.0f)
        {
            gravityVelocity = -1.0f;
        }
        else
        {
            // gravityVelocity += movementStats.Gravity * movementStats.GravityMultiplier * Time.deltaTime;
        }

        direction.y = gravityVelocity;
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotationVelocity, movementStats.SmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        animate();
        characterController.Move(direction * movementStats.Speed * Time.deltaTime);
    }

    private void animate()
    {
        //Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        //Calculate the Input Magnitude
        Speed = new Vector2(input.x, input.y).sqrMagnitude;
        //Physically move player
        if (Speed > allowPlayerWalk)
        {
            anim.SetFloat("Blend", Speed, StartAnimTime, Time.deltaTime);
        }
        else if (Speed < allowPlayerWalk)
        {
            anim.SetFloat("Blend", Speed, StopAnimTime, Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!movementStats.CanMove) return;

        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0.0f, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!movementStats.CanMove) return;
        if (!context.started) return;
        if (!IsGrounded()) return;

        // gravityVelocity += movementStats.JumpPower;
    }

    private bool IsGrounded() => characterController.isGrounded;
}