using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Util;

namespace Building
{
    public class Flower : ABuilding
    {
        [SerializeField] private float _damage = 100f;
        [SerializeField] private float _coolDownTime = 1f;
        private GameObject _enemy;
        private EnemyScript _enemyScript;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EnemyScript enemyScript) && ReferenceEquals(_enemy,null) && CurrentActivation > 0)
            {
                _enemyScript = enemyScript;
                _enemy = enemyScript.gameObject;
                StartCoroutine(Attack());
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (ReferenceEquals(other.gameObject, _enemy))
            {
                _enemy = null;
            }
        }
        
        protected override IEnumerator Attack()
        {
            yield return new WaitForSeconds(_coolDownTime);
            while (!ReferenceEquals(_enemy,null) && _enemy.gameObject.activeSelf && CurrentActivation > 0)
            {
                if (GameController.IsActive)
                {
                    _enemyScript.GetDamage(_damage);
                }
                yield return new WaitForSeconds(_coolDownTime);
            }
            _enemy = null;
        }
    }
}