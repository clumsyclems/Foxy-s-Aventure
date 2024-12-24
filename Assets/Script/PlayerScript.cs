using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    /* Value to know if the player is on the ground */
    public Boolean isGrounded = false;
    /* Boolean the set invicible the player */
    public Boolean isInvicible = false;
    /* The Recovery time blinking duration */
    public float recoveryTimeBlinkingDuration = 0.2f;
    /* The Recovery time */
    public float recoveryTime = 0f;
    /* Define the max speed of the player */
    public float maxSpeed = 2.0f;
    /* Define the jumpforce of the player */
    public float jumpForce = 10f;
    /* LayerMask to now which is the floor */
    public LayerMask floorLayerMask;
    /* Value to use for the size of the ground check box */
    public Vector3 vector3GroundCheckSize = Vector3.zero;
    /* Transform Need to create a zone to know if the player is on the ground */
    public Transform groundCheckTransform = null;

    /* Player RigidBody */
    private new Rigidbody2D rigidbody2D = null;
    /* Player Sprite Renderer */
    private SpriteRenderer spriteRenderer = null;
    /* Input action value which take the player movement */
    private InputAction movement = null;
    /* Player Input to switch commands according the type of screen */
    private PlayerInput playerInput = null;
    /* Player Character Animator */
    private Animator animator = null;
    /* Pause Menu Script */
    public PauseMenuScript pauseMenuScript = null;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        movement = playerInput.actions["Movement"];
    }

    /* Used to enable the binding Input Action */
    private void OnEnable()
    { 
        movement.Enable();

        playerInput.actions["Jump"].performed += DoJump;
        playerInput.actions["Jump"].Enable();
    }

    /* Used to disable the binding Input Action */
    private void OnDisable()
    {
        movement.Disable();

        playerInput.actions["Jump"].performed -= DoJump;
        playerInput.actions["Jump"].Disable();
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    { 
        /* Manage Vertical direction */
        animator.SetFloat("VerticalVelocity", rigidbody2D.linearVelocityY);

        /**
         * Manage Horizontal direction 
         * get the input system info for the movement 
         */
        float horizontalDirection = movement.ReadValue<Vector2>().x;
        animator.SetFloat("Speed", MathF.Abs(horizontalDirection));
        rigidbody2D.linearVelocity = new Vector2(horizontalDirection * maxSpeed * Time.deltaTime, rigidbody2D.linearVelocityY);

        /* Verify if flip the player sprite is needed */
        Flip(horizontalDirection);

        /* grounded verification */
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, vector3GroundCheckSize, 0, floorLayerMask);
        animator.SetBool("IsGrounded", isGrounded);

    }

    /* Draw Gizmo here the groundcheck verification area */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, vector3GroundCheckSize);
    }

    /* Verify the player collision with other trigerred collider */
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        switch (collision.gameObject.tag)
        {
            case "Gem":
            case "Floor":
            case "EndLevel":
            case "Heal":
                break;

            case "Hurt":
                TakeDamage(1);
                break;

            case "Enemy":
                TakeDamage(1);
                break;

            case "WeakSpot":
                if(!isGrounded){rigidbody2D.AddForce(1.5f * jumpForce * Vector2.up);}
                break;

            default:
                Debug.Log("Nothing to do for tag : "+collision.gameObject.tag);
                break;
        }
    }

    /* Verify the Collision Presence between the player and other object */
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Hurt":
                TakeDamage(1);
                break;
            case "Enemy":
                TakeDamage(1);
                break;

            default:
                break;
        }
    }

    /* Verify the Collision between the player and other object */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                TakeDamage(1);
                break;

            case "Floor":
                break;

            default:
                Debug.Log("Nothing to do for : " + collision.gameObject.tag);
                break;
        }
    }

    /* Flip the player sprite when he goes in another direction */
    private void Flip(float horizontalDirection)
    {
        if (horizontalDirection < -0.1f)
            spriteRenderer.flipX = true;
        else if (horizontalDirection > 0.1f)
            spriteRenderer.flipX = false;
    }

    /* Player ump function */
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    private void TakeDamage(int damage)
    { 
        if(!isInvicible)
        {
            Inventory.instance.UpdateHeart(-damage);
            StartCoroutine(HandleInvisibleDelay(recoveryTime));
            StartCoroutine(TakeDamageVisualAnimation());    
        }
    }

    private IEnumerator TakeDamageVisualAnimation()
    {
        while (isInvicible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(recoveryTimeBlinkingDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(recoveryTimeBlinkingDuration);
        }

    }

    private IEnumerator HandleInvisibleDelay(float delay)
    {
        isInvicible = true;
        yield return new WaitForSeconds(delay);
        isInvicible = false;
    }
}
