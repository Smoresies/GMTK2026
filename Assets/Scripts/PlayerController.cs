using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float dashSpeed = 30f;

    [SerializeField]
    private float dashDuration = 0.2f;

    [SerializeField]
    private float dashCooldown = 1f;
    
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
    
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    
    private bool isFiring = false;
    private Vector2 shootDir = Vector2.zero;
    private float fireRateTimer = 0f;


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
        
        // Only fire if we are inputting to fire and the timer is 0. 
        if (isFiring)
        {
            if (fireRateTimer <= 0f)
            {
                Shoot();
                fireRateTimer = fireRate;
            }
            fireRateTimer = Mathf.Clamp(fireRateTimer - Time.deltaTime, 0f, 1f);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        rigidBody.linearVelocity = moveInput * (currentSpeed * Time.fixedDeltaTime);
        // Debug.Log("Player Velocity: " + rigidBody.linearVelocity);
        dashTimeRemaining -= Time.fixedDeltaTime;
        isDashing = isDashing && dashTimeRemaining > 0;
    }

    private void Shoot()
    {
        // Fire towards shootDir
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDir * bulletSpeed,  ForceMode2D.Impulse);

        bullet.TryGetComponent(out BulletController bulletController);
        bulletController.SetDamage(bulletDamage);
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Player took " + damage + " damage");
    }
}
