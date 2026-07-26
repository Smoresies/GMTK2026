using UnityEngine;

public class MagicPewPewCultistEnemy : EnemyController
{
    [SerializeField]
    private float timeBetweenShots = 1f;

    [SerializeField]
    private float fireRange;

    private float timeBeforeNextShot = 0f;
    public GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 10f;
    
    [SerializeField]
    private int bulletDamage = 1;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        timeBeforeNextShot -= Time.fixedDeltaTime;
        // Debug.Log("Time before next shot: " + timeBeforeNextShot);
        if (timeBeforeNextShot < 0f && (rigidBody.position - player.GetRigidbody().position).magnitude < fireRange)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        timeBeforeNextShot = timeBetweenShots;
        // Fire towards shootDir
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce((player.GetRigidbody().position - rigidBody.position).normalized * bulletSpeed,  ForceMode2D.Impulse);

        bullet.TryGetComponent(out BulletController bulletController);
        bulletController.SetDamage(bulletDamage);
    }
}
