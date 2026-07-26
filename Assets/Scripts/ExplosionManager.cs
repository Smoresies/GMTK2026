using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    private ParticleSystem particles;
    private Collider2D coll;

    private float _damage = 1.0f;
    
    private bool _hitsPlayer = false;
    
    private void Start()
    {
        PlayerController pc = GameObject.FindAnyObjectByType<PlayerController>();
        
        ParticleSystemRenderer psRenderer = GetComponent<ParticleSystemRenderer>();
        psRenderer.material.SetColor("_EmissionColor", Color.red);
        particles = gameObject.GetComponent<ParticleSystem>();
        particles.Play(true);
        coll = gameObject.GetComponent<Collider2D>();
        
        List<Collider2D> results = new List<Collider2D>();
        int hitCount = coll.Overlap(results);

        foreach (Collider2D col in results)
        {
            // if it originated from the player OR the player has Bottled Rage
            if ((!_hitsPlayer || pc.hasBottledRage) && col.gameObject.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(_damage);
                // Debug.Log(col.gameObject.name);
            } else if (_hitsPlayer && col.gameObject.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(_damage);
                // Debug.Log(col.gameObject.name);
            }
        }
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetTargetsPlayer()
    {
        // This should only be set true from enemies
        _hitsPlayer = true;
    }
}
