using UnityEngine;

public class LittleHorrorMonsterEnemy : EnemyController
{
    [SerializeField]
    private int damage = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {    
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
