using UnityEngine;

namespace Inventory
{
    public class Grid : SceneSingleton<Grid>
    {
        [SerializeField] private Vector2Int _gridSize = default; 
        private Item[,] _grid;

        public Vector2Int GridSize => _gridSize;

        public Item[,] Grid1 => _grid;
        
        private void Awake()
        {
            _grid = new Item[_gridSize.x * 2, _gridSize.y * 2];
        }
        
        public bool IsPlaceTaken(int placeX, int placeY)
        {
            if (_grid[placeX + _gridSize.x, placeY + _gridSize.y] == null)
            {
                return false;
            }
            return true;
        }
        public void PlaceBuilding(int placeX, int placeY, Item building)
        {
            _grid[placeX + _gridSize.x, placeY + _gridSize.y] = building;
        }
    }
}    