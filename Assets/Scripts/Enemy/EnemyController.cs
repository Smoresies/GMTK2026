using System;
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
    [SerializeField]
    protected int damage = 5;
    [SerializeField]
    private AudioSource hurtSFX;
    [SerializeField]
    private GameObject spawnSmoke;

    protected PlayerController player;
    public Action OnDeathEvent;

    protected virtual void Start()
    {
        // Cache the Rigidbody2D component attached to the player
        rigidBody = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerController>();
        movementAi.initialize(player.transform, transform);
        ParticleSystem ps = Instantiate(spawnSmoke, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        // spawnSmoke.GetComponent<ParticleSystem>().Play();
        Debug.Log("Should be spawning particles");
    }
    public virtual void TakeDamage(float damage)
    {
        hurtSFX.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        hurtSFX.Play();
        health -= damage;
        if (health <= 0)
        {
            if (player.hasChronoSword && player.chronoSwordCD <= 0f)
            {
                player.freezeTime();
                if(player.hasTrickstersDeck)
                    player.freezeTime();
                player.chronoSwordCD = player.relicCDs;
            }
            
            // Eventually add some like. Art/effect here
            if (player.hasBottledRage)
            {
                GameObject explo = Instantiate(player.ExplosionPrefab, transform.position, transform.rotation);
                explo.GetComponent<ExplosionManager>().SetDamage(player.BulletDamage * 0.5f);
            }
                
            Die();
        }
            
    }

    protected void Die()
    {
        OnDeathEvent?.Invoke();
        Destroy(gameObject);
    }

    public void Curse2()
    {
        health *= 1.5f;
        damage *= 2;
    }
    
    protected virtual void FixedUpdate()
    {
        rigidBody.linearVelocity = movementAi.GetMoveDirection() * moveSpeed * Time.fixedDeltaTime;
    }
}
