using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Eventually add some like. Art/effect here
            Destroy(gameObject);
        }
            
    }
}
