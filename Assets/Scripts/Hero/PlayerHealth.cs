using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float x;
    private Vector3 localScale;
    
    public delegate void PlayerHealthFunctionWithoutArgs();
    public static event PlayerHealthFunctionWithoutArgs ZeroHealth;
    
    // Start is called before the first frame update
    private void Start()
    {
        HeroKnight.DamageTaken += DamageTaken;
        localScale = transform.localScale;
    }

    private void DamageTaken(int amount)
    {
        if(this == null) return;
        var transform1 = transform;
        if (transform1.localScale.x > 0)
        {
            x = (float)amount / 100 * localScale.x;
            transform1.localScale = new Vector3(transform1.localScale.x - x, localScale.y, localScale.z);    
        }
        else
        {
            ZeroHealth?.Invoke();
        }
    }
}
