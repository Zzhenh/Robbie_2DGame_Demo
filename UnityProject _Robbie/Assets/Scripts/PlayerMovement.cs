using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("运动参数")]
    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")]
    public float jumpForce = 6.4f;
    public float jumpHolderForce = 2f;
    public float jumpHolderDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15f;

    float jumpTime;

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadLocked;
    public bool isHanging;

    [Header("环境监测")]
    public LayerMask groundLayer;
    public float footOffset = 0.4f;
    public float reachOffset = 0.7f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    public float grapDistance = 0.4f;
    float playerHeight;
    public float eyeHeight = 1.5f;

    public float xVelocity;

    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    bool jumpPressed;
    bool jumpHeld;
    bool crouchPressed;
    bool crouchHeld;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;

        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }

    void Update()
    {
        if (GameManager.GameOver())
        {
            return;
        }

        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchPressed = Input.GetButtonDown("Crouch");
        crouchHeld = Input.GetButton("Crouch");
    }

    private void FixedUpdate()
    {
        if (GameManager.GameOver())
        {
            return;
        }

        PhysicsCheck();

        GroundMovement();

        MidAirMovement();
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }

        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);

        if (headCheck)
        {
            isHeadLocked = true;
        }
        else
        {
            isHeadLocked = false;
        }

        float direction = transform.localScale.x;
        Vector2 grapDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grapDir, grapDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grapDir, grapDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grapDistance, groundLayer);

        if (!isOnGround && rb.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;

            rb.bodyType = RigidbodyType2D.Static;

            isHanging = true;
        }
    }

    void GroundMovement()
    {
        if (isHanging)
        {
            return;
        }
        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }
        else if(!crouchHeld && isCrouch && !isHeadLocked)
        {
            StandUp();
        }
        else if(!isOnGround && isCrouch)
        {
            StandUp();
        }

        xVelocity = Input.GetAxis("Horizontal");

        if (isCrouch)
        {
            xVelocity /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FlipDirection();
    }

    void FlipDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Crouch()
    {
        isCrouch = true;

        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()
    {
        isCrouch = false;

        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                
                isHanging = false;
            }
        }
        if (crouchPressed)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            isHanging = false;
        }
        if (jumpPressed && isOnGround && !isJump && !isHeadLocked)
        {
            if (isCrouch)
            {
                StandUp();

                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHolderDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            AudioManager.PlayJumpAudio();
        }
        else if(isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHolderForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {
                isJump = false;
            }
        }
    }

    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDirection,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color col = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length,col);

        return hit;
    }
}
