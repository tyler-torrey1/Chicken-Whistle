using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D body { get; private set; }
    new public BoxCollider2D collider { get; private set; }

    [SerializeField] private AudioClip _jumpAudio;
    // horizontal control
    [SerializeField, Min(0)] private float _moveAcc = 50.0f;
    [SerializeField, Min(0)] private float _moveMaxVel = 5f;
    [SerializeField, Min(0)] private float _maxJumpHeight;


    // vertical control state
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpLaunchDuration = .5f; // for max jump, hold
    StopWatch _jumpWatch;
    bool _isJumping;

    // general state
    private float _jumpVel => 2f * Mathf.Abs(Physics2D.gravity.y * Time.fixedDeltaTime) * _maxJumpHeight; // should set in Awake and cached
    private Vector2 _moveDirection;

    private float _jump;


    // data cache

    ContactPoint2D[] currentContacts = new ContactPoint2D[10];

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning(name + ": Singleton betrayal!");

            instance.transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        _jumpWatch = new StopWatch();
        //jumpVel = ; should be cached

    }

    public void SetMoveInput(Vector2 dir)
    {
        _moveDirection = dir;
    }

    void Update()
    {
        // get direction of player

        // flip sprite based on horizontal movement
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _isGrounded = CheckIfGrounded();
        HandleMovement(_moveDirection);
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
        resolvedVelHorizontal += inputHorizontal * _moveAcc * Time.fixedDeltaTime;
        resolvedVelHorizontal = Mathf.Sign(resolvedVelHorizontal) * Mathf.Min(Mathf.Abs(resolvedVelHorizontal), _moveMaxVel);

        float resolvedX = resolvedVelHorizontal;

        body.linearVelocity = new Vector2(resolvedX, body.linearVelocity.y);
    }

    /**
     * If we are in the process of jumping, we will accelerate
     * for jumpLaunchDuration to approximately reach jumpHeight height.
     */
    private void HandleJump()
    {
        if (_isJumping && _jumpWatch <= _jumpLaunchDuration)
        {
            // Over jumpLaunchDuration, apply a total of jumpVel acceleration
            int frameCount = Mathf.Max(1, Mathf.RoundToInt(_jumpLaunchDuration / Time.fixedDeltaTime));
            float jumpAcc = _jumpVel * (1f / frameCount);
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y + jumpAcc);
        }
    }

    private void HandleAnimator()
    {
        
        // check if left
        if (_moveDirection.x > 0)
        {
            animator.SetBool("isLeft", false);
        }
        else if (_moveDirection.x < 0)
        {
            animator.SetBool("isLeft", true);
        }

        // check if moving
        if (body.linearVelocityX != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // check if jumping
        if (_jump > 0f && _isGrounded && !animator.GetBool("isAirborne"))
        {
            animator.SetTrigger("jump");
            animator.SetBool("isAirborne", true);
        }

        else if (_jump <= 0f && _isGrounded && animator.GetBool("isAirborne"))
        {
            animator.ResetTrigger("jump");
            animator.SetBool("isAirborne", false);
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
            if (contact.normal.y > Mathf.Epsilon)
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
        _moveDirection = context.ReadValue<Vector2>();
    }
    
    /**
     * Referenced by the Player Input to pass jump command.
     * Value comes in as 1 (pressed) or 0 (unpressed).
     */
    public void JumpInput(InputAction.CallbackContext context)
    {
        _jump = context.ReadValue<float>();

        if (_jump > 0f && _isGrounded && _isJumping == false)
        {
            _jumpWatch.Start();
            _isJumping = true;
            AudioSource.PlayClipAtPoint(_jumpAudio, body.worldCenterOfMass);
        }
        else if (_jump <= 0f)
        {
            _isJumping = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
