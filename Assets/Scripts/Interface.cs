using System;
using ObjectPool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface
{
    public class Interface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private PoolObjectType _type;
        
        private Item _pickedItem;

        public void OnPointerDown(PointerEventData eventData)
        {
            _pickedItem = ObjectPooler.Instance.SpawnFromPool(_type).GetComponent<Item>();
            _pickedItem.Take();
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _pickedItem.Drop();
        }
    }
}