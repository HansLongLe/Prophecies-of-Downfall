using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDamage : MonoBehaviour
{
    public static int Damage { get; private set; } = 10;

    public void IncreaseDamage(int value)
    {
        Damage += value;
    }

    public void DecreaseDamage(int value)
    {
        Damage -= value;
    }

}
