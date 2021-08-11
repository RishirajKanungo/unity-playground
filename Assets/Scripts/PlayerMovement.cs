using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D collision;
    private Animator animator;
    [SerializeField] private LayerMask jumpableGround;
    
    private enum MovementState{idle, walking, running, jumping, basic_attack, basic_attack2, basic_attack3 }
    // checks whether left or right (-1,1)
    private float dirX = 0f;
    private SpriteRenderer sprite;
    
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float moveSpeed = 3f;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); //allows for joystick support

        // Jumping
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }

        UpdateAnimationState();


        
    }

    private void UpdateAnimationState()
    {
        // state used to determine what movement being used
        MovementState state;

        // walking logic
        if (dirX > 0f)
        {
            state = MovementState.walking;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.walking;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // in the air (when jumping)
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }

        animator.SetInteger("state", (int)state); // cast enum into int
    }

    // check if we land on terrain
    private bool IsGrounded()
    {
        //box cast
        return Physics2D.BoxCast(collision.bounds.center, collision.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

}
