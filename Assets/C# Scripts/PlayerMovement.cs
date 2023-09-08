using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private float dirY = 0f;
    
    [SerializeField] private float moveSpeed = 7f;
    
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float doubleJumpForce = 14f;
    [SerializeField] private int doubleJumpsCount = 1;
    [SerializeField] private int doubleJumpsLeft;
    
    [SerializeField] private float dashForceX = 20f;
    [SerializeField] private float dashForceY = 5f;
    [SerializeField] private int dashCount = 1;
    [SerializeField] private int dashesLeft;
    [SerializeField] private float dashDuration = 0.6f;

    private enum MovementState {idle,running,jumping,falling,doubleJumping,dashing}
    private MovementState state;
    private MovementState previousState;
    private bool isDashing;
    private bool isDashAnimating;
    private float currentDashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        doubleJumpsLeft = doubleJumpsCount;
        dashesLeft = dashCount;
        isDashing = false;
        isDashAnimating = false;
        currentDashTime = 0f;
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        if (!isDashing)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }

        if (Input.GetButtonDown ("Jump")) 
        {
            if(IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if(doubleJumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                doubleJumpsLeft--;
            }        
        }

       
        if(Time.time > currentDashTime)
        {
            if (Input.GetButtonDown ("Dash") && dashesLeft > 0 && !isDashing)
            {
                isDashing = true;
                dashesLeft--;                
                rb.velocity = new Vector2(dirX * dashForceX, dirY *dashForceY);                
                currentDashTime = Time.time + dashDuration;
            }
            else if(isDashing)
            {
                StopDashing();
            }
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {   
        previousState = state;                

        if(!isDashAnimating)
        {
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                sprite.flipX = true;
            }
            else
            {
                state = MovementState.idle;
            }
   
            if(rb.velocity.y > .01f)
            {
                if (doubleJumpsLeft > 0) state = MovementState.jumping;
                else state = MovementState.doubleJumping;
            }
            else if(rb.velocity.y < -.01f) 
            {
                state = MovementState.falling;
            }
        }
        
        if(isDashing && !isDashAnimating) 
        {
            anim.SetBool("dashing", isDashing);
            state = MovementState.dashing;
            isDashAnimating = true;
            resetDash();
        }
        
        if(!isDashing && isDashAnimating)
        {
            anim.SetBool("dashing", isDashing);
            isDashAnimating = false;
        }

        anim.SetInteger("state", (int)state);

        if((int)previousState > (int)MovementState.running && (int)state < (int)MovementState.jumping)
        {
            if(IsGrounded())
            {
                resetDoubleJumps();                
            }
        }        
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size- new Vector3(.1f,.1f,.1f), 0f, Vector2.down, .2f, jumpableGround);
    }

    private void resetDoubleJumps()
    {
        doubleJumpsLeft = doubleJumpsCount;
    }

    private void resetDash()
    {
        Debug.Log("Reset dash");
        dashesLeft = dashCount;
    }

    private void StopDashing()
    {
        isDashing = false;
    }
}
