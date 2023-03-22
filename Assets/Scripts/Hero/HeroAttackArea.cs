using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackArea : MonoBehaviour
{
    [HideInInspector] public bool enteredArea;
    [HideInInspector] public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        enteredArea = true;
        enemy = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        enteredArea = false;
    }
}
