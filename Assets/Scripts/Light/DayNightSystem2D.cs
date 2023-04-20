using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public enum DayCycles
{
    Sunrise = 0,
    Day = 1,
    Sunset = 2,
    Night = 3,
    Midnight = 4
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]

    [SerializeField] private Light2D globalLight;

    [SerializeField] private float cycleCurrentTime = 0;
    
    [Tooltip("This is a cycle max time in seconds, if current time reach this value we change the state of the day and night cycles")]
    public float cycleMaxTime = 60; // duration of cycle

    public DayCycles dayCycle;

    [Header("Cycle Colors")]
    
    [SerializeField] private Color sunrise; //6:00 at 10:00
    
    [SerializeField] private Color day; // Eg: 10:00 at 16:00
    
    [SerializeField] private Color sunset; // Eg: 16:00 20:00
    
    [SerializeField] private Color night; // Eg: 20:00 at 00:00
    
    [SerializeField] private Color midnight; // Eg: 00:00 at 06:00
    
    public delegate void DayNightSystem2DWithoutArgs();
    public static event DayNightSystem2DWithoutArgs Lighted;
    public static event DayNightSystem2DWithoutArgs ChangePlaylistToNight;
    public static event DayNightSystem2DWithoutArgs ChangePlaylistToNormal;
    
    private void Start() 
    {
        dayCycle = DayCycles.Sunrise;
        globalLight.color = sunrise;
    }

     private void Update()
     {
         cycleCurrentTime += Time.deltaTime;
         if (cycleCurrentTime >= cycleMaxTime) 
        {
            cycleCurrentTime = 0;
            dayCycle++;
            DayCycleChanged();
            LightsSwitched();
        }
     }

     private void DayCycleChanged()
     {
         if(dayCycle > DayCycles.Midnight)
             dayCycle = 0;

         // percent it's an value between current and max time to make a color lerp smooth
         var percent = cycleCurrentTime / cycleMaxTime;
         globalLight.color = dayCycle switch
         {
             DayCycles.Sunrise => Color.Lerp(sunrise, day, percent),
             DayCycles.Day => Color.Lerp(day, sunset, percent),
             DayCycles.Sunset => Color.Lerp(sunset, night, percent),
             DayCycles.Night => Color.Lerp(night, midnight, percent),
             DayCycles.Midnight => Color.Lerp(midnight, day, percent),
             _ => globalLight.color
         };
         switch (dayCycle)
         {
             case DayCycles.Night or DayCycles.Midnight:
                 ChangePlaylistToNight?.Invoke();
                 break;
             case DayCycles.Day or DayCycles.Sunrise or DayCycles.Sunset:
                 ChangePlaylistToNormal?.Invoke();
                 break;
         }
     }

     private void LightsSwitched()
     {
         if(dayCycle is DayCycles.Sunset or DayCycles.Sunrise)
         {
             Lighted?.Invoke();
         }
     }
     
}
