using UnityEngine;
using UnityEngine.Serialization;

public static class PoolManager
{
    private static PoolPart[] _pools;
    private static GameObject _objectsParent;
    private static Transform _player;
    
    [System.Serializable]
    public struct PoolPart {
        public string _name;
        public PoolObject _prefab; 
        public int _count;
        public ObjectPooling Pool;
    }
    
    public static void Initialize(PoolPart[] newPools,Transform player)
    {
        _player = player;
        _pools = newPools;
        _objectsParent = new GameObject {name = "Pool"};
        for (int i=0; i<_pools.Length; i++) {
            if(_pools[i]._prefab!=null) {  
                _pools[i].Pool = new ObjectPooling();
                _pools[i].Pool.Initialize(_pools[i]._count, _pools[i]._prefab, _objectsParent.transform);
            }
        }
    }
    public static GameObject GetObject (string name, Vector3 position = default, Quaternion rotation = default) {
        GameObject result = null;
        if (_pools != null) {
            for (int i = 0; i < _pools.Length; i++) {
                if (string.Compare (_pools[i]._name, name) == 0) {
                    result = _pools[i].Pool.GetObject().gameObject;
                    result.transform.position = position;
                    result.transform.rotation = rotation; 
                    result.SetActive (true);
                    if (result.GetComponent<EnemyFollow>())
                    {
                        result.GetComponent<EnemyFollow>().StartMoving(_player);
                    }
                    break;
                }
            }
        } 
        return result;
    }
}
