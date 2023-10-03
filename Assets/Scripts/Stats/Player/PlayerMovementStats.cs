using UnityEngine;

public class PlayerMovementStats : MonoBehaviour
{
    #region Variables: Movement
    [SerializeField] private float speed = 3f;
    public float Speed { get; set; }

    [SerializeField] private bool canMove = true;
    public bool CanMove { get; set; }

    [HideInInspector] public Vector3 smoothMoveVelocity = Vector3.zero;
    public Vector3 MoveAmount { get; set; } = Vector3.zero;
    #endregion

    #region Variables: Rotation
    [SerializeField] private float smoothTime = 0.05f;
    public float SmoothTime { get; set; }
    #endregion

    #region Variables: Gravity
    public bool Grounded { get; set; } = false;
    // public float Gravity { get; set; } = -9.81f;

    // [SerializeField] private float gravityMultiplier = 1f;
    // public float GravityMultiplier { get; set; }
    #endregion

    #region Variables: Jumping
    [SerializeField] private float jumpPower = 4f;
    public float JumpPower { get; set; }
    #endregion

    private void Awake()
    {
        Speed = speed;
        CanMove = canMove;
        SmoothTime = smoothTime;
        // GravityMultiplier = gravityMultiplier;
        JumpPower = jumpPower;
    }
}