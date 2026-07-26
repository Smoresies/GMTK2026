using UnityEngine;

public class BulletController : MonoBehaviour
{

   private float _damage = 0;
   private float _critRate = 0.0f;
   private float _critDamage = 1.0f;

   [SerializeField] 
   private GameObject explosionPrefab;

   private void OnCollisionEnter2D(Collision2D collision)
   {
      PlayerController pc = GameObject.FindAnyObjectByType<PlayerController>();
      if (collision.gameObject.TryGetComponent(out EnemyController enemy))
      {
        

         
         // THIS IS LIKELY MASSIVELY OVER-FUCKING-POWERED
         if (pc.hasWeightedDie)
               _critRate *= 2.0f;
         
         
         int attacks = 1;
         bool didCrit = false;
         if (UnityEngine.Random.Range(0.0f, 1.0f) <= this._critRate)
         {
            _damage *= _critDamage;
            if (pc.hasRippedClover) 
               attacks = pc.hasTrickstersDeck ? 4 : 2;
            didCrit = true;
         } else if(pc.hasWeightedDie) 
            _damage /= 2;
         
         for (int i = 0; i < attacks; ++i)
         {
            enemy.TakeDamage(_damage);
            
            // Jagged Charm
            
            // Lightning Charm
            if (pc.hasLightningCharm && didCrit)
            {
               GameObject explode = Instantiate(explosionPrefab, transform.position, transform.rotation);
               explode.GetComponent<ExplosionManager>().SetDamage(_damage * 0.5f);
            }
            
            
            // Chronomancer's Charm
            if (pc.hasChronoCharm && didCrit)
            {
               
               if (pc.chronoCharmCD <= 0.0f)
               {
                  // only start the CD on the final application.
                  if (i + 1 == attacks)
                     pc.chronoCharmCD = pc.relicCDs;
                  pc.freezeTime();
                  if(pc.hasTrickstersDeck)
                     pc.freezeTime();
               }
            }
               
         }
            
      } else if (collision.gameObject.TryGetComponent(out PlayerController player))
      {
         if (pc.hasRippedClover && UnityEngine.Random.Range(0.0f, 1.0f) <= 0.25)
            _damage *= _critDamage;
         player.TakeDamage(_damage);
      }
      Destroy(gameObject);
   }

   public void SetDamage(float damage, float critRate = 0.0f,  float critDamage = 1.0f)
   {
      if(damage > 0)
         this._damage = damage;
      if(critRate > 0)
         this._critRate = critRate;
      if(critDamage > 0)
         this._critDamage = critDamage;
   }

}
