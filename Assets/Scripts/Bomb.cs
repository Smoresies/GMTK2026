using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float boomTimer = 1.5f;
    [SerializeField] private GameObject explosionPrefab;
    
    public float _damage = 1f;

    // Update is called once per frame
    void Update()
    {
        boomTimer -= Time.deltaTime;
        if (boomTimer <= 0)
        {
            GameObject explo = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explo.GetComponent<ExplosionManager>().SetDamage(_damage * 0.5f);
            explo.GetComponent<ExplosionManager>().SetTargetsPlayer();
            Destroy(gameObject);
        }
    }
}
