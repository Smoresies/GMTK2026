using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float healthTimer = 600f;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float dashSpeed = 30f;

    [SerializeField] private float dashDuration = 0.2f;

    [SerializeField] private float dashCooldown = 1f;

    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private int bulletDamage = 1;
    
    public int BulletDamage => bulletDamage;
    
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bombPrefab;

    public GameObject ExplosionPrefab => explosionPrefab;

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
    private float everyFiveSecondsTimer = 5.0f;
    private float slownessTimer = 0.0f;

    public float relicCDs { get; private set; } = 5.0f;

    private float frozenTimeTimer = 0.0f;
    private float chronoBootsCD = 0.0f;
    public float chronoCharmCD = 0.0f;
    public float chronoShieldCD = 0.0f;
    public float chronoSwordCD = 0.0f;

    // This is the crit-related stuff
    private float critRate = 0.1f;
    private float critDamage = 1.5f;

    // Used with Flower of the Old One
    private float timeRate = 0.8f;

    /// <summary>
    /// THIS IS THE LIST OF ALL OF THE RELIC BOOLEANS
    /// </summary>
    private bool carmineKnight = false;

    private bool carmineRook = false;
    private bool carmineBishop = false;
    private bool chronomancersBoots = false;
    private bool chronomachersShield = false;
    private bool edPills = false;
    private bool bombBelt = false;
    private bool hasDubiousEnergy = false;
    private bool hasFotOO = false;
    private bool hasQuill = false;
    
    // Needed externally for Bullets
    public bool hasRippedClover { get; private set; } = false;
    public bool hasWeightedDie { get; private set; } = false;
    public bool hasChronoCharm { get; private set; } = false;
    public bool hasChronoSword { get; private set; } = false;
    
    public bool hasBottledRage { get; private set; } = false;
    public bool hasLightningCharm { get; private set; } = false;

    // Needed just... fucking... everywhere.
    public bool hasTrickstersDeck { get; private set; } = false;
    public bool hasMembership { get; private set; } = false;
    public bool hasTongue { get; private set; } = false;

    // Temporal Paradoxes for Dummies
    private bool TPfD = false;
    
    
    // CURSES
    public bool curse2 { get; private set; } = false;
    public bool curse3 { get; private set; } = false;
    private bool curse4 = false;
    private bool curse5 = false;
    private bool curse6 = false;
    public bool curse7 { get; private set; } = false;
    private bool curse8 = false;
    private bool curse9 = false;
    public bool curse12 { get; private set; } = false;
    
    public bool curse16 { get; private set; } = false;
    
    public LevelManager LevelManager;

    // Audio Clips
    [SerializeField]
    private AudioSource dashSFX;
    [SerializeField]
    private AudioSource hurtSFX;
    [SerializeField]
    private AudioSource shootingSFX;
    [SerializeField]
    private AudioSource walkingSFX; //Iffy on, not implemented


    void Start()
    {
        // Cache the Rigidbody2D component attached to the player
        rigidBody = GetComponent<Rigidbody2D>();

        // bombBelt = true;
        // hasDubiousEnergy = true;
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
        if (isFiring)
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
            dashingRelics();
            dashTimeRemaining = dashDuration;
            dashSFX.pitch = Random.Range(0.9f, 1.1f);
            dashSFX.Play();

            if (curse4)
                slownessTimer = 1.0f;
        }
    }

    private void Update()
    {
        //Debug.Log(healthTimer);
        
        if (everyFiveSecondsTimer <= 0f)
        {
            EveryFiveSeconds();
            everyFiveSecondsTimer = 5f;
        }
        // Only fire if we are inputting to fire and the timer is 0. 
        if (isFiring)
        {
            if (fireRateTimer <= 0f)
            {
                Shoot(shootDir);
                
                // BOX OF CANDIED FINGERS IMPLEMENTED HERE
                fireRateTimer = fireRate;
            }

            fireRateTimer = Mathf.Clamp(fireRateTimer - Time.deltaTime, 0f, 1f);
        }

        // This needs a check to make sure the Room Challenge has begun
        // Otherwise it will constantly go down regardless.
        if (frozenTimeTimer <= 0.0f)
        {
            healthTimer -= (Time.deltaTime * (hasFotOO ? timeRate : 1.0f)) * 
                                    ((curse5 && moveInput.magnitude == 0 && !isDashing) ? 2.0f : 1.0f) *
                                    ((hasQuill) ? 2.0f : 1.0f);
            everyFiveSecondsTimer -= Time.deltaTime;
        }
            
        else
            frozenTimeTimer -= Time.deltaTime;
        relicCooldowns();
    }

    private void FixedUpdate()
    {
        // Adds the ability to be "slowed"
        float currentSpeed = (isDashing ? dashSpeed : moveSpeed) * (slownessTimer > 0f ? 0.25f : 1.0f);
        rigidBody.linearVelocity = moveInput * (currentSpeed * Time.fixedDeltaTime);
        // Debug.Log("Player Velocity: " + rigidBody.linearVelocity);
        dashTimeRemaining -= Time.fixedDeltaTime;
        isDashing = isDashing && dashTimeRemaining > 0;
    }

    public void Shoot(Vector2 _shootDir, Vector3 offset = default(Vector3))
    {
        shootingSFX.pitch = Random.Range(0.9f, 1.1f);
        shootingSFX.Play();

        // Fire towards shootDir
        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(_shootDir * (bulletSpeed * (curse6 ? 0.5f : 1.0f)), ForceMode2D.Impulse);

        bullet.TryGetComponent(out BulletController bulletController);
        bulletController.SetDamage(bulletDamage, critRate, critDamage);

        if (edPills)
        {
            bullet.transform.localScale *= 3f;
            if (hasTrickstersDeck)
                bullet.transform.localScale *= 3f;
        }
    }
    /*
    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Player took " + damage + " damage");
    }*/

    public Rigidbody2D GetRigidbody()
    {
        return rigidBody;
    }


    private void EveryFiveSeconds()
    {
        // Debug.Log("EveryFiveSeconds Triggered: " + everyFiveSecondsTimer);
        int repeats = hasTrickstersDeck ? 1 : 2;

        // Every 5 seconds is now skipped. We can go back to main loop and also do an extra decrement.
        if (curse9)
        {
            healthTimer -= 1;
            return;
        }
        
        if (curse8)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
            bomb.GetComponent<Bomb>()._damage = bulletDamage;
        }

        

        for (int i = 0; i < repeats; ++i)
        {
            // Implementation of Carmine Rook - Cardinal Shooting
            if (carmineRook)
            {
                Shoot(Vector2.up);
                Shoot(Vector2.down);
                Shoot(Vector2.left);
                Shoot(Vector2.right);
            }

            // Implementation of Carmine Bishop - Inter-Cardinal Shooting
            if (carmineBishop)
            {
                Shoot((Vector2.up + Vector2.right).normalized);
                Shoot((Vector2.up + Vector2.left).normalized);
                Shoot((Vector2.down + Vector2.right).normalized);
                Shoot((Vector2.down + Vector2.left).normalized);
            }

            // Implementations of Carmine Knight - + shape in direction shooting/facing
            if (carmineKnight)
            {
                Vector2 carmineKnightDir = Vector2.up;
                if (shootDir.magnitude > 0f)
                    carmineKnightDir = shootDir;
                Shoot(carmineKnightDir, new Vector3(0.5f, 0.5f));
                Shoot(carmineKnightDir, new Vector3(-0.5f, 0.5f));
                Shoot(carmineKnightDir);
                Shoot(carmineKnightDir, new Vector3(0, 0.5f));
                Shoot(carmineKnightDir, new Vector3(0, 1f));
            }
            
            if (hasDubiousEnergy)
            {
                GameObject explo = Instantiate(explosionPrefab, transform.position, transform.rotation);
                explo.GetComponent<ExplosionManager>().SetDamage(bulletDamage * 0.5f);
            }
        }
    }

    private void dashingRelics()
    {
        if (!isDashing)
            return;
        
        // Implementation for Chronomancer's Boots. Freeze for 0.5s
        if (chronomancersBoots && frozenTimeTimer > 0f && chronoBootsCD <= 0f)
        {
            chronoBootsCD = relicCDs;
            freezeTime();
            if (hasTrickstersDeck)
                freezeTime();
        }

        if (bombBelt)
        {
            GameObject explo = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explo.GetComponent<ExplosionManager>().SetDamage(bulletDamage * 0.5f);
        }
    }

    public void freezeTime()
    {
        // Specifically set for "double trigger" opportunities.
        if (frozenTimeTimer > 0f)
            frozenTimeTimer += 1.0f;
        else
            frozenTimeTimer = 1.0f;

        if (TPfD)
            frozenTimeTimer *= 2.0f;
    }

    private void relicCooldowns()
    {
        chronoBootsCD -= Time.deltaTime;
        chronoCharmCD -= Time.deltaTime;
        chronoShieldCD -= Time.deltaTime;
        chronoSwordCD -= Time.deltaTime;
        slownessTimer -= Time.deltaTime;
    }

    // Call this when the player gets the Wizard Coke Relic.
    public void wizardCoke()
    {
        fireRate /= 2f;
    }

    // This needs to be called when you receive the Infinite Edges Charm
    public void CritUpdate(bool infEdges = false)
    {
        critRate += 0.1f;
        if (infEdges)
            critDamage = 2.0f;
    }

    
    public void TakeDamage(float damage)
    {
        if (hasFotOO)
            timeRate += 0.1f;
        else
        {
            healthTimer -= damage;
            hurtSFX.pitch = Random.Range(0.9f, 1.1f);
            hurtSFX.Play();
        }
        
        if (chronomachersShield && chronoShieldCD <= 0.0f)
        {
            freezeTime();
            if(hasTrickstersDeck)
                freezeTime();
            chronoShieldCD = relicCDs;
        }
            
        Debug.Log(healthTimer);
        if (healthTimer <= 0)
        {
            // Eventually add some like. Art/effect here
            Destroy(gameObject);
        }

    }

    public void Capitalism(float damage)
    {
        healthTimer -= damage;
    }

    public void OnDebug(InputValue inputValue)
    {
        Debug.Log("Debug key pressed in player, completing level");
        LevelManager.OnDebug(inputValue);
    }

    public void AddCurse(Curse curse)
    {
        switch (curse.curseIdentifier)
        {
            case 2:
                curse2 = true;
                break;
            case 3:
                curse3 = true;
                break;
            case 4:
                curse4 = true;
                break;
            case 5:
                curse5 = true;
                break;
            case 6:
                curse6 = true;
                break;
            case 7:
                curse7 = true;
                break;
            case 8:
                curse8 = true;
                break;
            case 9:
                curse9 = true;
                break;
            case 12:
                curse12 = true;
                break;
            case 16:
                curse16 = true;
                break;
            default:
                break;
        }
    }

    public void AddRelic(Relic relic)
    {
        switch (relic.relicName)
        {
            case "Carmine Knight":
                carmineKnight = true;
                break;
            case "Carmine Rook":
                carmineRook = true;
                break;
            case "Carmine Bishop":
                carmineBishop = true;
                break;
            case "Dubious Energy Tonic":
                hasDubiousEnergy = true;
                break;
            case "Evocation Dysfuntion Pills":
                edPills = true;
                break;
            case "Bag of Wizard Coke":
                wizardCoke();
                break;
            case "Chronomancer's Shield":
                chronomachersShield = true;
                break;
            case "Chronomancer's Boots":
                chronomancersBoots = true;
                break;
            case "Chronomancer's Blade":
                hasChronoSword = true;
                break;
            case "Temporal Paradoxes for Dummies":
                TPfD = true;
                break;
            case "Infinite Edges Charm":
                CritUpdate(true);
                break;
            case "Lightning Charm":
                CritUpdate();
                hasLightningCharm = true;
                break;
            case "Chronomancer's Charm":
                hasChronoCharm = true;
                break;
            case "Bombardier's Belt":
                bombBelt = true;
                break;
            case "Bloodstained Membership Card":
                hasMembership = true;
                break;
            case "Tongue of Karen":
                hasTongue = true;
                break;
            case "Flower of the Old One":
                hasFotOO = true;
                break;
            case "Bottled Rage":
                hasBottledRage = true;
                break;
            case "Trickster's Deck":
                hasTrickstersDeck = true;
                break;
            case "Ripped Clover Charm":
                CritUpdate();
                CritUpdate();
                hasRippedClover = true;
                break;
            case "Cerulean Quill":
                fireRate *= 1.5f;
                moveSpeed *= 1.5f;
                hasQuill = true;
                break;
            case "Weighted Die":
                hasWeightedDie = true;
                break;
            default:
                Debug.Log("Shit's Fucked " + relic.relicName);
                break;
        }
    }
}

