using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        DontDestroyOnLoad(instance);

    }
    
}
