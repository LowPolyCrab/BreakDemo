using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public delegate void InputUpdatedDelegate(Vector2 inputVal);
    public event InputUpdatedDelegate OnInputUpdated;

    [SerializeField] private RectTransform rangeTransform;
    [SerializeField] private RectTransform thumbStickTransform;
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private float weaponSwitchTime = 0.3f;

    private float _range;

    private void Awake()
    {
        _range = rangeTransform.sizeDelta.x/2f;
        deadZone = deadZone * deadZone;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rangeTransform.position = eventData.pressPosition;
        thumbStickTransform.position = eventData.pressPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        thumbStickTransform.localPosition = Vector2.zero;
        rangeTransform.localPosition = Vector2.zero;
        OnInputUpdated?.Invoke(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset = Vector2.ClampMagnitude(eventData.position - eventData.pressPosition, _range);
        thumbStickTransform.position = eventData.pressPosition + offset;
        Vector2 input = offset / _range;

        if(input.sqrMagnitude < deadZone)
           return;

        OnInputUpdated?.Invoke(offset/_range);
    }
}
