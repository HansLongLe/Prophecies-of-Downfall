using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour
{
    public delegate void EnemyAttackAreaFunctionWithTarget(IDamageable component);
    public event EnemyAttackAreaFunctionWithTarget AttackTarget;
    public event EnemyAttackAreaFunctionWithTarget StopAttacking;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var dmgComponent)) return;
        AttackTarget?.Invoke(dmgComponent);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var dmgComponent)) return;
        StopAttacking?.Invoke(dmgComponent);
    }
}
