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
         if (UnityEngine.Random.Range(0.0f, 1.0f) <= this._critRate)
            _damage *= _critDamage;
         enemy.TakeDamage(_damage);
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
