using System;
using Drop;
using ObjectPool;
using UnityEngine;

namespace Inventory
{
    public class Inventory : SceneSingleton<Inventory>
    {
        [SerializeField] private Slot[] _slots;
        
        private void Start()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].gameObject.SetActive(false);
            }
            
            
            AddItem(ObjectPooler.Instance.SpawnFromPool(PoolObjectType.FlowerDrop)
                .GetComponent<DropScript>());
            AddItem(ObjectPooler.Instance.SpawnFromPool(PoolObjectType.FireMushroomDrop)
                .GetComponent<DropScript>());
        }

        public void AddItem(DropScript dropScript)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Drop != null && _slots[i].Drop.Type == dropScript.Type)
                {
                    _slots[i].Add();
                    return;
                }
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Drop == null)
                {
                    _slots[i].gameObject.SetActive(true);
                    _slots[i].Drop = dropScript;
                    break;
                }
            }
        }
    }
}