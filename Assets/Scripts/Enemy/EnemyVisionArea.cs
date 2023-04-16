using System;
using JetBrains.Annotations;
using UnityEngine;


public class EnemyVisionArea : MonoBehaviour
{
    private IDamageable component;
    public delegate void EnemyVisionAreaFunctionWithTarget(IDamageable component);
    public event EnemyVisionAreaFunctionWithTarget MoveTowards;
    public event EnemyVisionAreaFunctionWithTarget StopFollowing;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmgComponent))
        {
            MoveTowards?.Invoke(dmgComponent);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmgComponent))
        {
            StopFollowing?.Invoke(dmgComponent);
        }
    }
    
}
