using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Player>(out var player))
            {
                //player.SetGrounded(true);
                //player.SetAirborne(false);
                //player.SetJumped(false);
                //player.SetDoubleJumped(false);
            }
        }
    }
}
