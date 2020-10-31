using Player;
using UnityEngine;

namespace ObjectPool
{
    public interface IPooledObject
    {
        PoolObjectType PoolType { get; set; }
        void Init();
        void OnObjectSpawn();
        void OnObjectDespawn();
        void Despawn();
    }
}