using UnityEngine;

public class Player : MonoBehaviour
{
    private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private Animator an;

    // Movement Parameters
    private readonly float MoveSpeed = 10f;
    private float CurrentSpeed = 0f;
    private readonly float JumpForce = 15f;
    private bool Jumped, DoubleJumped = false;

    // Idle Parameters
    private readonly float IdleTimeDefault = 7;
    private readonly float SleepTimeDefault = 35;
    private float IdleTime, SleepTime;

    // Boolean States
    private bool _moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");

        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        MovePlayer();
    }

    // Get/Set
    public float GetMoveSpeed() => MoveSpeed;
    public void SetCurrentSpeed(float speed) => CurrentSpeed = speed;
    public float GetCurrentSpeed() => CurrentSpeed;
    public float GetJumpForce() => JumpForce;
    public bool GetJumped() => Jumped;
    public void SetJumped(bool jumped) => Jumped = jumped;
    public bool GetDoubleJumped() => DoubleJumped;
    public void SetDoubleJumped(bool doubleJumped) => DoubleJumped = doubleJumped;

    // Animation Methods
    private void UpdateIdleAnimator()
    {
        an.SetBool("idle", true);
        if (IdleTime <= 0)
        {
            an.SetBool("idle2", true);
        }

        if (SleepTime <= 0) UpdateSleepAnimator(true);
    }

    private void UpdateSleepAnimator(bool sleep)
    {
        an.SetBool("sleep", sleep);
    }

    private void UpdateHorizontalAnimator(float xMove, bool moving)
    {
        if (moving) an.SetBool("idle", false);

        Vector3 scale = transform.localScale;

        if (xMove > 0 )
            transform.localScale = new(Mathf.Abs(scale.x), scale.y, scale.z);
        if (xMove < 0)
            transform.localScale = new(-Mathf.Abs(scale.x), scale.y, scale.z);
    }

    // Methods
    private bool CheckGrounded()
    {
        Vector2 origin = new(transform.position.x, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1.1f, LayerMask.GetMask("Ground"));
        Collider2D ground = Physics2D.OverlapCircle(origin, 0.4f, groundLayer);

        Debug.DrawRay(origin, Vector2.down * 1.1f, Color.green);
        return hit.collider != null || ground != null;
    }

    private void MovePlayer()
    {
        if (!Input.anyKey) {
            if (an.GetBool("sleep")) return;

            IdleTime -= Time.deltaTime;
            SleepTime -= Time.deltaTime;

            UpdateIdleAnimator();
            return;
        }

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
        UpdateSleepAnimator(false);

        float xVelocity = GetXVelocity();
        _moving = (xVelocity != 0f);
        UpdateHorizontalAnimator(xVelocity, _moving);
        Debug.Log(xVelocity);
        rb.linearVelocityX = xVelocity;
    }

    public float GetXVelocity()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) return 0f;

        //int direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        //return direction*MoveSpeed*Time.deltaTime;
        return Input.GetAxis("Horizontal")*MoveSpeed;
    }
}
