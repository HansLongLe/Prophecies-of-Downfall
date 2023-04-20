using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredTree : MonoBehaviour, IDamageable
{

    private SpriteRenderer spriteRenderer;

    public delegate void SacredTreeWithInt(int amount);

    public static event SacredTreeWithInt TakenDamage;

    private const float effectDuration = 0.1f; 
    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        TakenDamage?.Invoke(amount);
        StartCoroutine(TakenDamageEffect());
    }

    private IEnumerator TakenDamageEffect()
    {
        var timer = 0f;
        spriteRenderer.color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
        while (timer < effectDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
