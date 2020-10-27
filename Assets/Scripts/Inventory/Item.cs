using ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class Item : PooledObject
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _putRange;

        public Collider Collider => _collider;

        public Renderer Renderer => _renderer;

        public float PUTRange => _putRange;
    }
}