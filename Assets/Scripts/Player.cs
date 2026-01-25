using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // Scene Management
    private LayerMask groundLayer;
    private Camera cam;
    private float camOS;
    private TilePainter tp;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator an;

    private NightSlash ns;
    private SpriteRenderer nssr;

    // Colliders
    private BoxCollider2D ihc;
    private CircleCollider2D nsc;

    // Movement Parameters
    private readonly float MoveSpeed = 10f;
    private float CurrentSpeed = 0f;
    private readonly float JumpForce = 10f;
    private bool Jumping, DoubleJumped = false;
    private bool Grounded, Dashable = true;
    private bool Dashing = false;
    private readonly float DashSpeed = 4f;
    private readonly float DashDuration = 0.1f;
    private readonly float DashCooldown = 1;

    private bool Downslashable = false;
    private bool Downslashing = false;
    private readonly float DownslashSpeed = 4f;

    private bool NightSlashing = false;
    private bool NightSlashable = true;
    private float NightSlashCooldown = 1f;

    // Idle Parameters
    private readonly float IdleTimeDefault = 7;
    private readonly float SleepTimeDefault = 35;
    private float IdleTime, SleepTime;

    // Boolean States
    private bool _moving = false;

    public void Init(TilePainter tp, Camera cam)
    {
        this.tp = tp;
        this.cam = cam;
        camOS = this.cam.orthographicSize;
    }

    private void Awake()
    {
        ns = gameObject.GetComponentInChildren<NightSlash>();
        nssr = ns.GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();

        nssr.enabled = false;

        ihc = GetComponentInChildren<BoxCollider2D>();
        ihc.enabled = false;
        nsc = GetComponentInChildren<CircleCollider2D>();
        nsc.enabled = false;

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGrounded())
        {
            an.SetBool("jump", false);
            SetJumping(false);
            SetDoubleJumped(false);
            SetDownslashing(false);
            SetDownslashable(false);
            Downslash(false);
        }

        MovePlayer();
        JumpPlayer();
    }

    // Get/Set
    public TilePainter GetTilePainter() => tp;
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
    public bool GetDashable() => Dashable;
    public void SetDashable(bool dashable) => Dashable = dashable;
    public bool GetDashing() => Dashing;
    public void SetDashing(bool dashing) => Dashing = dashing;
    public float GetDashSpeed() => DashSpeed;
    public float GetDashDuration() => DashDuration;
    public float GetDashCooldown() => DashCooldown;
    public bool GetDownslashable() => Downslashable;
    public void SetDownslashable(bool downslashable) => Downslashable = downslashable;
    public bool GetDownslashing() => Downslashing;
    public void SetDownslashing(bool downslashing) => Downslashing = downslashing;
    public float GetDownslashSpeed() => DownslashSpeed;
    public bool GetNightSlashable() => NightSlashable;
    public void SetNightSlashable(bool nightSlashable) => NightSlashable = nightSlashable;
    public bool GetNightSlashing() => NightSlashing;
    public void SetNightSlashing(bool nightSlashing) => NightSlashing = nightSlashing;
    public float GetNightSlashCooldown() => NightSlashCooldown;


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

        if (GetDashing()) an.SetBool("dash", true);

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

    private RaycastHit2D? CheckFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            sr.flipX ? transform.position : new(transform.position.x + 0.1f, transform.position.y, transform.position.z),
            sr.flipX ? Vector2.left : Vector2.right,
            2f,
            groundLayer);
        Debug.DrawRay(transform.position, sr.flipX ? Vector2.left * 2f : Vector2.right * 2f, Color.green);

        if (hit.collider != null)
        {
            return hit;
        }

        return null;
    }

    private void ApplyDash()
    {
        RaycastHit2D? hit = CheckFront();
        if (hit != null)
        {
            Tilemap tilemap = hit.Value.collider.gameObject.GetComponent<Tilemap>();
            if (tilemap == null) return;

            Vector3 point = hit.Value.point;
            
            if (sr.flipX)
            {
                point.x -= 0.1f;
            }
            Vector3Int cell = tilemap.WorldToCell(point);
            Vector3 hitPos = tilemap.CellToWorld(cell);
            tp.DestroyTile(Vector2Int.FloorToInt(hitPos));
        }
    }

    private void MovePlayer()
    {
        // Idle Logic
        if (!Input.anyKey)
        {
            if (an.GetBool("sleep")) return;

            IdleTime -= Time.deltaTime;
            SleepTime -= Time.deltaTime;

            UpdateIdleAnimator();
            return;
        }

        IdleTime = IdleTimeDefault;
        SleepTime = SleepTimeDefault;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift) && GetDashable())
            {
                SetDashing(true);
                SetDashable(false);
                StartCoroutine(Dash());
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl) && GetDownslashable())
                {
                    Downslash(true);
                    SetDownslashing(true);
                    SetDownslashable(false);
                }
                else if (!Input.GetKey(KeyCode.LeftControl) && GetDownslashing())
                {
                    Downslash(false);
                    SetDownslashing(false);
                    SetDownslashable(true);
                }
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (GetNightSlashable() && !GetNightSlashing()) StartCoroutine(NightSlashRoutine());
        }

        float dashMultiplier = GetDashing() ? DashSpeed : 1f;

        if (GetDashing()) ApplyDash();

        // Movement Logic
        float xVelocity = GetXVelocity();
        _moving = (xVelocity != 0f);
        UpdateHorizontalAnimator(xVelocity, _moving);
        // if force is right and lvx is less than velocity, apply force
        // if force is left and lvx is more than velocity, apply force
        if ((xVelocity > 0 && rb.linearVelocityX < xVelocity * dashMultiplier) 
            || (xVelocity < 0 && rb.linearVelocityX > xVelocity * dashMultiplier)) {
            rb.AddForceX(xVelocity * dashMultiplier);
        }
        //rb.linearVelocityX = xVelocity * dashMultiplier;
    }

    private float GetXVelocity()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) return 0f;

        return Input.GetAxis("Horizontal")*MoveSpeed;
    }

    private void JumpPlayer()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            SetJumping(false);
            ToggleIronHead(false);
        }

        if (GetJumping()) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!GetGrounded())
            {
                if (GetDoubleJumped()) return;

                SetDoubleJumped(true);
                an.SetBool("jump", false);
            }
            else
            {
                ToggleIronHead(true);
            }

            an.SetBool("jump", true);
            SetJumping(true);
            rb.linearVelocityY = JumpForce;
            SetDownslashable(true);
        }
    }

    public void OnJumpEnd()
    {
        an.SetBool("jump", false);
    }

    public void OnDashEnd()
    {
        an.SetBool("dash", false);
    }

    private IEnumerator Dash()
    {
        cam.orthographicSize = camOS + 0.1f;
        yield return new WaitForSeconds(DashDuration);
        SetDashing(false);
        cam.orthographicSize = camOS;
        StartCoroutine(StartDashCooldown());
    }

    private IEnumerator StartDashCooldown()
    {
        yield return new WaitForSeconds(DashCooldown);
        SetDashable(true);
    }

    private IEnumerator NightSlashRoutine()
    {
        nssr.enabled = true;
        nsc.enabled = true;
        SetNightSlashing(true);
        SetNightSlashable(false);

        yield return new WaitForSeconds(0.2f);
        nsc.enabled = false;
        nssr.enabled = false;
        SetNightSlashing(false);

        yield return new WaitForSeconds(NightSlashCooldown);
        SetNightSlashable(true);
    }

    private void Downslash(bool b)
    {
        if (b)
        {
            cam.orthographicSize = camOS + 0.1f;
            rb.linearVelocityY = -0.1f;
            rb.gravityScale = 8f;
        }
        else
        {
            cam.orthographicSize = camOS;
            rb.gravityScale = 2f;
        }

    }

    private void ToggleIronHead(bool t)
    {
        ihc.enabled = t;
    }
}
