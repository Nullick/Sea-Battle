using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private List<Tile> _placableTiles = new List<Tile>();
    [SerializeField] private List<Tile> _unplacableTiles = new List<Tile>();
    [SerializeField] private List<Tile> _shipTiles = new List<Tile>();

    private Board _board;
    private List<SceneTile> _sceneTiles = new List<SceneTile>();

    public List<Tile> PlacableTiles => _placableTiles;
    public List<Tile> UnplacableTiles => _unplacableTiles;

    public void Initialize(Board board)
    {
        _board = board;
        _placableTiles = _board.Tiles;
        _sceneTiles = _board.SceneTiles;
    }

    public void AddUnplacableTiles(Ship placableShip)
    {
        List<Tile> shipTiles = placableShip.ShipTiles;

        for (int i = 0; i < _placableTiles.Count; i++)
        {
            for (int j = 0; j < shipTiles.Count; j++)
            {
                if (_placableTiles[i].EqualsCoordinates(shipTiles[j]))
                {
                    _unplacableTiles.Add(_placableTiles[i]);
                    _shipTiles.Add(_placableTiles[i]);
                }
            }
        }

        _placableTiles.RemoveAll(tile => _unplacableTiles.Contains(tile));
        AddNeighbors(shipTiles);
    }

    private void AddNeighbors(List<Tile> shipTiles)
    {
        HashSet<Tile> neighborTiles = new HashSet<Tile>();
        List<Tile> listNeighbors = new List<Tile>();

        foreach (Tile shipTile in _shipTiles)
        {
            foreach (Tile tile in shipTiles)
            {
                if (shipTile.EqualsCoordinates(tile))
                {
                    listNeighbors = shipTile.GetTileNeighbors();

                    foreach (Tile neighbor in listNeighbors)
                    {
                        neighborTiles.Add(neighbor);
                    }
                }
            }
        }

        foreach (Tile tile in _shipTiles)
        {
            neighborTiles.Remove(tile);
        }

        foreach (Tile tile in neighborTiles)
        {
            _unplacableTiles.Add(tile);
        }

        _placableTiles.RemoveAll(tile => _unplacableTiles.Contains(tile));
    }

    public void ChangeUnplacableTiles(Ship placableShip)
    {
        List<Tile> shipTilesToReplace = placableShip.ShipTiles;
        List<Tile> tilesToRemove = new List<Tile>();
        HashSet<Tile> neighborsToRemove = new HashSet<Tile>();
        List<Tile> neighbors = new List<Tile>();

        for (int i = 0; i < _shipTiles.Count; i++)
        {
            for (int j = 0; j < shipTilesToReplace.Count; j++)
            {
                if (shipTilesToReplace[j].EqualsCoordinates(_shipTiles[i]))
                {
                    neighbors = _shipTiles[i].GetTileNeighbors();

                    foreach (Tile tile in neighbors)
                    {
                        neighborsToRemove.Add(tile);
                        neighborsToRemove.Add(_shipTiles[i]);
                    }
                }
            }
        }

        foreach (Tile tile in neighborsToRemove)
        {
            _placableTiles.Add(tile);
        }

        _unplacableTiles.RemoveAll(tile => neighborsToRemove.Contains(tile));
        _shipTiles.RemoveAll(tile => neighborsToRemove.Contains(tile));
    }

    public bool IsShipTile(Tile tile)
    {
        foreach (Tile shipTiles in _shipTiles)
        {
            if (tile.X == shipTiles.X && tile.Z == shipTiles.Z)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeTileMat(Tile tile, bool isHit)
    {
        foreach (SceneTile sceneTile in _sceneTiles)
        {
            if (tile == sceneTile.Tile)
            {
                if (isHit)
                {
                    sceneTile.ChangeMaterialHit();
                    break;
                }
                else if (isHit == false)
                {
                    sceneTile.ChangeMaterialMiss();
                    break;
                }
            }
        }
    }

    public void DeleteShipNeighbors(Ship ship)
    {
        HashSet<Tile> neighborTiles = new HashSet<Tile>();
        List<Tile> listNeighbors = new List<Tile>();

        foreach (Tile shipTile in _shipTiles)
        {
            foreach (Tile tile in ship.ShipTiles)
            {
                if (shipTile.EqualsCoordinates(tile))
                {
                    listNeighbors = shipTile.GetTileNeighbors();

                    foreach (Tile neighbor in listNeighbors)
                    {
                        neighborTiles.Add(neighbor);
                    }
                }
            }
        }

        foreach (Tile tile in _shipTiles)
        {
            neighborTiles.Remove(tile);
        }

        foreach (Tile shipTiles in ship.ShipTiles)
        {
            RemoveTile(shipTiles);
        }

        foreach (SceneTile sceneTile in _sceneTiles)
        {
            foreach (Tile tile in neighborTiles)
            {
                if (tile == sceneTile.Tile)
                {
                    sceneTile.ChangeMaterialMiss();
                    //_placableTiles.Remove(tile);
                    RemoveTile(tile);
                }
            }
        }
    }

    public List<Tile> GetPossibleTileNeighbors(Tile tile)
    {
        List<Tile> tiles = new List<Tile>();

        List<Tile> neighbors = tile.GetTileNeighbors();

        foreach(Tile neighbor in neighbors)
        {
            if (GetAllTiles().Contains(neighbor))
            {
                tiles.Add(neighbor);
            }
        }

        return tiles;
    }

    public void RemoveTile(Tile tile)
    {
        _placableTiles.Remove(tile);
        _unplacableTiles.Remove(tile);
        _shipTiles.Remove(tile);
    }

    public List<Tile> GetAllTiles()
    {
        return _placableTiles.Union(_unplacableTiles).Union(_shipTiles).ToList();
    }
}
