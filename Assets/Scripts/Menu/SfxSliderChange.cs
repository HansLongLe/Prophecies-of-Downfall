using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SfxSliderChange : MonoBehaviour,IEndDragHandler
{
    public delegate void SfxSliderChangeWithFloat(float value);

    public static event SfxSliderChangeWithFloat SliderMoved;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.IsPointerMoving()) return;
        var value = eventData.pointerDrag.GetComponent<Slider>().value;
        SliderMoved?.Invoke(value);
    }
}
