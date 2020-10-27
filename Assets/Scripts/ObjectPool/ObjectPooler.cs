using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPooler : SceneSingleton<ObjectPooler>
    {
        [SerializeField] private Transform _player = default;
           public Dictionary<PoolObjectType, Queue<GameObject>> PoolDictionary;
           public List<PoolObjects> Pool;
           private Dictionary<PoolObjectType, int> _poolIndexes = new Dictionary<PoolObjectType, int>();
           private Dictionary<PoolObjectType, Transform> _poolMasters = new Dictionary<PoolObjectType, Transform>();
           
           private void Awake()
           {
               PoolDictionary = new Dictionary<PoolObjectType, Queue<GameObject>>();
               GameObject master = new GameObject("Pool");
               for (int j = 0; j < Pool.Count; j++)
               {
                   GameObject poolSpecifiMaster = new GameObject(Pool[j].Tag.ToString());
                   poolSpecifiMaster.transform.parent = master.transform;
                   Queue<GameObject> objectPool = new Queue<GameObject>();
                   _poolIndexes.Add(Pool[j].Tag, j);
                   _poolMasters.Add(Pool[j].Tag, poolSpecifiMaster.transform);
                   for (int i = 0; i < Pool[j].Size; i++)
                   {
                       GameObject obj = Instantiate(Pool[j].Prefab);
                       obj.transform.parent = poolSpecifiMaster.transform;
                       IPooledObject iPool = obj.GetComponent<IPooledObject>();
                       if (iPool == null)
                       {
                           PooledObject temp = obj.AddComponent<PooledObject>();
                           iPool = temp;
                       }
                       iPool.PoolType = Pool[j].Tag;
                       obj.SetActive(false);
                       objectPool.Enqueue(obj);
                   }
                   PoolDictionary.Add(Pool[j].Tag, objectPool);
               }
           }
           public GameObject SpawnFromPool(PoolObjectType tag, Vector3 pos = default, Quaternion rot = default, GameObject parent = null)
           {
               if (!PoolDictionary.ContainsKey(tag))
               {
                   Debug.Log("PoolObjects with Tag " + tag + " doesn't exist ..");
                   return null;
               }
               GameObject objToSpawn;
                if (PoolDictionary[tag].Count != 0)
                {
                    objToSpawn = PoolDictionary[tag].Peek();
                    objToSpawn.SetActive(true);
                    objToSpawn.transform.position = pos;
                    objToSpawn.transform.rotation = rot;
                    IPooledObject iPooledObj = objToSpawn.GetComponent<IPooledObject>();
                    iPooledObj.Player = _player;
                    iPooledObj.Init();
                    iPooledObj.OnObjectSpawn();

                    PoolDictionary[tag].Dequeue();
                }
                else
                {
                    objToSpawn = ExpandPool(tag, pos, rot);
                }

                if (parent)
                {
                    objToSpawn.transform.SetParent(parent.transform);
                }
                return objToSpawn;
            }

           public void Despawn(GameObject obj)
           {
               PoolObjectType tag = obj.GetComponent<IPooledObject>().PoolType;
               if ( PoolDictionary.ContainsKey(tag))
               {
                   PoolDictionary[tag].Enqueue(obj);
                   IPooledObject iPooledObj = obj.GetComponent<IPooledObject>();
                   if (iPooledObj != null) iPooledObj.OnObjectDespawn();
                   obj.SetActive(false);
               }
               else
               {
                   Debug.LogError("Trying to despawn object which is not pooled !");
               }
           }

           private GameObject ExpandPool(PoolObjectType tag, Vector3 pos, Quaternion rot)
           {
               int index = _poolIndexes[tag];
               GameObject temp = Instantiate(Pool[index].Prefab);
               temp.SetActive(true);
               temp.transform.SetParent(_poolMasters[tag]);
               temp.transform.position = pos;
               temp.transform.rotation = rot;
               IPooledObject iPool = temp.GetComponent<IPooledObject>();
               if (iPool == null)
               {
                   PooledObject tempPool = temp.AddComponent<PooledObject>();
                   iPool = tempPool;
               }
               iPool.PoolType = tag;
               iPool.Player =  _player;
               iPool.Init();
               iPool.OnObjectSpawn();
               PoolDictionary[tag].Enqueue(temp);
               PoolDictionary[tag].Dequeue();
               Pool[index].Size++;
               return temp;
            }
    }
}
