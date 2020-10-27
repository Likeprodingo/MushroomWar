using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyController : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] 
    protected RectTransform _background = null;
    [SerializeField] 
    private RectTransform _handle = null;
    private Camera _cam = default;
    private Canvas canvas = default;
    private Vector2 _input = Vector2.zero;
    private Vector2 _position;
    private float _deadZone = 0;
    
    public Vector3 Direction { get; private set; }
    private void Start()
    {
        Vector2 center = new Vector2(0.5f, 0.5f);
        canvas = GetComponentInParent<Canvas>();
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
        _position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
        Direction = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 radius = _background.sizeDelta / 2;
        _input = (eventData.position - _position) / (radius * canvas.scaleFactor);
        HandleInput(_input.magnitude, _input.normalized);
        _handle.anchoredPosition = _input * radius;
        Direction = _input;
    }
    
    protected virtual void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > _deadZone)
        {
            if (magnitude > 1)
                _input = normalised;
        }
        else
            _input = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}
