using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject HpFrame;
    private float x;
    private Vector3 localScale;
    private const int barVisibilityTime = 3;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer hpFrameSpriteRenderer;


    // Start is called before the first frame update
    private void Start()
    {
        localScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        hpFrameSpriteRenderer = HpFrame.GetComponent<SpriteRenderer>();
        hpFrameSpriteRenderer.enabled = false;
    }

    
    public float TakeDamage(int damageAmount)
    {
        var transform1 = transform;
        if (!(transform1.localScale.x > 0)) return transform.localScale.x;
        x = (float)damageAmount / 100 * localScale.x;
        transform1.localScale = new Vector3(transform1.localScale.x - x, localScale.y, localScale.z);
        spriteRenderer.enabled = true;
        hpFrameSpriteRenderer.enabled = true;
        StartCoroutine(HealthBarVisibility());
        return transform.localScale.x;
    }

    private IEnumerator HealthBarVisibility()
    {
        var timer = 0f;
        while (timer <= barVisibilityTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.enabled = false;
        hpFrameSpriteRenderer.enabled = false;
    }
    
}
