using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float healthTimer = 600f;
    
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float dashSpeed = 30f;

    [SerializeField]
    private float dashDuration = 0.2f;

    [SerializeField]
    private float dashCooldown = 1f;
    
    [SerializeField]
    private Rigidbody2D rigidBody;
    
    [SerializeField]
    private float fireRate = 0.5f;
    
    [SerializeField]
    private float bulletSpeed = 10f;
    
    [SerializeField]
    private int bulletDamage = 1;
    
    public GameObject bulletPrefab;
    
    /// <summary>
    /// The current movement input from the player. This is a Vector2 representing the direction and magnitude of the player's movement input.
    /// </summary>
    private Vector2 moveInput;

    private Vector2 lastMoveDir = Vector2.up;
    
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    
    private bool isFiring = false;
    private Vector2 shootDir = Vector2.zero;
    private float fireRateTimer = 0f;
    private int everyFiveSecondsTimer = 595;


    void Start()
    {
        // Cache the Rigidbody2D component attached to the player
        rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles player movement input from the Input System. This method is called automatically by the Input System when the player provides movement input.
    /// </summary>
    /// <param name="movementValue">The movement input value.</param>
    public void OnMove(InputValue movementValue)
    {
        // Get last frames movement Direction, only if it wasn't "Zero"
        // Default to up.
        if (moveInput.magnitude > 0f)
            lastMoveDir = moveInput;
        if (!isDashing)
        {
            moveInput = movementValue.Get<Vector2>().normalized;
            // Debug.Log("Move Input: " + moveInput);
        }
    }

    /// <summary>
    /// Handles player shooting input from the Input System. This method is called automatically by the Input System when the player provides shooting input.
    /// </summary>
    /// <param name="shootValue">The shooting input value.</param>
    public void OnShoot(InputValue shootValue)
    {
        Vector2 shootInput = shootValue.Get<Vector2>();
        
        // check to avoid double shooting when the input comes back to center.
        // Maybe adjust to some epsilon value for Joysticks?
        isFiring = shootInput.magnitude > 0f;
        if  (isFiring)
            shootDir = shootInput.normalized;
        // Debug.Log("Shoot Input: " + shootInput);
        // Implement shooting logic here
    }

    public void OnDash(InputValue dashValue)
    {
        if (dashValue.isPressed && !isDashing)
        {
            // Debug.Log("Dash Input: Pressed");
            isDashing = true;
            dashTimeRemaining = dashDuration;
        }
    }

    private void Update()
    {
        if (healthTimer <= everyFiveSecondsTimer)
        {
            EveryFiveSeconds();
            everyFiveSecondsTimer -= 1;
        }
        
        // Only fire if we are inputting to fire and the timer is 0. 
        if (isFiring)
        {
            if (fireRateTimer <= 0f)
            {
                Shoot(shootDir);
                fireRateTimer = fireRate;
            }
            fireRateTimer = Mathf.Clamp(fireRateTimer - Time.deltaTime, 0f, 1f);
        }
        
        // This needs a check to make sure the Room Challenge has begun
        // Otherwise it will constantly go down regardless.
        healthTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        rigidBody.linearVelocity = moveInput * (currentSpeed * Time.fixedDeltaTime);
        // Debug.Log("Player Velocity: " + rigidBody.linearVelocity);
        dashTimeRemaining -= Time.fixedDeltaTime;
        isDashing = isDashing && dashTimeRemaining > 0;
    }

    private void Shoot(Vector2 _shootDir, Vector3 offset = default(Vector3))
    {
        // Fire towards shootDir
        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
                 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(_shootDir * bulletSpeed,  ForceMode2D.Impulse);

        bullet.TryGetComponent(out BulletController bulletController);
        bulletController.SetDamage(bulletDamage);
    }
    
    private void EveryFiveSeconds()
    {
        // Debug.Log("EveryFiveSeconds Triggered: " + everyFiveSecondsTimer);
        /*
        // Implementation of Carmine Rook - Cardinal Shooting
        Shoot(Vector2.up);
        Shoot(Vector2.down);
        Shoot(Vector2.left);
        Shoot(Vector2.right);
        
        // Implementation of Carmine Bishop - Inter-Cardinal Shooting
        Shoot((Vector2.up + Vector2.right).normalized);
        Shoot((Vector2.up + Vector2.left).normalized);
        Shoot((Vector2.down + Vector2.right).normalized);
        Shoot((Vector2.down + Vector2.left).normalized);
        */
        // Implementations of Carmine Knight - + shape in direction shooting/facing
        Vector2 carmineKnightDir = Vector2.up;
        if (shootDir.magnitude > 0f)
            carmineKnightDir = shootDir;
        Shoot(carmineKnightDir, new Vector3(0.5f, 0.5f));
        Shoot(carmineKnightDir, new Vector3(-0.5f, 0.5f));
        Shoot(carmineKnightDir);
        Shoot(carmineKnightDir,  new Vector3(0, 0.5f));
        Shoot(carmineKnightDir,  new Vector3(0, 1f));

    }
}
