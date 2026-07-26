using System;
using UnityEngine;

public class LittleHorrorMonsterEnemy : EnemyController
{
    [SerializeField]
    protected MoveToPlayerAi moveToPlayerAi;
    [SerializeField]
    protected NoMovementAi noMovementAi;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float timeBetweenDecisions = 2f;
    private float timeBeforeNextDecision = 2f;
    [SerializeField]
    private float attackDistance;

    protected override void Start()
    {
        base.Start();
        moveToPlayerAi.initialize(player.transform, transform);
        noMovementAi.initialize(player.transform, transform);
        initNewMovementAi(noMovementAi);
    }

    private void initNewMovementAi(MovementAi newMovementAi)
    {
        movementAi = newMovementAi;
        movementAi.initialize(player.transform, transform);
        // Debug.Log(this.name + " is now using " + newMovementAi.GetType().Name);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        timeBeforeNextDecision -= Time.fixedDeltaTime;
        if (timeBeforeNextDecision < 0f)
        {
            initNewMovementAi(moveToPlayerAi);
        }
        if ((player.transform.position - transform.position).magnitude < attackDistance)
        {    
            GameObject explo = Instantiate(explosionPrefab, transform.position, transform.rotation);
            ExplosionManager exSetters = explo.GetComponent<ExplosionManager>();
            exSetters.SetDamage(damage * 3f);
            exSetters.SetTargetsPlayer();
            

            Die();
        }
    }
}
