using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D body { get; private set; }
    new public BoxCollider2D collider { get; private set; }

    // horizontal control
    [SerializeField, Min(0)] private float moveAcc = 50.0f;
    [SerializeField, Min(0)] private float moveMaxVel = 5f;
    [SerializeField, Min(0)] private float maxJumpHeight;


    // vertical control state
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpLaunchDuration = .5f; // for max jump, hold
    StopWatch jumpWatch;
    bool isJumping;

    // general state
    private float jumpVel => Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y / (Time.fixedDeltaTime * Time.fixedDeltaTime)) * maxJumpHeight); // should set in Awake and cached
    private Vector2 moveDirection;

    // data cache

    ContactPoint2D[] currentContacts = new ContactPoint2D[10];

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        jumpWatch = new StopWatch();
        //jumpVel = ; should be cached

    }

    public void SetMoveInput(Vector2 dir)
    {
        moveDirection = dir;
    }

    void Update()
    {
        // get direction of player

        // flip sprite based on horizontal movement
        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        HandleMovement(moveDirection);
        HandleJump();
        HandleAnimator();
    }

    /**
     * Side to side movement
     */
    private void HandleMovement(Vector2 direction)
    {
        float inputHorizontal = Mathf.Approximately(direction.x, 0f) ? 0f : Mathf.Sign(direction.x);

        float currVelHorizontal = body.linearVelocity.x;

        float resolvedVelHorizontal = currVelHorizontal;

        // Instant stop
        if (Mathf.Approximately(inputHorizontal, 0f) || // no input
            (!Mathf.Approximately(currVelHorizontal, 0f) && Mathf.Sign(inputHorizontal) != Mathf.Sign(currVelHorizontal))) // u-turn
        {
            resolvedVelHorizontal = 0f;
        }
        resolvedVelHorizontal += inputHorizontal * moveAcc * Time.fixedDeltaTime;
        resolvedVelHorizontal = Mathf.Sign(resolvedVelHorizontal) * Mathf.Min(Mathf.Abs(resolvedVelHorizontal), moveMaxVel);

        float resolvedX = resolvedVelHorizontal;

        body.linearVelocity = new Vector2(resolvedX, body.linearVelocity.y);
    }

    /**
     * If we are in the process of jumping, we will accelerate
     * for jumpLaunchDuration to approximately reach jumpHeight height.
     */
    private void HandleJump()
    {
        if (isJumping && jumpWatch <= jumpLaunchDuration)
        {
            // Over jumpLaunchDuration, apply a total of jumpVel acceleration
            float jumpAcc = jumpVel * (Time.fixedDeltaTime / Mathf.Max(Time.fixedDeltaTime, jumpLaunchDuration));
            body.AddForceY(jumpAcc * body.mass);
        }
    }

    private void HandleAnimator()
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            // animator.SetBool("isMoving", true);
            if (moveDirection.x != 0)
            {
                // animator.SetBool("isSideways", true);
                // animator.SetBool("isForward", false);
                // animator.SetBool("isBackward", false);
            }
            if (moveDirection.y < 0)
            {
                // animator.SetBool("isSideways", false);
                // animator.SetBool("isForward", true);
                // animator.SetBool("isBackward", false);
            }
            if (moveDirection.y > 0)
            {
                // animator.SetBool("isSideways", false);
                // animator.SetBool("isForward", false);
                // animator.SetBool("isBackward", true);
            }
        }
        else
        {
            // animator.SetBool("isMoving", false);
        }

    }

    /**
     * Check if any existing collision contacts are downward.
     */
    private bool CheckIfGrounded()
    {
        int count = body.GetContacts(currentContacts);

        // ez case
        if (count == 0)
        {
            return false;
        }
        foreach (ContactPoint2D contact in currentContacts)
        {
            if (contact.normal.y > 0)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Referenced by the Player Input to pass move commands
     */
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    
    /**
     * Referenced by the Player Input to pass jump command.
     * Value comes in as 1 (pressed) or 0 (unpressed).
     */
    public void JumpInput(InputAction.CallbackContext context)
    {
        float jump = context.ReadValue<float>();

        if (jump > 0f && isGrounded && isJumping == false)
        {
            jumpWatch.Start();
            isJumping = true;
        }
        else if (jump <= 0f)
        {
            isJumping = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
