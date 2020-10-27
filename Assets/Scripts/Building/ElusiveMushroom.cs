using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Inventory;
using ObjectPool;
using Player;
using UnityEngine;
using Util;

namespace Building
{
    public class ElusiveMushroom : Building, IHealth
    {
        [SerializeField] protected float _maxHealth = 100;
        [SerializeField] private Item _item = default;
        
        private List<EnemyScript> _enemies = new List<EnemyScript>();
        private IHealth _playerHealthScript;
        private Transform _player;
        private float _health;
        
        private new void OnEnable()
        {
            StartColor = _renderer.material.color;
            AddEssence();
            _health = _maxHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyScript enemyScript))
            {
                _player = enemyScript.Player.transform;
                _playerHealthScript = enemyScript.PlayerHealth;
                enemyScript.Player = transform;
                enemyScript.PlayerHealth = this;
                _enemies.Add(enemyScript);
            }
        }
        
        protected override IEnumerator Attack()
        {
            yield break;
        }
        
        public void GetDamage(float damage)
        {
            _health -= damage;
            DamageNumScript damageNumScript = ObjectPooler
                .Instance
                .SpawnFromPool(PoolObjectType.HitNumber)
                .GetComponent<DamageNumScript>();
            damageNumScript.Text.text = damage.ToString();
            damageNumScript.transform.position = transform.position;
            if (_health <= 0)
            {
                StartCoroutine(Death());
            }
        }
        
        public IEnumerator Death()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].gameObject.activeSelf)
                {
                    _enemies[i].Player = _player;
                    _enemies[i].PlayerHealth = _playerHealthScript;
                }
            }
            _item.Despawn();
        }
    }
}