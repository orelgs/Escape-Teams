using System.Security;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float speed;
    [SerializeField] private float smoothTime;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool canMove;

    private Vector3 moveAmount = Vector3.zero;
    private bool grounded;

    [SerializeField] private GameObject ui;
    private Rigidbody rb;
    private PhotonView PV;

    [Range(0, 1f)]
    [SerializeField] private float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    [SerializeField] private float StopAnimTime = 0.15f;
    [SerializeField] private float allowPlayerWalk = 0.1f;
    private Animator anim;
    private PlayerInput playerInput;
    private Vector3 previousValidPosition;

    private float currentRotationVelocity;
    private Vector2 input;

    private Camera camera;
    Vector3 cameraForward;
    Vector3 cameraRight;

    [SerializeField] private Light playerSelfLights;
    [SerializeField] private Light playerLights;

    [SerializeField] private JoystickController movementJoystickController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        camera = GetComponentInChildren<Camera>();
        previousValidPosition = transform.position;
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(camera.gameObject.transform.parent.gameObject);
            Destroy(ui);
            Destroy(playerInput);
            Destroy(playerSelfLights.gameObject);
            Destroy(playerLights);
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            // movementJoystickController.EnableJoystickInput();
            // movementJoystickController.MovementOccured += Move;
            // playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);

            if (Application.isMobilePlatform)
            {
                movementJoystickController.EnableJoystickInput();
                movementJoystickController.MovementOccured += Move;
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
            }
            else
            {
                playerInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current, Mouse.current);
            }
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        cameraForward = camera.transform.forward;
        cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;

        Animate();
        ApplyMovement();
        ApplyRotation();
        transformLimits();


    }

    private void transformLimits()
    {
        isPlayerGoingDown();
        if (IsInvalidPosition(this.transform.position))
        {
            this.transform.position = ClampPosition(this.transform.position);
        }
        if (transform.position != previousValidPosition)
        {
            previousValidPosition = transform.position;
        }
    }

    bool IsInvalidPosition(Vector3 position)
    {
        return Mathf.Approximately(position.x, 11.18f) ||
               Mathf.Approximately(position.z, -42.50785f) ||
               Mathf.Approximately(position.z, 0.61f) ||
               Mathf.Approximately(position.x, -6.76f);
    }

    Vector3 ClampPosition(Vector3 position)
    {
        float minX = -7.0f;
        float maxX = 11.13f;
        float minZ = -42.81f;
        float maxZ = 1.02f;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        return position;
    }

    private void isPlayerGoingDown()
    {
        if (transform.position.y < -0.3f)
        {
            this.transform.position = new Vector3(10.65f, 0.042f, -37.9f);
        }
    }

    private void Animate()
    {
        float effectiveSpeed = new Vector2(moveAmount.x, moveAmount.z).sqrMagnitude;

        if (effectiveSpeed > allowPlayerWalk)
        {
            anim.SetFloat("Blend", effectiveSpeed, StartAnimTime, Time.deltaTime);
        }
        else if (effectiveSpeed < allowPlayerWalk)
        {
            anim.SetFloat("Blend", effectiveSpeed, StopAnimTime, Time.deltaTime);
        }
    }

    public void SetMove(bool val)
    {
        canMove = val;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        input = context.ReadValue<Vector2>();
    }

    public void Move(Vector2 movementDirection)
    {
        if (!canMove) return;

        input = movementDirection;
    }

    private void ApplyMovement()
    {
        Vector3 forwardRelativeVerticalInput = input.y * cameraForward;
        Vector3 rightRelativeHorizontalInput = input.x * cameraRight;

        moveAmount = forwardRelativeVerticalInput + rightRelativeHorizontalInput;

        rb.MovePosition(rb.position + moveAmount * (canMove ? speed : 0) * Time.fixedDeltaTime);
    }

    private void ApplyRotation()
    {
        if (moveAmount == Vector3.zero) return;

        var targetAngle = Mathf.Atan2(moveAmount.x, moveAmount.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotationVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (!context.started) return;
        if (!grounded) return;

        rb.AddForce(transform.up * jumpPower);
    }

    public void SetGroundedState(bool grounded)
    {
        this.grounded = grounded;
    }
}
