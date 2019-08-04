using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomMovementController : MonoBehaviour
{
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
    public float k_WallCheckRadius = 2f;
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

    // Start is called before the first frame update
    void Start()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_WallCheckLeft = transform.Find("WallCheckLeft");
        m_WallCheckRight = transform.Find("WallCheckRight");
        if (m_Anim == null) m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_IsOnGround = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundCheckRadius, IsGroundMask)
            .Where(c => c.gameObject != gameObject)
            .Count() > 0;
        //GetComponent<CapsuleCollider2D>().IsTouchingLayers(IsGroundMask);
        m_IsOnWall = !m_IsOnGround && GetComponent<CapsuleCollider2D>().IsTouchingLayers(IsWallMask);

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
                newVelocity = GetWallJumpDir() * WallJumpForce;
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
        jump = Input.GetButtonDown("Jump");
    }


    private Vector2 GetWallJumpDir()
    {
        var wallCheckPosLeft = m_FacingRight ? m_WallCheckLeft : m_WallCheckRight;
        var wallCheckPosRight = m_FacingRight ? m_WallCheckRight : m_WallCheckLeft;

        Collider2D[] collidersWallLeft = Physics2D.OverlapCircleAll(wallCheckPosLeft.position, k_WallCheckRadius, IsWallMask);
        Collider2D[] collidersWallRight = Physics2D.OverlapCircleAll(wallCheckPosRight.position, k_WallCheckRadius, IsWallMask);

        var m_WallJumpDir = Vector2.zero;
        m_collidingWallTransform = null;
        if (collidersWallLeft.Count() > 0)
        {
            m_collidingWallTransform = collidersWallLeft.First().transform;
            m_WallJumpDir = m_WallJumpDirToRight;
            Debug.Log("jump At left wall");
        }
        else if (collidersWallRight.Count() > 0)
        {
            m_collidingWallTransform = collidersWallRight.First().transform;
            m_WallJumpDir = m_WallJumpDirToLeft;
            Debug.Log("jump At right wall");
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
        if (m_GroundCheck == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundCheckRadius);
    }
}
