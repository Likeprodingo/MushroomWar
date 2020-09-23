using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject _player = default;
    private NavMeshAgent _navMesh = default;

    void Awake()
    {
        _player = GameObject.Find("Player");
        _navMesh = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        _navMesh.SetDestination(_player.transform.position);
    }
}
