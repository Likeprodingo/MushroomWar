using System;
using UnityEngine;

namespace Player
{
    public class CharacterMoveController : MonoBehaviour
    {
        [SerializeField]
        private JoyController _joystick = default;
        [SerializeField]
        private Rigidbody _rigidbody = default;
        [SerializeField]
        private float _speed = 15f;
        
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        private void FixedUpdate()
        {
            Move(_joystick.Direction);    
        }

        private void Move(Vector2 input)
        {
            _rigidbody.velocity = new Vector3(input.x * _speed,0,input.y * _speed);
        }
    }
}
