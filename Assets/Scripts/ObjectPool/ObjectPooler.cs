using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPooler : SceneSingleton<ObjectPooler>
    {
            public Dictionary<PoolObjectType, Queue<PooledObject>> PoolDictionary;
           public List<PoolObjects> Pool;
           private Dictionary<PoolObjectType, int> _poolIndexes = new Dictionary<PoolObjectType, int>();
           private Dictionary<PoolObjectType, Transform> _poolMasters = new Dictionary<PoolObjectType, Transform>();
           
           private void Awake()
           {
               PoolDictionary = new Dictionary<PoolObjectType, Queue<PooledObject>>();
               GameObject master = new GameObject("Pool");
               for (int j = 0; j < Pool.Count; j++)
               {
                   GameObject poolSpecifiMaster = new GameObject(Pool[j].Tag.ToString());
                   poolSpecifiMaster.transform.parent = master.transform;
                   Queue<PooledObject> objectPool = new Queue<PooledObject>();
                   _poolIndexes.Add(Pool[j].Tag, j);
                   _poolMasters.Add(Pool[j].Tag, poolSpecifiMaster.transform);
                   for (int i = 0; i < Pool[j].Size; i++)
                   {
                       GameObject obj = Instantiate(Pool[j].Prefab);
                       obj.transform.parent = poolSpecifiMaster.transform;
                       PooledObject iPool = obj.GetComponent<PooledObject>();
                       if (iPool == null)
                       {
                           iPool = obj.AddComponent<PooledObject>();
                       }
                       iPool.PoolType = Pool[j].Tag;
                       obj.SetActive(false);
                       objectPool.Enqueue(iPool);
                   }
                   PoolDictionary.Add(Pool[j].Tag, objectPool);
               }
           }
           public PooledObject SpawnFromPool(PoolObjectType tag, Vector3 pos = default, Quaternion rot = default, GameObject parent = null)
           {
               if (!PoolDictionary.ContainsKey(tag))
               {
                   Debug.Log("PoolObjects with Tag " + tag + " doesn't exist ..");
                   return null;
               }
               PooledObject objToSpawn;
                if (PoolDictionary[tag].Count != 0)
                {
                    objToSpawn = PoolDictionary[tag].Peek();
                    objToSpawn.gameObject.SetActive(true);
                    var transform1 = objToSpawn.transform;
                    transform1.position = pos;
                    transform1.rotation = rot;
                    objToSpawn.Init();
                    objToSpawn.OnObjectSpawn();
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

           public void Despawn(PooledObject obj)
           {
               PoolObjectType tag = obj.GetComponent<IPooledObject>().PoolType;
               if ( PoolDictionary.ContainsKey(tag))
               {
                   PoolDictionary[tag].Enqueue(obj); 
                   obj.OnObjectDespawn();
                   obj.gameObject.SetActive(false);
               }
               else
               {
                   Debug.LogError("Trying to despawn object which is not pooled !");
               }
           }

           private PooledObject ExpandPool(PoolObjectType tag, Vector3 pos, Quaternion rot)
           {
               int index = _poolIndexes[tag];
               GameObject temp = Instantiate(Pool[index].Prefab, _poolMasters[tag], true);
               temp.SetActive(true);
               temp.transform.position = pos;
               temp.transform.rotation = rot;
               PooledObject iPool = temp.GetComponent<PooledObject>();
               if (iPool == null)
               {
                   iPool = temp.AddComponent<PooledObject>();
               }
               iPool.PoolType = tag;
               iPool.Init();
               iPool.OnObjectSpawn();
               PoolDictionary[tag].Enqueue(iPool);
               PoolDictionary[tag].Dequeue();
               Pool[index].Size++;
               return iPool;
            }
    }
}
