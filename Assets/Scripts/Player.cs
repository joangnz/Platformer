using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator an;

    // Movement Parameters
    private readonly float MoveSpeed = 10f;
    private float CurrentSpeed = 0f;
    private readonly float JumpForce = 10f;
    private bool Jumping, DoubleJumped = false;
    private bool Grounded = true;

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
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGrounded())
        {
            SetJumping(false);
            SetDoubleJumped(false);
        }
        MovePlayer();
        JumpPlayer();
    }

    // Get/Set
    public float GetMoveSpeed() => MoveSpeed;
    public void SetCurrentSpeed(float speed) => CurrentSpeed = speed;
    public float GetCurrentSpeed() => CurrentSpeed;
    public float GetJumpForce() => JumpForce;
    public bool GetJumping() => Jumping;
    public void SetJumping(bool jumping) => Jumping = jumping;
    public bool GetDoubleJumped() => DoubleJumped;
    public void SetDoubleJumped(bool doubleJumped) => DoubleJumped = doubleJumped;
    public bool GetGrounded() => Grounded;
    public void SetGrounded(bool grounded) => Grounded = grounded;

    // Animation Methods
    private void UpdateIdleAnimator()
    {
        an.SetBool("idle", true);
        an.SetBool("running", false);
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
        if (moving)
        {
            an.SetBool("idle", false);
            an.SetBool("idle2", false);
            an.SetBool("sleep", false);
            an.SetBool("running", true);
        }

        if (xMove > 0)
            sr.flipX = false;
        if (xMove < 0)
            sr.flipX = true;
    }

    // Methods
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        bool grounded = hit.collider != null;

        Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.green);
        SetGrounded(grounded);
        return grounded;
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

        float xVelocity = GetXVelocity();
        _moving = (xVelocity != 0f);
        UpdateHorizontalAnimator(xVelocity, _moving);
        rb.linearVelocityX = xVelocity;
    }

    private float GetXVelocity()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) return 0f;

        return Input.GetAxis("Horizontal")*MoveSpeed;
    }

    private void JumpPlayer()
    {
        if (!Input.GetKey(KeyCode.Space)) SetJumping(false);

        if (GetJumping()) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!GetGrounded())
            {
                if (GetDoubleJumped()) return;

                SetDoubleJumped(true);
            }

            an.SetBool("jump", true);
            SetJumping(true);
            rb.linearVelocityY = JumpForce;
        }
    }

    public void OnJumpEnd()
    {
        // Change the bool parameter in the Animator
        an.SetBool("jump", false);
    }
}
