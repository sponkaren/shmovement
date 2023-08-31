using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovemetState {idle,running,jumping,falling}
    private MovemetState state;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown ("Jump") && (int)state<(int)MovemetState.jumping) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {              
        if (dirX > 0f)
        {
            state = MovemetState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovemetState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovemetState.idle;
        }

        
        if(rb.velocity.y > .01f)
        {
            state = MovemetState.jumping;
        }
        else if(rb.velocity.y < -.01f) 
        {
            state = MovemetState.falling;
        }
        

        anim.SetInteger("state", (int)state);
    }
}
