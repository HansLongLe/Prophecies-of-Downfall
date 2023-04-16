using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireLight : MonoBehaviour
{
    [SerializeField] private GameObject flameObject;

    [SerializeField] private GameObject glowObject;

    [SerializeField] private GameObject sparkObject;

    [SerializeField] private GameObject light2DObject;
    
    private Light2D light2D;
    
    // Start is called before the first frame update
    private void Start()
    {
        DayNightSystem2D.Lighted += ToggleLight;
        flameObject.SetActive(false);
        glowObject.SetActive(false);
        sparkObject.SetActive(false);
        light2D = light2DObject.GetComponent<Light2D>();
        light2D.enabled = false;
    }

    private void ToggleLight()
    {
        if (light2D.enabled)
        {
            light2D.enabled = false;
            flameObject.SetActive(false);
            glowObject.SetActive(false);
            sparkObject.SetActive(false);
        }
        else
        {
            light2D.enabled = true;
            flameObject.SetActive(true);
            glowObject.SetActive(true);
            sparkObject.SetActive(true);
        }
    }
}
