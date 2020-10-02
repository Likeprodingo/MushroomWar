using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyScript : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float _updateTime = 0.5f;
    [SerializeField]
    private NavMeshAgent _navMesh = default;
    [SerializeField] 
    private float _maxHealth = 100;
    [SerializeField] 
    private float _attackRange = 3f;
    
    [SerializeField]
    private float _damage = 1f;
    private PlayerHealthScript _playerHealth = default;
    private float _health = default;
    private float _coolDown = 1f;
    private float _attackRate = 0.25f;

    public float AttackRange => _attackRange;

    public float Damage => _damage;

    public float Health => _health;

    public PoolObjectType PoolType { get; set; }

    public Transform Player { get; set;}

    public void Init()
    {
        _playerHealth = Player.gameObject.GetComponent<PlayerHealthScript>();
        _health = _maxHealth;
    }

    public void OnObjectSpawn()
    {
        StartCoroutine(Moving());
    }

    public void OnObjectDespawn()
    {
        throw new NotImplementedException();
    }

    public void Despawn()
    {
        ObjectPooler.Instance.Despawn(gameObject);
    }
    
    private IEnumerator Moving()
    {
        var waiter = new WaitForSeconds(_updateTime);
        while (true)
        {
            yield return waiter;
            if (PlayerInRange() &&  _coolDown >= 1)
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
        if (_health <= 0)
        {
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
        Despawn();
    }
}
