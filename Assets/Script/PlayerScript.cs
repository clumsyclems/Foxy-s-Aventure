using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : Singleton<PlayerScript>
{
    /* Value to know if the player is on the ground */
    [SerializeField] private Boolean isGrounded = false;
    /* Boolean the set invicible the player */
    [SerializeField] private Boolean isInvicible = false;
    /* The Recovery time blinking duration */
    [SerializeField] private float recoveryTimeBlinkingDuration = 0.2f;
    /* The Recovery time */
    [SerializeField] private float recoveryTime = 0f;
    /* Define the max speed of the player */
    [SerializeField] private float maxSpeed = 2.0f;
    /* Define the jumpforce of the player */
    [SerializeField] private float jumpForce = 10f;
    /* LayerMask to now which is the floor */
    [SerializeField] private LayerMask floorLayerMask;
    /* Value to use for the size of the ground check box */
    [SerializeField] private Vector2 vector2GroundCheckSize = Vector3.zero;
    /* Transform Need to create a zone to know if the player is on the ground */
    [SerializeField] private Transform groundCheckTransform = null;

    /* Player RigidBody */
    [SerializeField] private new Rigidbody2D rigidbody2D = null;
    /* Player Sprite Renderer */
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    /* Input action value which take the player movement */
    [SerializeField] private InputAction movement = null;
    /* Player Input to switch commands according the type of screen */
    [SerializeField] private PlayerInput playerInput = null;
    /* Player Character Animator */
    [SerializeField] private Animator animator = null;

    /* Event to manage the player input from other game object */
    public static event System.Action<bool> OnControlToggle;

    protected override void Awake()
    {
        base.Awake();
        movement = playerInput.actions["Movement"];
    }

    /* Used to enable the binding Input Action */
    private void OnEnable()
    { 
        /* Enable movement player input */
        movement.Enable();

        /* Define and enable jump player input */
        playerInput.actions["Jump"].performed += DoJump;
        playerInput.actions["Jump"].Enable();

        /* Add the hability to other object to enable and disable player input */
        OnControlToggle += ToggleControls;
    }

    /* Used to disable the binding Input Action */
    private void OnDisable()
    {
        /* Disable movement player input */
        movement?.Disable();

        /* remove and disable jump player input */
        if (playerInput != null)
        {
            playerInput.actions["Jump"].performed -= DoJump;
            playerInput.actions["Jump"].Disable();
        }

        /* Remove the hability to other object to enable and disable player input */

        OnControlToggle -= ToggleControls;
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
        isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, vector2GroundCheckSize, 0, floorLayerMask);
        animator.SetBool("IsGrounded", isGrounded);

    }

    /* Draw Gizmo here the groundcheck verification area */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, vector2GroundCheckSize);
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
            case "WeakSpot":
            case "SceneBorder":
                break;

            case "Hurt":
                TakeDamage(1);
                break;

            case "Enemy":
                TakeDamage(1);
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

    /* Player jump function */
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    /* Function to the player can take the damage */
    private void TakeDamage(int damage)
    { 
        if(!isInvicible)
        {
            Inventory.Instance.UpdateHeart(-damage);
            StartCoroutine(HandleInvisibleDelay(recoveryTime));
            StartCoroutine(TakeDamageVisualAnimation());    
        }
    }

    /* Coroutine to enable TakeDamage visual animation (Sprite Flicking) */ 
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

    /* Coroutine to enable the hability to the player to be Invisible */
    private IEnumerator HandleInvisibleDelay(float delay)
    {
        isInvicible = true;
        yield return new WaitForSeconds(delay);
        isInvicible = false;
    }

    /* Function to change the player input status (Enable/Disable) */
    private void ToggleControls(bool state)
    {
        playerInput.enabled = state;
    }

    /* Public function to triggered the event to enable or disable the player input */
    public static void TriggerToggleControls(bool state)
    {
        OnControlToggle?.Invoke(state);
    }
}
