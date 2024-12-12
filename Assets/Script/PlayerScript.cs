using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour 
{
    /* Define the max speed of the player */
    public float maxSpeed = 2.0f;
    /* Define the jumpforce of the player */
    public float jumpForce = 10f;
    /* LayerMask to now which is the floor */
    public LayerMask layerMask;
    /* Value to know if the player is on the ground */
    public Boolean isGrounded = false;
    /* Value to use for the size of the ground check box */
    public Vector3 groundCheckSize = Vector3.zero;

    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    private PlayerInputAction playerInputActions;
    private InputAction movement;

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Player.Jump.Enable();

    }

    private void DoJump (InputAction.CallbackContext obj)
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        /* Horizontal direction */
        float horizontalDirection = movement.ReadValue<Vector2>().x ;
        rigidbody2D.linearVelocity = new Vector2(horizontalDirection * maxSpeed * Time.deltaTime, rigidbody2D.linearVelocityY);
        Flip(horizontalDirection);
    }

    private void Flip(float horizontalDirection)
    {
        Debug.Log("Horizontal velocity " +  horizontalDirection);
        if(horizontalDirection < - 0.1f) 
            spriteRenderer.flipX = true;
        else if(horizontalDirection > 0.1f)
            spriteRenderer.flipX = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetType() == typeof(TilemapCollider2D))
        {
            isGrounded = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(TilemapCollider2D))
        {
            isGrounded = false;
        }
    }
}
