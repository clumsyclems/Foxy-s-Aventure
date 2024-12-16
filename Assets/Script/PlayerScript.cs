using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    /* Value to know if the player is on the ground */
    public Boolean isGrounded = false;
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
    /* Gem Text to modify */
    public TextMeshProUGUI numberOfGemText = null;
    /* The pause menu panel */
    public Image pauseMenu = null;
    /* Canvas which use CanvasScript too modify info on the canvas */
    public CanvasScript canvas = null;

    /* Player RigidBody */
    private new Rigidbody2D rigidbody2D = null;
    /* Player Sprite Renderer */
    private SpriteRenderer spriteRenderer = null;
    /* Player Input Action Class*/
    private PlayerInputAction playerInputActions = null;
    /* Input action value which take the player movement */
    private InputAction movement = null;
    /* Player Character Animator */
    private Animator animator = null;
    /* the Player number of heart */
    private int numberHeart = 3;
    /* the Player number of gems collected */
    private int numberGems = 0;

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
    }

    /* Used to enable the binding Input Action */
    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Player.Jump.Enable();

        playerInputActions.Player.Pause.performed += ChangePauseMenuStatus;
        playerInputActions.Player.Pause.Enable();

    }

    /* Used to disable the binding Input Action */
    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Pause.Disable();
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        canvas.AddHeart(this.numberHeart);
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

    /* Veeify the player collision with other trigerred collider */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* If the object collide have the tag "Gem" detroy it */

        switch (collision.gameObject.tag)
        {
            case "Gem":
                numberGems++;
                numberOfGemText.text = numberGems.ToString();
                Destroy(collision.gameObject);
                break;

            case "EndLevel":
                SceneManager.LoadScene("SampleScene 1");
                break;

            case "Hurt":
                canvas.RemoveHeart(1);
                break;

            case "Enemy":
                canvas.RemoveHeart(1);
                --numberHeart;
                break;

            case "Heal":
                canvas.AddHeart(1);
                ++numberHeart;
                break;

            default:
                Debug.Log("Nothing to do for tag : "+collision.gameObject.tag);
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

    /* Activate/desactivate pause menu */
    private void ChangePauseMenuStatus(InputAction.CallbackContext obj)
    {
        /* Change the pause menu status from enable to disable and inverse */
        pauseMenu.enabled = !pauseMenu.enabled;
        /* If the pauseMenu is enable the game stop and if it is disable the game start */
        Time.timeScale = pauseMenu.enabled ? 0f : 1f;
    }

    public void SetNumberOfHeart(int numberOfHeart)
    { 
        this.numberHeart = numberOfHeart;
    }
    public int GetNumberHeart()
    {
        return this.numberHeart;
    }
}
