using System;
using UnityEngine;
using System.Linq;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private LayerMask m_WhatIsWall;                  // A mask determining what is ground to the character
        public float k_WallCheckRadius = 2f;
        public float m_WallJumpForce = 300;
        public Vector2 m_WallJumpDirToLeft = Vector2.left + Vector2.up;
        public Vector2 m_WallJumpDirToRight = Vector2.right + Vector2.up;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        private Transform m_WallCheckLeft;
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Transform m_WallCheckRight;
        public Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private CircleCollider2D m_FeetCircleCollider;

        private bool m_WallJump = false;
        private Vector2 m_WallJumpDir;
        // The wall the character is currently touching
        private Transform m_TouchingWall;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_WallCheckLeft = transform.Find("WallCheckLeft");
            m_WallCheckRight = transform.Find("WallCheckRight");
            if(m_Anim == null ) m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_FeetCircleCollider = GetComponent<CircleCollider2D>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }

        private void HandleWallJump()
        {
            var wallCheckPosLeft = m_FacingRight ? m_WallCheckLeft : m_WallCheckRight;
            var wallCheckPosRight = m_FacingRight ? m_WallCheckRight : m_WallCheckLeft;

            Collider2D[] collidersWallLeft = Physics2D.OverlapCircleAll(wallCheckPosLeft.position, k_WallCheckRadius, m_WhatIsWall);
            Collider2D[] collidersWallRight = Physics2D.OverlapCircleAll(wallCheckPosRight.position, k_WallCheckRadius, m_WhatIsWall);

            m_WallJumpDir = Vector2.zero;
            m_TouchingWall = null;
            if (collidersWallLeft.Count() > 0)
            {
                m_WallJumpDir = m_WallJumpDirToRight;
                m_TouchingWall = collidersWallLeft.First().transform;
            }
            else if (collidersWallRight.Count() > 0)
            {
                m_WallJumpDir = m_WallJumpDirToLeft;
                m_TouchingWall = collidersWallRight.First().transform;
            }

            m_WallJump = m_WallJumpDir != Vector2.zero;
        }

        public void Move(float move, bool crouch, bool jump)
        {
            HandleWallJump();

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                var newVelocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                #region Bugfix: character levitates as long as he runs towards wall while in mid-air
                if (m_WallJump && move != 0.0f)
                {
                    bool movingTowardsWall = false;
                    if (m_Rigidbody2D.position.x >= m_TouchingWall.position.x && move < 0.0f)
                    {
                        movingTowardsWall = true;
                    }
                    else if (m_Rigidbody2D.position.x <= m_TouchingWall.position.x && move > 0.0f)
                    {
                        movingTowardsWall = true;
                    }

                    if (movingTowardsWall)
                    {
                        Debug.Log("MOVING TOWARDS WALL!");
                        newVelocity.x = 0.0f;
                    }
                }
                #endregion
                
                // Move the character
                m_Rigidbody2D.velocity = newVelocity;

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }

            if (jump)
            {
                // If the player should jump...
                if (m_Grounded && m_Anim.GetBool("Ground"))
                {
                    // Add a vertical force to the player.
                    m_Grounded = false;
                    m_Anim.SetBool("Ground", false);
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                }
                else if (m_WallJump)
                {
                    var dir = m_WallJumpDir * m_WallJumpForce;
                    m_Rigidbody2D.velocity = Vector2.zero;
                    m_Rigidbody2D.AddForce(dir);

                    Flip();
                    Debug.Log("WALL JUMP: " + dir);
                }
            }
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
    }
}
