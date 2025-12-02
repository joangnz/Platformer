using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    // Movement Parameters
    private readonly float MoveSpeed = 50f;
    private float CurrentSpeed = 0f;
    private readonly float JumpForce = 200f;
    private readonly float Gravity = -9.81f;
    private int JumpCount = 0;

    // Idle Parameters
    private readonly float IdleTimeDefault = 7;
    private readonly float SleepTimeDefault = 35;
    private float IdleTime, SleepTime;

    // Boolean States
    private bool _moving, _airborne = false;
    private bool _grounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
    }

    // Get/Set
    public float GetMoveSpeed() => MoveSpeed;
    public void SetCurrentSpeed(float speed) => CurrentSpeed = speed;
    public float GetCurrentSpeed() => CurrentSpeed;
    public float getJumpForce() => JumpForce;
    public int GetJumpCount() => JumpCount;
    public void ResetJumpCount() => JumpCount = 0;

    // Animation Methods
    private void UpdateIdleAnimator()
    {
        if (IdleTime <= 0)
        {
            _animator.SetBool("west", false);
            _animator.SetBool("east", false);
            _animator.SetBool("south", true);
        }

        if (SleepTime <= 0) UpdateSleepAnimator(true);
    }

    private void UpdateSleepAnimator(bool sleep)
    {
        _animator.SetBool("sleep", sleep);
    }

    private void UpdateHorizontalAnimator(float xMove, bool moving)
    {
        if (moving) _animator.SetBool("south", false);

        if (xMove > 0 )
        {
            _animator.SetBool("east", true);
            _animator.SetBool("west", false);
        } else
        {
            _animator.SetBool("east", false);
            _animator.SetBool("west", true);
        }
    }

    // Methods
    private void ReadInput()
    {
        if (!Input.anyKey) {
            if (_animator.GetBool("sleep")) return;

            IdleTime -= Time.deltaTime;
            SleepTime -= Time.deltaTime;

            UpdateIdleAnimator();
            return;
        }

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
        UpdateSleepAnimator(false);

        float xMove = GetHMove();
        _moving = (xMove != 0f);
        UpdateHorizontalAnimator(xMove, _moving);

        float yMove = GetVMove();

        _rigidbody2D.MovePosition(new Vector2(
            xMove + transform.position.x,
            yMove + transform.position.y
            ));

        // _rigidbody2D.AddForceY()

    }

    public float GetHMove()
    {
        return Input.GetAxis("Horizontal")*MoveSpeed*Time.deltaTime;
    }

    public float GetVMove()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_grounded) Jump(); else DoubleJump();

            (_grounded, _airborne) = (_airborne, _grounded);

            return JumpForce*Time.deltaTime;
        }

        return Gravity;
    }

    private void Jump()
    {

        JumpCount++;
        return;
    }

    private void DoubleJump()
    {
        if (JumpCount >= 2) return;


        JumpCount++;
        return;
    }

}
