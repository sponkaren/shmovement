using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float doubleJumpForce = 14f;
    [SerializeField] private int doubleJumpsCount = 1;
    [SerializeField] private int doubleJumpsLeft;

    private enum MovementState {idle,running,jumping,falling}
    private MovementState state;
    private MovementState previousState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        doubleJumpsLeft = doubleJumpsCount;
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown ("Jump")) 
        {
            if(IsGrounded())
            {
                Debug.Log("Grounded jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if(doubleJumpsLeft > 0)
            {
                Debug.Log("Double Jumping!");
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                doubleJumpsLeft--;
            }        
        }
        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {   
        previousState = state;

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
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.01f) 
        {
            state = MovementState.falling;
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
}
