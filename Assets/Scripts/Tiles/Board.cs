using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _firstTile;

    [Header("Corners")]
    [SerializeField] private Transform _leftDownCorner;
    [SerializeField] private Transform _leftUpCorner;
    [SerializeField] private Transform _rightDownCorner;
    [SerializeField] private Transform _rightUpCorner;

    [Header("Lists")]
    [SerializeField] private List<Tile> _tiles = new List<Tile>();
    //[SerializeField] private List<Ship> _shipsOnBoard = new List<Ship>();
    [SerializeField] private List<SceneTile> _sceneTiles = new List<SceneTile>();

    private const double TOLERANCE = .1;

    public List<Tile> Tiles => _tiles;
    //public List<Ship> Ships => _shipsOnBoard;
    public List<SceneTile> SceneTiles => _sceneTiles;

    //public event Action AllShipEnded;

    public void Initialize()
    {
        _firstTile = this.transform.GetChild(0);
        Vector3 position = _firstTile.transform.position;

        Tile tile = new Tile((int)position.x, (int)position.z);
        _tiles.Add(tile);
        AddNeighbours(tile);

        for (int i = 0; i < 100; i++)
        {
            _sceneTiles.Add(transform.GetChild(i).GetComponent<SceneTile>());
        }

        foreach (SceneTile sceneTile in _sceneTiles)
        {
            foreach (Tile tile1 in _tiles)
            {
                if (sceneTile.transform.position == new Vector3(tile1.X, 0f, tile1.Z))
                {
                    sceneTile.Initialize(tile1);
                }
            }
        }
    }

    private void AddNeighbours(Tile tile)
    {
        if (tile.Front == null && tile.Z + 2 <= _leftUpCorner.position.z)
        {
            Tile newTile = _tiles.FirstOrDefault(lTile => Mathf.Abs(lTile.X - tile.X) < TOLERANCE
            && Mathf.Abs(lTile.Z - tile.Z - 2) < TOLERANCE);

            if (newTile == null)
            {
                newTile = new Tile(null, tile, null, null, tile.X, tile.Z + 2);
                _tiles.Add(newTile);
            }

            tile.Front = newTile;
            AddNeighbours(newTile);
        }

        if (tile.Back == null && tile.Z - 2 >= _leftDownCorner.position.z)
        {
            Tile newTile = _tiles.FirstOrDefault(lTile => Mathf.Abs(lTile.X - tile.X) < TOLERANCE
            && Mathf.Abs(lTile.Z - tile.Z + 2) < TOLERANCE);

            if (newTile == null)
            {
                newTile = new Tile(tile, null, null, null, tile.X, tile.Z - 2);
                _tiles.Add(newTile);
            }

            tile.Back = newTile;
            AddNeighbours(newTile);
        }

        if (tile.Left == null && tile.X - 2 >= _leftDownCorner.position.x)
        {
            Tile newTile = _tiles.FirstOrDefault(lTile => Mathf.Abs(lTile.X - tile.X + 2) < TOLERANCE
            && Mathf.Abs(lTile.Z - tile.Z) < TOLERANCE);

            if (newTile == null)
            {
                newTile = new Tile(null, null, null, tile, tile.X - 2, tile.Z);
                _tiles.Add(newTile);
            }

            tile.Left = newTile;
            AddNeighbours(newTile);
        }

        if (tile.Right == null && tile.X + 2 <= _rightDownCorner.position.x)
        {
            Tile newTile = _tiles.FirstOrDefault(lTile => Mathf.Abs(lTile.X - tile.X - 2) < TOLERANCE
            && Mathf.Abs(lTile.Z - tile.Z) < TOLERANCE);

            if (newTile == null)
            {
                newTile = new Tile(null, null, tile, null, tile.X + 2, tile.Z);
                _tiles.Add(newTile);
            }

            tile.Right = newTile;
            AddNeighbours(newTile);
        }
    }

    public Transform GetFirstTilePosition()
    {
        return _firstTile;
    }

    public List<Tile> GetCornerTiles()
    {
        List<Tile> tiles = new List<Tile>();

        foreach (Tile tile in _tiles)
        {
            List<Tile> neighbours = tile.GetTileNeighbors();
            if (neighbours.Count == 2)
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }

    //public void AddShipsOnBoard(Ship ship)
    //{
    //    _shipsOnBoard.Add(ship);
    //}

    //public void RemoveShipOnBoard(Ship ship)
    //{
    //    _shipsOnBoard.Remove(ship);

    //    if(_shipsOnBoard.Count == 0)
    //    {
    //        AllShipEnded?.Invoke();
    //    }
    //}
}
