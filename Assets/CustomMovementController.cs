﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovementController : MonoBehaviour
{
    public float MaxSpeed = 10.0f;
    public Vector2 Velocity;
    public float Gravity = 9.8f;
    public float JumpForce = 3.0f;
    public LayerMask IsGroundMask;
    public LayerMask IsWallMask;

    public float JumpTimeInSec = 1.5f;
    public float m_jumpCD;
    public float GravityMultiplier = 3.0f;

    private float m_jumpStartY;
    private Rigidbody2D m_Rigidbody2D;
    private bool jump;
    private bool falling;
    private bool m_IsOnGround;
    private bool m_IsOnWall;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up references.
        //m_GroundCheck = transform.Find("GroundCheck");
        //m_CeilingCheck = transform.Find("CeilingCheck");
        //m_WallCheckLeft = transform.Find("WallCheckLeft");
        //m_WallCheckRight = transform.Find("WallCheckRight");
        //if (m_Anim == null) m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //m_FeetCircleCollider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        // float vertVelocity = 0.0f;
        ////vertVelocity -= Gravity * Time.fixedDeltaTime;
        // if (jump)
        // {
        //     m_jumpCD += Time.fixedDeltaTime;

        //     vertVelocity = m_jumpCD * m_jumpCD * JumpForce / 2.0f;

        //     if (m_jumpCD >= JumpTimeInSec)
        //     {
        //         jump = false;
        //         falling = true;
        //         m_jumpCD = 0.0f;
        //         m_jumpStartY = m_Rigidbody2D.position.y;
        //     }
        // }
        // else if (falling)
        // {
        //     m_jumpCD += Time.fixedDeltaTime;

        //     vertVelocity = m_jumpCD * m_jumpCD * -Gravity / 2.0f;

        //     if (m_jumpCD >= JumpTimeInSec)
        //     {
        //         jump = false;
        //         falling = false;
        //         m_jumpCD = 0.0f;
        //     }
        // }
        // float newY = vertVelocity != 0.0f ? m_jumpStartY + vertVelocity : m_Rigidbody2D.position.y;
        m_IsOnGround = GetComponent<CapsuleCollider2D>().IsTouchingLayers(IsGroundMask);
        m_IsOnWall = !m_IsOnGround && GetComponent<CapsuleCollider2D>().IsTouchingLayers(IsWallMask);

        if (jump)
        {
            if (m_IsOnGround)
            {
                Velocity.y += JumpForce;
            }
            else if (m_IsOnWall)
            {
                Velocity += new Vector2(-1.0f, 1.0f) * JumpForce;
            }
        }

        var vertVelocity = Velocity.y - GravityMultiplier * Gravity * Mathf.Pow(Time.fixedDeltaTime, 2);
     
        if (m_IsOnGround)
        {
            if (!jump)
            {
                vertVelocity = Mathf.Max(0.0f, vertVelocity);
            }
        }

        float horiz = Input.GetAxis("Horizontal");

        Velocity = new Vector2(
            Mathf.Clamp(horiz * MaxSpeed * Time.fixedDeltaTime, -MaxSpeed, MaxSpeed),
            vertVelocity);

        m_Rigidbody2D.MovePosition(
            new Vector2(m_Rigidbody2D.transform.position.x, m_Rigidbody2D.transform.position.y) + Velocity);
    }

    // Update is called once per frame
    void Update()
    {
        jump = Input.GetButtonDown("Jump");
    }
}