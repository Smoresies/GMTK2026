using System.Collections.Generic;
using UnityEngine;

public class BiiiiigHorrorMonsterEnemy : EnemyController
{
    [SerializeField]
    protected MoveToPositionAI moveToPositionAi;
    [SerializeField]
    protected NoMovementAi noMovementAi;
    [SerializeField]
    private float timeBetweenDecisions = 1f;
    private float timeBeforeNextDecision = 0f;
    [SerializeField]
    private float timeBetweenAttacks = 1f;
    private float timeBeforeNextAttack = 0f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private float distanceToPlayer;
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private List<WeightedObject<string>> weightedDecisions;
    [SerializeField]
    private float variableDecisionTime = .25f;
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
        timeBeforeNextDecision -= Time.fixedDeltaTime;
        timeBeforeNextAttack -= Time.fixedDeltaTime;
        if ((player.transform.position - transform.position).magnitude < attackDistance && timeBeforeNextAttack < 0)
        {
            timeBeforeNextAttack = timeBetweenAttacks;
            player.TakeDamage(attackDamage);
            Debug.Log(this.name + " attacked player for " + attackDamage + " damage");
        }
        if (timeBeforeNextDecision < 0f)
        {
            timeBeforeNextDecision = timeBetweenDecisions + Random.Range(-variableDecisionTime, variableDecisionTime);
            switch(Utils.GetRandomWeightedObject(weightedDecisions).item)
            {
                case "noMove":
                    initNewMovementAi(noMovementAi);
                    break;
                case "move":
                    moveToPositionAi.UpdateValues(player.transform.position.x - distanceToPlayer, player.transform.position.x + distanceToPlayer, player.transform.position.y - distanceToPlayer, player.transform.position.y + distanceToPlayer);
                    initNewMovementAi(moveToPositionAi);
                    break;
                default:
                    Debug.LogError("You dun fucked up decisions A-A RON");
                    break;
            }
        }
    }
}
