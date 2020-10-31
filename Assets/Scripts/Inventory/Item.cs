using System;
using Building;
using Drop;
using ObjectPool;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class Item : PooledObject
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _putRange;
        [SerializeField] private Sprite _sprite = default;

        public Sprite Sprite => _sprite;

        public Collider Collider => _collider;

        public Renderer Renderer => _renderer;

        public float PUTRange => _putRange;

        private void OnTriggerEnter(Collider other)
        {
            
            if (PoolType == PoolObjectType.DarkEssence || PoolType == PoolObjectType.Essence)
            {
                if (other.TryGetComponent(out PlayerHealthScript player))
                {
                    InventoryScript.Instance.AddItem(this);
                    Despawn();
                    return;
                }
                if (PoolType == PoolObjectType.DarkEssence && other.TryGetComponent(out ProducerScript producerScript))
                {
                    StartCoroutine(producerScript.ProduceEssence());
                }
            
                if (PoolType == PoolObjectType.Essence && other.TryGetComponent(out ABuilding building)) //TODO Building.Building?????
                {
                    if ((building.transform.position - transform.position).magnitude < 1.3f)
                    {
                        building.AddEssence();
                        Despawn();
                    }
                }
            }
        }
    }
}