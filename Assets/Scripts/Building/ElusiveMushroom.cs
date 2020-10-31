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
    public class ElusiveMushroom : ABuilding, IHealth
    {
        [SerializeField] protected float _maxHealth = 100;
        [SerializeField] private Item _item = default;
        
        private List<EnemyScript> _enemies = new List<EnemyScript>();
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
                enemyScript.Aim = transform;
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
                as DamageNumScript;
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
                    _enemies[i].Aim = PlayerHealthScript.Instance.transform;
                    _enemies[i].PlayerHealth =  PlayerHealthScript.Instance;
                }
            }
            _item.Despawn();
        }
    }
}