using System;
using Drop;
using ObjectPool;
using UnityEngine;

namespace Inventory
{
    public class InventoryScript : SceneSingleton<InventoryScript>
    {
        [SerializeField] private Slot[] _slots;
        
        private void Start()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].gameObject.SetActive(false);
            }
            ObjectPooler.Instance.SpawnFromPool(PoolObjectType.FlowerDrop);
            ObjectPooler.Instance.SpawnFromPool(PoolObjectType.FireMushroomDrop);
            ObjectPooler.Instance.SpawnFromPool(PoolObjectType.DarkEssence);
        }

        public void AddItem(Item item)
        {
            item.gameObject.SetActive(false);
            Debug.Log(item.PoolType + "попали");
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item != null && _slots[i].Item.PoolType == item.PoolType)
                {
                    _slots[i].Add();
                    return;
                }
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item == null)
                {
                    Debug.Log(item.PoolType + "добавляемся");
                    _slots[i].gameObject.SetActive(true);
                    _slots[i].Item = item;
                    break;
                }
            }
        }
        
    }
}