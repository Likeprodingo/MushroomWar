using System;
using System.Collections;
using ObjectPool;
using TMPro;
using UnityEngine;

namespace Util
{
    public class DamageNumScript : PooledObject
    {
        [SerializeField] private Rigidbody _rigidbody = default;
        [SerializeField] private TextMesh _text = default;
        [SerializeField] private float _updateRate = default;
        [SerializeField] private int _moveCount = default;
        [SerializeField] private float _speed = default;

        public TextMesh Text => _text;

        public override void OnObjectSpawn()
        {
            StartCoroutine(Move());
        }


        private void Update()
        {
            _rigidbody.velocity = new Vector3(0,_speed,_speed);
            Color color = _text.color;
                
            _text.color = color;
        }

        private IEnumerator Move()
        { 
            yield return new WaitForSeconds(_updateRate);
            Despawn();
        }
    }
}