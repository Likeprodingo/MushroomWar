using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMoveController : MonoBehaviour
{
    [SerializeField]
    private Joystick _joystick = default;
    [SerializeField]
    private Rigidbody _rigidbody = default;
    [SerializeField]
    private float _speed = 15f;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _speed, 0, _joystick.Vertical * _speed);
    }
}
