using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float StartY { get; } = 2.4f;
    private float Speed { get; set; } = 1f;
    private float Knockback { get; set; } = 5f;

    private Collider2D c;

    void Start()
    {
        c = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            GameObject playerObject = collision.gameObject;
            Player player = playerObject.GetComponent<Player>();
            Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();

            if (playerObject.transform.position.y < transform.position.y)
            {
                playerRb.AddForce(new Vector2(0, 1) * (Knockback / 2), ForceMode2D.Impulse);
            } else
            {
                Destroy(gameObject);
            }
            if (playerRb != null)
            {

                playerRb.AddForce(knockbackDirection * Knockback, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Destroy(gameObject);
        }
    }
}