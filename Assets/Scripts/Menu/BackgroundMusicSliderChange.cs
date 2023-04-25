using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundMusicSliderChange : MonoBehaviour, IEndDragHandler
{
    // Start is called before the first frame update
    public delegate void BackgroundMusicSliderChangeWithFloat(float value);

    public static event BackgroundMusicSliderChangeWithFloat SliderMoved;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.IsPointerMoving()) return;
        var value = eventData.pointerDrag.GetComponent<Slider>().value;
        SliderMoved?.Invoke(value);
    }
}
