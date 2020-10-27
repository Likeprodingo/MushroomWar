using UnityEngine;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour, IPooledObject
    {
        public Transform Player { get; set; }
        public PoolObjectType PoolType { get; set; }
        

        public virtual void OnObjectSpawn()
        {

        }

        public virtual void OnObjectDespawn()
        {

        }

        public virtual void Init()
        {

        }

        public void Despawn()
        {
           ObjectPooler.Instance.Despawn(gameObject);
        }
    }
}