using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D body { get; private set; }
    new public BoxCollider2D collider { get; private set; }

    // horizontal control
    [SerializeField] private float moveAcc = 50.0f;
    [SerializeField] private float moveMaxVel = 5f;
    [SerializeField] private float jumpHeight;


    // vertical control state
    [SerializeField] private bool isGrounded;

    // general state
    private float jumpAcc;
    private Vector2 moveInput;
    bool isJumping;

    // data cache

    ContactPoint2D[] currentContacts = new ContactPoint2D[10];

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        jumpAcc = 2f * jumpHeight * Mathf.Abs(Physics2D.gravity.y);
    }

    public void SetMoveInput(Vector2 dir)
    {
        moveInput = dir;
    }

    void Update()
    {
        // get direction of player

        // flip sprite based on horizontal movement
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        if (isJumping && !isGrounded)
        {
            isJumping = false;
        }
        MovePlayer();
    }

    public void HandleInput(Vector2 direction)
    {
        float inputDirHorz = Mathf.Approximately(direction.x, 0f) ? 0f : Mathf.Sign(direction.x);
        float inputDirVert = Mathf.Approximately(direction.y, 0f) ? 0f : Mathf.Sign(direction.y);

        float currHorz = body.linearVelocity.x;
        float currVert = body.linearVelocity.y;

        float desiredVelHorz = currHorz;

        // Instant stop
        if (Mathf.Approximately(inputDirHorz, 0f) || // no input
            (!Mathf.Approximately(currHorz, 0f) && Mathf.Sign(inputDirHorz) != Mathf.Sign(currHorz))) // u-turn
        {
            desiredVelHorz = 0f;
        }
        desiredVelHorz += inputDirHorz * moveAcc * Time.fixedDeltaTime;
        desiredVelHorz = Mathf.Sign(desiredVelHorz) * Mathf.Min(Mathf.Abs(desiredVelHorz), moveMaxVel);

        // Jump
        if (!isJumping && isGrounded && inputDirVert > 0)
        {
            body.AddForceY(jumpAcc * body.mass);
            isJumping = true;
        }

        float resolvedX = desiredVelHorz;
        float resolvedY = currVert;

        body.linearVelocity = new Vector2(resolvedX, resolvedY);
    }

    private void MovePlayer()
    {
        // Move Player
        HandleInput(moveInput);

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetBool("isMoving", true);
            if (moveInput.x != 0)
            {
                animator.SetBool("isSideways", true);
                animator.SetBool("isForward", false);
                animator.SetBool("isBackward", false);
            }
            if (moveInput.y < 0)
            {
                animator.SetBool("isSideways", false);
                animator.SetBool("isForward", true);
                animator.SetBool("isBackward", false);
            }
            if (moveInput.y > 0)
            {
                animator.SetBool("isSideways", false);
                animator.SetBool("isForward", false);
                animator.SetBool("isBackward", true);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
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
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
