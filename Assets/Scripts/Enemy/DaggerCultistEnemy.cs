using UnityEngine;

public class DaggerCultistEnemy : EnemyController
{
    [SerializeField]
    protected MoveToPlayerAi moveToPlayerAi;
    [SerializeField] 
    protected MoveAwayFromPlayerAi runAwayAi;
    [SerializeField]
    private float timeBetweenAttacks = 3f;
    private float timeBeforeNextAttack = 0f;
    [SerializeField]
    private int attackDamage = 1;
    protected void Start()
    {
        base.Start();
        moveToPlayerAi.initialize(player.transform, transform);
        runAwayAi.initialize(player.transform, transform);
        initNewMovementAi(moveToPlayerAi);
    }

    private void initNewMovementAi(MovementAi newMovementAi)
    {
        movementAi = newMovementAi;
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();
        timeBeforeNextAttack -= Time.fixedDeltaTime;
        if (movementAi == moveToPlayerAi && moveToPlayerAi.IsWithinStopDistance())
        {
            initNewMovementAi(runAwayAi);
            timeBeforeNextAttack = timeBetweenAttacks;
            player.TakeDamage(attackDamage);
            Debug.Log(this.name + " attacked player for " + attackDamage + " damage");
        }
        else if (movementAi == runAwayAi && timeBeforeNextAttack < 0f)
        {
            initNewMovementAi(moveToPlayerAi);
            Debug.Log(this.name + " is now moving towards the player");
        }
    } 
}
