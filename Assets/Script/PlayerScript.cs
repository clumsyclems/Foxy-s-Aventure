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
    public LayerMask floorLayerMask;
    /* Value to know if the player is on the ground */
    public Boolean isGrounded = false;
    /* Value to use for the size of the ground check box */
    public Vector3 vector3GroundCheckSize = Vector3.zero;
    /* Transform Need to create a zone to know if the player is on the ground */
    public Transform groundCheckTransform = null;

    private new Rigidbody2D rigidbody2D = null;
    private SpriteRenderer spriteRenderer = null;

    private PlayerInputAction playerInputActions = null;
    private InputAction movement = null;
    private Animator animator = null;

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
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        /* Manage Vertical direction */
        animator.SetFloat("VerticalVelocity", rigidbody2D.linearVelocityY);
        /* Manage Horizontal direction */
        float horizontalDirection = movement.ReadValue<Vector2>().x;
        animator.SetFloat("Speed", MathF.Abs(horizontalDirection));
        rigidbody2D.linearVelocity = new Vector2(horizontalDirection * maxSpeed * Time.deltaTime, rigidbody2D.linearVelocityY);
        Flip(horizontalDirection);
        /* grounded verification */
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, vector3GroundCheckSize, 0, floorLayerMask);
        animator.SetBool("IsGrounded", isGrounded);


    }

    private void Flip(float horizontalDirection)
    {
        if(horizontalDirection < - 0.1f) 
            spriteRenderer.flipX = true;
        else if(horizontalDirection > 0.1f)
            spriteRenderer.flipX = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, vector3GroundCheckSize);
    }
}
