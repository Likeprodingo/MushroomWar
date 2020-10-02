using UnityEngine;

namespace ObjectPool
{
   [System.Serializable]
   public class PoolObjects
   {
      public PoolObjectType Tag;
      public GameObject Prefab;
      public int Size;
   }
}
