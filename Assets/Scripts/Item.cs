using System;
using System.Collections;
using ObjectPool;
using UnityEngine;

namespace Interface
{
    public class Item : MonoBehaviour, IPooledObject
    {
        [SerializeField] private PoolObjectType _type = default;
        [SerializeField] private float _putRange = default;
        [SerializeField] private Collider _collider = default;
        [SerializeField] private Renderer _renderer = default;

        private Color _startColor;
        private Camera _camera;
        private bool _isActive = false;
        public PoolObjectType PoolType { get; set; }
        public Transform Player { get; set; }

        private void Start()
        {
            Debug.Log(PoolType);
        }

        public void Init()
        {
            _startColor = _renderer.material.color;
            _camera = Camera.main;
        }

        public void OnObjectSpawn()
        {
            
        }

        public void OnObjectDespawn()
        {
            
        }

        public void Despawn()
        {
            ObjectPooler.Instance.Despawn(gameObject);
        }

        public void Drop()
        {
            _isActive = false;
        }

        public void Take()
        {
            
            _isActive = true;
            StartCoroutine(PlacingItem());
        }
        
        private bool IsPlacingPossible(int worldPosX, int worldPosY)
        {
            float x = Mathf.RoundToInt(Player.transform.position.x);
            float y = Mathf.RoundToInt(Player.transform.position.z);
            if (worldPosX < x - _putRange
                 || worldPosX > x + _putRange
                 || worldPosY < y - _putRange
                 || worldPosY > y + _putRange)
            {
                return false;
            }

            if ((PoolType == PoolObjectType.Flower ||PoolType == PoolObjectType.FireMushroom ||PoolType == PoolObjectType.ElusiveMushroom)  && Grid.Instance.IsPlaceTaken(worldPosX, worldPosY))
            {
                return true;
            }
            return true;
        }

        public IEnumerator PlacingItem()
        {
            var availible = true;
            _collider.enabled = false;
            var worldPosX = 0;
            var worldPosZ = 0;
            while (_isActive)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit) && hit.collider.name.Equals("Plane"))
                {
                    var worldPosition = hit.point;
                    worldPosX = Mathf.RoundToInt(worldPosition.x);
                    worldPosZ = Mathf.RoundToInt(worldPosition.z);
                    transform.position = new Vector3(worldPosX, transform.position.y, worldPosZ);
                    if (IsPlacingPossible(worldPosX,worldPosZ))
                    {
                        availible = true;
                        _renderer.material.color = Color.green;
                    }
                    else
                    {
                        availible = false;
                        _renderer.material.color = Color.red;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            _renderer.material.color = _startColor;
            _collider.enabled = true;
            Grid.Instance.PlaceBuilding(worldPosX,worldPosZ,this);
            if (!availible)
            {
                Despawn();
            }
        }
    }
}