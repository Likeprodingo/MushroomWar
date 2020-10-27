using System;
using System.Collections;
using Building;
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
                Inventory.Inventory.Instance.AddItem(this);
                Despawn();
                return;
            }
            
            if (Type == PoolObjectType.DarkEssence && other.TryGetComponent(out ProducerScript producerScript))
            {
                StartCoroutine(producerScript.ProduceEssence(this));
            }
            
            if (Type == PoolObjectType.Essence && other.TryGetComponent(out Building.Building building)) //TODO Building.Building?????
            {
                if ((building.transform.position - transform.position).magnitude < 1.3f) //TODO сделать так чтобы ближайшее определяло
                {
                    building.AddEssence();
                    Despawn();
                }
            }
        }
    }
}