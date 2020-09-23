using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridBuilding : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _gridSize = default;

    [SerializeField]
    private float _buildRange = default;
    private GameObject _player = default;
    private Building[,] _grid = default;
    private Building _targetBuilding =default;
    private Camera _mainCamera = default;

    public Vector2Int GridSize => _gridSize;

    public float BuildRange => _buildRange;

    public Building[,] Grid => _grid;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        _grid = new Building[_gridSize.x * 2, _gridSize.y * 2];
        _mainCamera = Camera.main;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (!ReferenceEquals(_targetBuilding,null))
        {
            Destroy(_targetBuilding.gameObject);
        }

        _targetBuilding = Instantiate(buildingPrefab);
        _targetBuilding.GetComponentInChildren<CapsuleCollider>().enabled = false;
    }
    
    void Update()
    {
        if (!ReferenceEquals(_targetBuilding,null))
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                int worldPosX = Mathf.RoundToInt(worldPosition.x);
                int worldPosZ = Mathf.RoundToInt(worldPosition.z);
                _targetBuilding.transform.position = worldPosition;
                Vector3 playerPosition = _player.transform.position;
                int x = Mathf.RoundToInt(playerPosition.x);
                int y = Mathf.RoundToInt(playerPosition.z);

                bool available = true;
                
                if ( worldPosX > x + _buildRange)
                {
                    available = false;
                }

                if (
                    worldPosX < x - _buildRange
                    ||worldPosX > x + _buildRange
                    || worldPosX < -_gridSize.x + _targetBuilding.Size.x 
                    || worldPosX > _gridSize.x - _targetBuilding.Size.x
                    ||worldPosZ < y - _buildRange 
                    || worldPosZ < -_gridSize.y + _targetBuilding.Size.y 
                    || worldPosZ > _gridSize.y - _targetBuilding.Size.y
                    || worldPosZ > y + _buildRange
                    || isPlaceTaken(worldPosX, worldPosZ)
                    )
                {
                    available = false;
                }
                
                _targetBuilding.transform.position = new Vector3(worldPosX,_targetBuilding.transform.position.y,worldPosZ);
                _targetBuilding.GetComponent<Building>().SetTransparent(available);

                if (Input.GetMouseButtonDown(0) && available)
                {
                    PlaceBuilding(worldPosX,worldPosZ);
                }
            }
        }
    }

    private bool isPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < _targetBuilding.Size.x; x++)
        {
            for (int y = 0; y < _targetBuilding.Size.y ; y++)
            {
                if (_grid[placeX + _gridSize.x + x, placeY + _gridSize.y + y] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void PlaceBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < _targetBuilding.Size.x; x++)
        {
            for (int y = 0; y < _targetBuilding.Size.y ; y++)
            {
                _grid[placeX + _gridSize.x + x, placeY + _gridSize.y + y] = _targetBuilding;
            }
        }
        _targetBuilding.GetComponentInChildren<CapsuleCollider>().enabled = true;
        _targetBuilding.GetComponent<Building>().SetNormal();
        _targetBuilding = null;
    }
}
