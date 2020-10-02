using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPoint : MonoBehaviour
{
    
    [SerializeField] private readonly Vector3[] _spawnPosition = {
        new Vector3(0,0,0),
        new Vector3(0,0,2),
        new Vector3(2,0,0),
        new Vector3(-2,0,0),
        new Vector3(0,0,-2),
    };

    [SerializeField] private PoolObjectType _type = default;
    private static readonly List<GameObject> _enemies = new List<GameObject>();
    public static List<GameObject> Enemies => _enemies;
    public Vector3[] SpawnPosition => _spawnPosition; 
    public int WolfCount { get; set; } = 0;
    
    public void Spawn()
    {
        int i = 0;
        for (; i < WolfCount && i < 5; i++)
        { 
            //GameObject enemy = PoolManager.GetObject("Wolf", transform.position + _spawnPosition[i], transform.rotation);
            //_enemies.Add(enemy);
        }
    }
}
