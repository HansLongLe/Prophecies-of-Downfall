using UnityEngine;

public class DestroyEventHeroKnight : MonoBehaviour
{
    // Destroy particles when animation has finished playing. 
    // destroyEvent() is called as an event in animations.
    public void DestroyEvent()
    {
        Destroy(gameObject);
    }
}
