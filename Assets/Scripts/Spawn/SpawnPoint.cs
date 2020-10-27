using System.Collections.Generic;
using Enemy;
using ObjectPool;
using UnityEngine;

namespace Spawn
{
    public class SpawnPoint : PooledObject
    {
    
        [SerializeField] private readonly Vector3[] _spawnPosition = {
            new Vector3(0,0,0),
            new Vector3(0,0,2),
            new Vector3(2,0,0),
            new Vector3(-2,0,0),
            new Vector3(0,0,-2),
        };
        [SerializeField] private Wave _wave = default;
        
        private static readonly List<EnemyScript> Enemies = new List<EnemyScript>();
        private static int _waveNumber = 0;
        
        public Wave Wave
        {
            get => _wave;
            internal set => _wave = value;
        }
        
        public Vector3[] SpawnPosition => _spawnPosition;
        

        public void EnemyDeath(EnemyScript enemy)
        {
            Enemies.Remove(enemy);
        }
        public void Spawn()
        {
            _waveNumber++;
            int i = 0;
            for (; i < _wave.WolfCount && i < 5; i++)
            { 
                EnemyScript enemy = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.Wolf, transform.position + _spawnPosition[i], transform.rotation).GetComponent<EnemyScript>();
                Enemies.Add(enemy);
            }
            
            for (; i < _wave.OgrCount && i < 5; i++)
            { 
                EnemyScript enemy = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.Ogr, transform.position + _spawnPosition[i], transform.rotation).GetComponent<EnemyScript>();
                Enemies.Add(enemy);
            }
            
            for (; i < _wave.ShamanCount && i < 5; i++)
            { 
                EnemyScript enemy = ObjectPooler.Instance.SpawnFromPool(PoolObjectType.Shaman, transform.position + _spawnPosition[i], transform.rotation).GetComponent<EnemyScript>();
                Enemies.Add(enemy);
            }
        }
    }
}
