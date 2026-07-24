using System;
using UnityEditor.UI;
using UnityEngine;
using Random = System.Random;

public class BulletController : MonoBehaviour
{

   private float _damage = 0;
   private float _critRate = 0.0f;
   private float _critDamage = 1.0f;

   private void OnCollisionEnter2D(Collision2D collision)
   {

      if (collision.gameObject.TryGetComponent(out EnemyController enemy))
      {
         PlayerController pc = GameObject.FindAnyObjectByType<PlayerController>();

         // THIS IS LIKELY MASSIVELY OVER-FUCKING-POWERED
         if (pc.hasWeightedDie)
            if (pc.hasTrickstersDeck)
               _critRate *= 4.0f;
            else
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
            
            // Chronomancer's Charm
            if (pc.hasChronoCharm)
            {
               
               if (pc.chronoCharmCD <= 0.0f)
               {
                  // only start the CD on the final application.
                  if (i + 1 == attacks)
                     pc.chronoCharmCD = pc.relicCDs;
                  pc.freezeTime();
               }
            }
               
         }
            
      } else if (collision.gameObject.TryGetComponent(out PlayerController player))
      {
         if (UnityEngine.Random.Range(0.0f, 1.0f) <= this._critRate)
            _damage *= _critDamage;
         player.TakeDamage(_damage);
      }
         
      Destroy(gameObject);
   }

   public void SetDamage(float damage, float critRate,  float critDamage)
   {
      if(damage > 0)
         this._damage = damage;
      if(critRate > 0)
         this._critRate = critRate;
      if(critDamage > 0)
         this._critDamage = critDamage;
   }

}
