using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    protected float health;
    protected Rigidbody2D rigidBody;
    [SerializeField] 
    protected MovementAi movementAi;
    [SerializeField]
    protected float moveSpeed = 5f;

    protected PlayerController player;

    protected virtual void Start()
    {
        // Cache the Rigidbody2D component attached to the player
        rigidBody = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerController>();
        movementAi.initialize(player.transform, transform);
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Eventually add some like. Art/effect here
            Destroy(gameObject);
        }
            
    }

    
    protected virtual void FixedUpdate()
    {
        rigidBody.linearVelocity = movementAi.GetMoveDirection() * moveSpeed * Time.fixedDeltaTime;
        // Debug.Log("Player Velocity: " + rigidBody.linearVelocity);
    }
}
