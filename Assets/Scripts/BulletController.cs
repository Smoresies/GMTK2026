using System;
using UnityEditor.UI;
using UnityEngine;

public class BulletController : MonoBehaviour
{

   private int _damage = 0;

   private void OnCollisionEnter2D(Collision2D collision)
   {
      if(collision.gameObject.TryGetComponent(out EnemyController enemy))
      {   
         enemy.TakeDamage(_damage);
      }
      Destroy(gameObject);
   }

   public void SetDamage(int damage)
   {
      if(damage > 0)
      {  
         this._damage = damage;
      }
   }

}
