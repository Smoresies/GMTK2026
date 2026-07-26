using System;
using UnityEngine;

public class LittleHorrorMonsterEnemy : EnemyController
{


    [SerializeField] private GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {    
            GameObject explo = Instantiate(explosionPrefab, transform.position, transform.rotation);
            ExplosionManager exSetters = explo.GetComponent<ExplosionManager>();
            exSetters.SetDamage(damage * 3f);
            exSetters.SetTargetsPlayer();
            

            Destroy(gameObject);
        }
    }
}
