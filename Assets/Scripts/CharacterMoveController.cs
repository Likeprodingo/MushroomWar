using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMoveController : MonoBehaviour
{
    [SerializeField]
    private JoyController _joystick = default;
    [SerializeField]
    private Rigidbody _rigidbody = default;
    [SerializeField]
    private float _speed = 15f;

    private void Start()
    {
        _joystick.MoveEvent += Move;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    private void Move(Vector2 input)
    {
        _rigidbody.velocity = new Vector3(input.x * _speed,0,input.y * _speed);
    }
}
