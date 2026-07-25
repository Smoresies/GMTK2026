using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    private ParticleSystem particles;
    private Collider2D coll;

    private float _damage = 1.0f;
    
    private void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        particles.Play(true);
        coll = gameObject.GetComponent<Collider2D>();
        
        List<Collider2D> results = new List<Collider2D>();
        int hitCount = coll.Overlap(results);

        foreach (Collider2D col in results)
        {
            if (col.gameObject.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(_damage);
                Debug.Log(col.gameObject.name);
            }
        }
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
