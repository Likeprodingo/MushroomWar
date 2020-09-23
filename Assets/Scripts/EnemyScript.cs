using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float _updateTime = 0.5f;
    [SerializeField]
    private NavMeshAgent _navMesh = default;
    private Transform _player = default;
    [SerializeField] private float _maxHealth = 100;
    private float _health;

    public void StartMoving(Transform player)
    {
        _health = _maxHealth;
        _player = player;
        StartCoroutine(Moving());
    }
    
    IEnumerator Moving()
    {
        var waiter = new WaitForSeconds(_updateTime);
        while (true)
        {
            yield return waiter;
            _navMesh.SetDestination(_player.position);
        }
    }
    
    public void GetDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<PoolObject>().ReturnToPool();
    }
}
