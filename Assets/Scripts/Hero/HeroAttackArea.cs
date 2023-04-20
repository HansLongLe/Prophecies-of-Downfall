using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackArea : MonoBehaviour
{
    private Collider2D attackAreaCollider;

    private readonly List<IDamageableEnemy> colliderList = new List<IDamageableEnemy>();

    private void Start()
    {
        attackAreaCollider = GetComponent<Collider2D>();
        HeroKnight.TurnedAround += SwitchAttackAreaX;
        HeroKnight.Attacked += Damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<IDamageableEnemy>(out var enemyComponent))
        {
            colliderList.Add(enemyComponent);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<IDamageableEnemy>(out var enemyComponent))
        {
            colliderList.Remove(enemyComponent);
        }
    }

    private void SwitchAttackAreaX(int direction)
    {
        if (attackAreaCollider != null)
        {
            var offsetX = Math.Abs(attackAreaCollider.offset.x);
            attackAreaCollider.offset = new Vector2(direction * offsetX, attackAreaCollider.offset.y);
        }
        
    }

    private void Damage()
    {
        foreach (var enemy in colliderList)
        {
            enemy?.TakeDamage(HeroDamage.Damage);
        }
    }
}
