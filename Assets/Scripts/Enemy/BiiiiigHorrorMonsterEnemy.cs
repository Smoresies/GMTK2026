using UnityEngine;

public class BiiiiigHorrorMonsterEnemy : EnemyController
{
    [SerializeField]
    private float timeBetweenAttacks = 3f;
    private float timeBeforeNextAttack = 0f;
    [SerializeField]
    private int attackDamage = 1;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        timeBeforeNextAttack -= Time.fixedDeltaTime;
        if ((movementAi as MoveToPlayerAi).IsWithinStopDistance() && timeBeforeNextAttack < 0f)
        {
            timeBeforeNextAttack = timeBetweenAttacks;
            player.TakeDamage(attackDamage);
            Debug.Log(this.name + " attacked player for " + attackDamage + " damage");
        }
    } 
}
