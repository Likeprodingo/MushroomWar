using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

namespace Spawn
{
    public class LevelController : MonoBehaviour
    {
        
        [SerializeField] private float _spawnNumber = 5f;
        [SerializeField] private Vector3 _spawnDistance = new Vector3(6,0,6);
        private List<SpawnPoint> _points = new List<SpawnPoint>();

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

                SpawnPoint spawn = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.Spawn,pos,transform.rotation,gameObject).GetComponent<SpawnPoint>();
                spawn.Wave = new Wave(2,0,0);
                spawn.Spawn();
                _points.Add(spawn);
            }
        }
    }
}
