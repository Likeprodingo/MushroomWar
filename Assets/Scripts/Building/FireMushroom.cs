using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Building
{
    public class FireMushroom : Building
    {

        [SerializeField] private float _damage = 1f;
        private readonly List<EnemyScript> _enemies = new List<EnemyScript>();
        private List<EnemyScript> _removeEnemies = new List<EnemyScript>();
        private bool _isActive;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyScript enemyScript))
            {
                _enemies.Add(enemyScript);
                if (!_isActive)
                {
                    StartCoroutine(Attack());
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Enemy"))
            {
                _enemies.Remove(other.GetComponent<EnemyScript>());
            }
        }

        protected override IEnumerator Attack()
        {
            _isActive = true;
            while (_enemies.Count != 0 && CurrentActivation > 0)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    if (_enemies[i].gameObject.activeSelf == false)
                    {
                        _removeEnemies.Add(_enemies[i]);
                    }
                    _enemies[i].GetDamage(_damage);
                }

                for (int i = 0; i < _removeEnemies.Count; i++)
                {
                    _enemies.Remove(_removeEnemies[i]);
                }

                _removeEnemies = new List<EnemyScript>();
                yield return new WaitForSeconds(1.1f);
            }

            _isActive = false;
        }
    }
}