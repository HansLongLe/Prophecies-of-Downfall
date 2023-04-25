using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredTreeHealth : MonoBehaviour
{
    
    private float x;
    private Vector3 localScale;

    public delegate void SacredTreeHealthWithoutArgs();

    public static event SacredTreeHealthWithoutArgs TreeDestroyed;
    
    // Start is called before the first frame update
    private void Start()
    {
        SacredTree.TakenDamage += TakenDamage;
        localScale = transform.localScale;
    }

    private void TakenDamage(int amount)
    {
        if (this == null) return;
        var transform1 = transform;
        if (transform1.localScale.x >= 0)
        {
            x = (float)amount / 500 * localScale.x;
            transform1.localScale = new Vector3(transform1.localScale.x - x, localScale.y, localScale.z);
        }
        else
        {
            TreeDestroyed?.Invoke();
        }

    }

}
