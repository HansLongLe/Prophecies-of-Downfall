using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampLight : MonoBehaviour
{

    [SerializeField] private GameObject glowObject;

    [SerializeField] private GameObject light2DObject;
    [SerializeField] private GameObject lampObject;

    [SerializeField] private Sprite litLamp;

    [SerializeField] private Sprite unlitLamp;

    private Light2D light2D;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        DayNightSystem2D.Lighted += ToggleLights;
        glowObject.SetActive(false);
        light2D = light2DObject.GetComponent<Light2D>();
        light2D.enabled = false;
        spriteRenderer = lampObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unlitLamp;
    }

    // Update is called once per frame
    private void ToggleLights()
    {
        if (light2D.enabled)
        {
            glowObject.SetActive(false);
            light2D.enabled = false;
            spriteRenderer.sprite = unlitLamp;
        }
        else
        {
            glowObject.SetActive(true);
            light2D.enabled = true;
            spriteRenderer.sprite = litLamp;
        }
    }
}
