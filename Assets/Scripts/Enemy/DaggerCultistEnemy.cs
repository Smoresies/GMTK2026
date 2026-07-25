using UnityEngine;

public class DaggerCultistEnemy : EnemyController
{
    [SerializeField]
    protected MoveToPositionAI moveToPositionAi;
    [SerializeField]
    protected NoMovementAi noMovementAi;
    [SerializeField]
    private float timeBetweenAttacks = 3f;
    private float timeBeforeNextAttack = 0f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private float distanceToPlayer;
    [SerializeField]
    private float attackDistance;
    protected override void Start()
    {
        base.Start();
        moveToPositionAi.initialize(player.transform, transform);
        noMovementAi.initialize(player.transform, transform);
        initNewMovementAi(moveToPositionAi);
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
        timeBeforeNextAttack -= Time.fixedDeltaTime;
        if (movementAi == moveToPositionAi && moveToPositionAi.IsWithinStopDistance())
        {
            initNewMovementAi(noMovementAi);
            timeBeforeNextAttack = timeBetweenAttacks;

            if ((player.transform.position - transform.position).magnitude < attackDistance)
            {
                player.TakeDamage(attackDamage);
                Debug.Log(this.name + " attacked player for " + attackDamage + " damage");
            }
        }
        else if (movementAi == noMovementAi && timeBeforeNextAttack < 0f)
        {
            moveToPositionAi.UpdateValues(player.transform.position.x - distanceToPlayer, player.transform.position.x + distanceToPlayer, player.transform.position.y - distanceToPlayer, player.transform.position.y + distanceToPlayer);
            initNewMovementAi(moveToPositionAi);
            Debug.Log(this.name + " is now moving towards the player");
        }
    }
}
