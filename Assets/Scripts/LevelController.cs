using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _points = new List<GameObject>();
    [SerializeField]
    private float _spawnNumber = 5f;
    [SerializeField]
    private Vector3 _spawnDistance = new Vector3(6,0,6);
    [SerializeField]
    private GameObject _spawnPrefab = default;

    public float SpawnNumber
    {
        get => _spawnNumber;
        set => _spawnNumber = value;
    }

    public List<GameObject> Points => _points;

    void Start()
    {
        
        _spawnDistance.y = 0;
        for (int i = 0; i < _spawnNumber; i++)
        {
            Vector3 pos = new Vector3(Random.Range(0,10), 0 , Random.Range(0, 10));
            pos += _spawnDistance;
            switch (i % 4)
            {
                case 1:
                    pos.x *= -1;
                    break;
                case 2:
                    pos.z *= -1;
                    break;
                case 3:
                    pos.x *= -1;
                    pos.z *= -1;
                    break;
            }
            GameObject spawn = Instantiate(_spawnPrefab, pos, transform.rotation, transform);
            spawn.GetComponent<SpawnPoint>().WolfCount = 1;
            _points.Add(spawn);
        }
        foreach(GameObject point in _points)
        {
            point.GetComponent<SpawnPoint>().Spawn();
        }
    }
}
