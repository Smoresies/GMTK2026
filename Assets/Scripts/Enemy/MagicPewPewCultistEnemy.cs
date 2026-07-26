using System.Collections.Generic;
using UnityEngine;

public class MagicPewPewCultistEnemy : EnemyController
{
    [SerializeField]
    protected MoveToPositionAI moveToPositionAi;
    [SerializeField]
    protected NoMovementAi noMovementAi;
    [SerializeField]
    private float timeBetweenDecisions = 1f;
    private float timeBeforeNextDecision = 0f;
    public GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private int bulletDamage = 1;
    [SerializeField]
    private List<WeightedObject<string>> weightedDecisions;
    [SerializeField]
    private float variableDecisionTime = .25f;

    [SerializeField]
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        moveToPositionAi.initialize(player.transform, transform);
        noMovementAi.initialize(player.transform, transform);
        initNewMovementAi(noMovementAi);
    }
    private void initNewMovementAi(MovementAi newMovementAi)
    {
        movementAi = newMovementAi;
        movementAi.initialize(player.transform, transform);
        Debug.Log(this.name + " is now using " + newMovementAi.GetType().Name);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        timeBeforeNextDecision -= Time.fixedDeltaTime;

        // Debug.Log("Time before next shot: " + timeBeforeNextShot);
        if (timeBeforeNextDecision < 0f)
        {
            timeBeforeNextDecision = timeBetweenDecisions + Random.Range(-variableDecisionTime, variableDecisionTime);
            switch(Utils.GetRandomWeightedObject(weightedDecisions).item)
            {
                case "shoot":
                    anim.SetBool("Moving", false);
                    FireBullet();
                    break;
                case "noMove":
                    anim.SetBool("Moving", false);
                    initNewMovementAi(noMovementAi);
                    break;
                case "move":
                    anim.SetBool("Moving", true);
                    initNewMovementAi(moveToPositionAi);
                    break;
                default:
                    Debug.LogError("You dun fucked up decicisons A-A RON: " + Utils.GetRandomWeightedObject(weightedDecisions).item);
                    break;
            }
        }
    }

    private void FireBullet()
    {
        // Fire towards shootDir
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce((player.GetRigidbody().position - rigidBody.position).normalized * bulletSpeed, ForceMode2D.Impulse);

        bullet.TryGetComponent(out BulletController bulletController);
        bulletController.SetDamage(bulletDamage);
    }
}
