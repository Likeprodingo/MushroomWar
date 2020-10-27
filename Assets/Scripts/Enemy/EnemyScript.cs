using System;
using System.Collections;
using ObjectPool;
using Player;
using Spawn;
using UnityEngine;
using UnityEngine.AI;
using Util;
using Random = UnityEngine.Random;

namespace Enemy
{

    public class EnemyScript : PooledObject
    {
        [SerializeField] protected float _updateTime = 0.5f;
        [SerializeField] protected NavMeshAgent _navMesh = default;
        [SerializeField] protected float _maxHealth = 100;
        [SerializeField] protected float _attackRange = 3f;
        [SerializeField] protected float _damage = 1f;
        [SerializeField] protected float _health = default;
        [SerializeField] protected float _coolDownTime = 1f;
        [SerializeField] protected float _coolDown = 1f;
        [SerializeField] protected float _attackRate = 0.25f;
        [SerializeField] protected SpawnPoint _spawnPoint = default;
        private IHealth _playerHealth;
        private bool _alive = true;

        public IHealth PlayerHealth
        {
            get => _playerHealth;
            set => _playerHealth = value;
        }

        public float AttackRange => _attackRange;

        public float Damage => _damage;

        public float Health => _health;
        
        public override void Init()
        {
            _playerHealth = Player.gameObject.GetComponent<PlayerHealthScript>();
            _health = _maxHealth;
        }

        public override void OnObjectSpawn()
        {
            StartCoroutine(Moving());
        }
        
        private IEnumerator Moving()
        {
            var waiter = new WaitForSeconds(_updateTime);
            while (true)
            {
                yield return waiter;
                if (PlayerInRange() && _coolDown >= _coolDownTime)
                {
                    _coolDown = 0;
                    StartCoroutine(Attack());
                }
                else
                {
                    _navMesh.SetDestination(Player.position);
                }

                _coolDown += _attackRate;
            }
        }

        private bool PlayerInRange()
        {
            return (Player.position - transform.position).magnitude <= _attackRange;
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
            if (_health <= 0 && _alive)
            {
                _alive = false;
                StartCoroutine(Death());
            }
        }

        private IEnumerator Attack()
        {
            //start Animation
            yield return new WaitForSeconds(1.5f);
            if (PlayerInRange())
            {
                _playerHealth.GetDamage(_damage);
            }
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(1f);
            _spawnPoint.EnemyDeath(this);
            Drop();
            Despawn();
        }

        private void Drop()
        {
            PoolObjectType type = PoolObjectType.FlowerDrop;
            int dropCount = Random.Range(1, 3);
            int dropIndex;
            GameObject drop;
            for (int i = 0; i < dropCount; i++)
            {
                dropIndex = Random.Range(0, 2);
                if (dropIndex == 0)
                {
                    dropIndex = Random.Range(0, 9);
                    switch (dropIndex)
                    { 
                        case 6:
                        case 7:
                          type = PoolObjectType.FireMushroomDrop;
                          break;
                        case 8:
                        case 9:
                          type = PoolObjectType.ElusiveMushroomDrop;
                          break;
                    }
                    drop = ObjectPooler.Instance.SpawnFromPool(type,transform.position);
                }
                else
                {
                    dropIndex = Random.Range(50, 100);
                    drop = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.DarkEssence, transform.position);
                }
            }
        }
    }
}
