using System.Collections;
using System.Collections.Generic;
using Drop;
using Enemy;
using Inventory;
using ObjectPool;
using Player;
using UnityEngine;
using Util;

namespace Building
{
    public class ProducerScript : MonoBehaviour, IHealth
    {
        [SerializeField] private float _timeDelay = default;
        
        [SerializeField] protected float _maxHealth = 100;
        private List<EnemyScript> _enemies = new List<EnemyScript>();
        private float _health;
        
        private void Start()
        {
            _health = _maxHealth;
        }
        
        public IEnumerator ProduceEssence()
        {
            Debug.Log("Сожрал");
            Item item = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.Essence) as Item;
            item.gameObject.SetActive(false);
            InventoryScript.Instance.AddItem(item);
            Debug.Log("Сожрал дубль 2");
            yield return new WaitForSeconds(0.1f);//TODO
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

        public void GetDamage(float damage)
        {
            _health -= damage;
            DamageNumScript damageNumScript = ObjectPooler
                .Instance
                .SpawnFromPool(PoolObjectType.HitNumber) as DamageNumScript;
            if (!(damageNumScript is null))
            {
                damageNumScript.Text.text = damage.ToString();
                damageNumScript.transform.position = transform.position;
            }

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
            //Destroy(gameObject);
        }
    }
}