using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
    /* Define the max speed of the player */
    public float maxSpeed = 2.0f;
    /* Define the jumpforce of the player */
    public float jumpForce = 10f;
    /* Transform used to verify if the player is on the ground or not */
    public Transform groundCheck;
    /* LayerMask to now which is the floor */
    public LayerMask layerMask;
    /* Value to know if the player is on the ground */
    public Boolean isGrounded = false;

    public float groundCheckRadius = 0f;

    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        /* Horizontal direction */
        float horizontalDirection = Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime;
        rigidbody2D.linearVelocity = new Vector2(horizontalDirection, rigidbody2D.linearVelocityY);
        Flip(horizontalDirection);
    }

    void Flip(float horizontalDirection)
    {
        spriteRenderer.flipX = (horizontalDirection < 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
