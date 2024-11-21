using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoudButton : Button
{
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button clicked!");
        base.OnPointerClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button down!");
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button up!");
        base.OnPointerUp(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button entered!");
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button exited!");
        base.OnPointerExit(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button selected!");
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button deselected!");
        base.OnDeselect(eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Button submitted!");
        base.OnSubmit(eventData);
    }


}
