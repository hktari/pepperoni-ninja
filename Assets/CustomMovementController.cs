using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CustomMovementController : MonoBehaviour
{
    public GameObject Shuriken;

    public float MaxSpeed = 10.0f;
    public float MaxSpeedAirborne = 1.0f;
    public float MaxFallSpeed = 30.0f;

    public Vector2 Velocity;
    public float Gravity = 9.8f;
    public float JumpForce = 3.0f;
    public float WallJumpForce = 10.0f;

    public LayerMask IsGroundMask;
    public LayerMask IsWallMask;

    public float JumpTimeInSec = 1.5f;
    public float m_jumpCD;
    public float GravityMultiplier = 3.0f;

    public float k_GroundCheckRadius = 0.2f;
    public float k_WallCheckWidth = 2f;
    public Vector2 m_WallJumpDirToLeft = Vector2.left + Vector2.up;
    public Vector2 m_WallJumpDirToRight = Vector2.right + Vector2.up;

    private Rigidbody2D m_Rigidbody2D;
    private bool jump;
    private bool falling;
    private bool m_IsOnGround;
    private bool m_IsOnWall;
    private Transform m_GroundCheck;
    private Transform m_WallCheckLeft;
    private Transform m_WallCheckRight;
    public Animator m_Anim;            // Reference to the player's animator component.
    private bool m_FacingRight = true;
    private Transform m_collidingWallTransform;
    private RhytmManager m_RhytmManager;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_WallCheckLeft = transform.Find("WallCheckLeft");
        m_WallCheckRight = transform.Find("WallCheckRight");
        m_RhytmManager = GameObject.Find("RhytmManager").GetComponent<RhytmManager>();
        if (m_Anim == null) m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_IsOnGround = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundCheckRadius, IsGroundMask)
            .Where(c => c.gameObject != gameObject && !c.isTrigger) // Ignore triggers and self
            .Count() > 0;

        var wallJumpDir = GetWallJumpDir();
        m_IsOnWall = !m_IsOnGround && wallJumpDir != Vector2.zero;

        Vector2 gravity = Vector2.down * Gravity;
        Vector2 newVelocity = Velocity + gravity;

        float horiz = Input.GetAxis("Horizontal");

        if (jump)
        {
            if (m_IsOnGround)
            {
                newVelocity += Vector2.up * JumpForce;
                jump = false;
            }
            else if (m_IsOnWall)
            {
                Flip();
                newVelocity = wallJumpDir * WallJumpForce;
                Debug.Log("WALL JUMP");
                jump = false;
            }
        }

        if (m_IsOnGround)
        {
            newVelocity.x = horiz * MaxSpeed;

            if (!jump)
            {
                newVelocity.y = Mathf.Max(0.0f, newVelocity.y);
            }
        }
        else
        {
            newVelocity += Vector2.right * horiz * MaxSpeedAirborne;
        }

        Velocity = new Vector2(
            Mathf.Clamp(newVelocity.x, -MaxSpeed, MaxSpeed),
            Mathf.Max(newVelocity.y, -MaxFallSpeed));

        m_Anim.SetFloat("Speed", Mathf.Abs(Velocity.x));
        m_Anim.SetFloat("vSpeed", Velocity.y);
        m_Anim.SetBool("Ground", m_IsOnGround);

        // If the input is moving the player right and the player is facing left...
        if (Velocity.x > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (Velocity.x < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

        m_Rigidbody2D.MovePosition(
            new Vector2(m_Rigidbody2D.transform.position.x, m_Rigidbody2D.transform.position.y) + Velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        var t_jump = Input.GetButtonDown("Jump");
        if (t_jump && m_RhytmManager.TryPerformAction())
        {
            jump = true;
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Instantiate(Shuriken, new Vector3(transform.position.x, transform.position.y, 100), this.transform.rotation);
        }
    }


    private Vector2 GetWallJumpDir()
    {
        var wallCheckPosLeft = m_FacingRight ? m_WallCheckLeft : m_WallCheckRight;
        var wallCheckPosRight = m_FacingRight ? m_WallCheckRight : m_WallCheckLeft;

        bool isTouchingLeft = wallCheckPosLeft.GetComponent<BoxCollider2D>().IsTouchingLayers(IsWallMask);
        bool isTouchingRight = wallCheckPosRight.GetComponent<BoxCollider2D>().IsTouchingLayers(IsWallMask);

        var m_WallJumpDir = Vector2.zero;
        if (isTouchingLeft)
        {
            m_WallJumpDir += m_WallJumpDirToRight;
        }
        if (isTouchingRight)
        {
            m_WallJumpDir += m_WallJumpDirToLeft;
        }

        return m_WallJumpDir;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        if (m_GroundCheck != null)
        {
            Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundCheckRadius); ;
        }

        //if (m_WallCheckLeft != null)
        //{
        //    Gizmos.DrawWireCube(m_WallCheckLeft.position, new Vector3(k_WallCheckWidth * 0.5f, k_WallCheckWidth, 1.0f));
        //}


        //if (m_WallCheckRight != null)
        //{
        //    Gizmos.DrawWireSphere(m_WallCheckRight.position, new Vector3(k_WallCheckWidth * 0.5f, k_WallCheckWidth, 1.0f));
        //}
    }
}
