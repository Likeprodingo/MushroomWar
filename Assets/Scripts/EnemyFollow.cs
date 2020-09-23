using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField]
    private float _updateTime = 0.5f;
    [SerializeField]
    private NavMeshAgent _navMesh = default;
    [SerializeField]
    private Transform _player = default;

    public void StartMoving(Transform player)
    {
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
}
