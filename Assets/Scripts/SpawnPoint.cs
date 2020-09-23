using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPoint : MonoBehaviour
{
    private static readonly List<GameObject> _enemies = new List<GameObject>();
    [SerializeField]
    private GameObject _wolf = default;
    private readonly Vector3[] _spawnPosition = {
        new Vector3(0,0,0),
        new Vector3(0,0,2),
        new Vector3(2,0,0),
        new Vector3(-2,0,0),
        new Vector3(0,0,-2),
    };

    public static List<GameObject> Enemies => _enemies;
    public Vector3[] SpawnPosition => _spawnPosition; 
    public int WolfCount { get; set; } = 0;
    
    public void Spawn()
    {
        int i = 0;
        for (; i < WolfCount && i < 5; i++)
        {
            GameObject enemy = Instantiate(_wolf, transform.position + _spawnPosition[i], transform.rotation,transform);
            _enemies.Add(enemy);
        }
    }
}
