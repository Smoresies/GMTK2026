using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float moveSpeed = 5f;
    void Start()
    {
        // Cache the Rigidbody2D component attached to the player
        rigidBody = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            // Eventually add some like. Art/effect here
            Destroy(gameObject);
        }
            
    }

    
    private void FixedUpdate()
    {
        rigidBody.linearVelocity = Vector2.zero * moveSpeed * Time.fixedDeltaTime;
        // Debug.Log("Player Velocity: " + rigidBody.linearVelocity);
    }
}
