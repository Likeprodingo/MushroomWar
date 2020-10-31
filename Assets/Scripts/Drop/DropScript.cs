using System;
using System.Collections;
using Building;
using Inventory;
using ObjectPool;
using Player;
using UnityEngine;
using Util;

namespace Drop
{
    public class DropScript : PooledObject
    {
        [SerializeField] private Sprite _sprite = default;
        [SerializeField] private PoolObjectType _type  = default;

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        public PoolObjectType Type
        {
            get => _type;
            set => _type = value;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealthScript player))
            {
                Item item = ObjectPooler.Instance.SpawnFromPool(Type) as Item;
                InventoryScript.Instance.AddItem(item);
                Despawn();
            }
        }
    }
}