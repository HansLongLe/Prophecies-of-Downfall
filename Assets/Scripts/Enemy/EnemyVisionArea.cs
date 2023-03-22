using System;
using UnityEngine;


public class EnemyVisionArea : MonoBehaviour
{
    [HideInInspector]
    public bool enteredArea;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        enteredArea = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        enteredArea = false;
    }
}
