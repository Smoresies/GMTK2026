using System;
using UnityEngine;

public class LittleHorrorMonsterEnemy : EnemyController
{


    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float attackDistance;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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
