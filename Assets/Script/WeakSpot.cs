using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    /* Define the jumpforce to apply to the rigidbody2D */
    [SerializeField] private float bounceForce = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
        if(collision.gameObject.CompareTag("Player") && rigidbody2D.linearVelocityY < 0.01)
        {
            rigidbody2D.AddForce(bounceForce * Vector2.up);
            Destroy(transform.parent.gameObject.transform.parent.gameObject);
        }
    }

}
