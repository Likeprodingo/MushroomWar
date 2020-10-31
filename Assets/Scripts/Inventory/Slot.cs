using System;
using System.Collections;
using Drop;
using ObjectPool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Util;
using Image = UnityEngine.UI.Image;


namespace Inventory
{
    public class Slot: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Camera _camera;
        [SerializeField] private Transform _player;
        [SerializeField] private Image _image;
        private DropScript _drop = null;
        private int _count = 0;
        private Item _item;
        private bool isActive =false;

        public Item Item
        {
            get => _item;
            set 
            { 
                Debug.Log(value.PoolType + "устанавливаемся");
                _count = 1;
                _item = value;
                _image.sprite = value.Sprite;
            }
            
        }

        public int Count
        {
            get => _count;
            set => _count = value;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        public void Add()
        {
            _count++;
        }

        public void Get()
        {
            if (_count != 0)
            {
                _count--;
            }
            if (_count == 0)
            {
                _item = null;
                _image.sprite = null;
                gameObject.SetActive(false);
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_item && GameController.IsActive)
            {
                _item.gameObject.SetActive(true);
                isActive = true;
                StartCoroutine(PlacingItem());
            }
        }

        private bool IsPlacingPossible(int worldPosX, int worldPosY)
        {
            var position = _player.transform.position;
            float x = Mathf.RoundToInt(position.x);
            float y = Mathf.RoundToInt(position.z);
            if (worldPosX < x - _item.PUTRange
                || worldPosX > x + _item.PUTRange
                || worldPosY < y - _item.PUTRange
                || worldPosY > y + _item.PUTRange)
            {
                return false;
            }

            if (_item.PoolType != PoolObjectType.Essence && _item.PoolType != PoolObjectType.DarkEssence && Grid.Instance.IsPlaceTaken(worldPosX,worldPosY))
            {
                return false;
            }
            return true;
        }

        public IEnumerator PlacingItem()
        {
            var availible = false;
            var material = _item.Renderer.material;
            Color color = material.color;
            color.a *= 0.3f;
            _item.Collider.enabled = false;
            material.color = color;
            var worldPosX = 0;
            var worldPosZ = 0;
            while (isActive)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit) && hit.collider.name.Equals("Plane"))
                {
                    var worldPosition = hit.point;
                    worldPosX = Mathf.RoundToInt(worldPosition.x);
                    worldPosZ = Mathf.RoundToInt(worldPosition.z);
                    var targetItemTransform = _item.transform;
                    targetItemTransform.position = new Vector3(worldPosX, targetItemTransform.position.y, worldPosZ);
                    availible = IsPlacingPossible(worldPosX, worldPosZ);
                }
                if(!isActive) break;
                yield return new WaitForEndOfFrame();
            }
            color.a /= 0.3f;
            material.color = color;
            if (availible)
            {
                _item.Collider.enabled = true;
                Grid.Instance.PlaceBuilding(worldPosX,worldPosZ,_item);
                Get();
            }
            else
            {
                _item.Despawn();
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            isActive = false;
        }
    }
}    