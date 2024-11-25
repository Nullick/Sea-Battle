using System;
using System.Collections.Generic;

public class ShipController
{
    private List<Ship> _ships = new List<Ship>();
    private Dictionary<ShipType, int> _shipDictPlace = new Dictionary<ShipType, int>();
    private Dictionary<ShipType, int> _shipsForUI = new Dictionary<ShipType, int>();
    private TileController _tileController;

    public List<Ship> Ships => _ships;
    public Dictionary<ShipType, int> ShipDict => _shipDictPlace;
    public Dictionary<ShipType, int> ShipsForUI => _shipsForUI;

    public event Action AllShipsEnded;
    public event Action ShipDestroyed;

    public bool IsShipDestroyed = false;

    public ShipController(TileController tileController)
    {
        _shipDictPlace.Add(ShipType.OneTile, 4);
        _shipDictPlace.Add(ShipType.TwoTile, 3);
        _shipDictPlace.Add(ShipType.ThreeTile, 2);
        _shipDictPlace.Add(ShipType.FourTile, 1);

        _shipsForUI.Add(ShipType.OneTile, 4);
        _shipsForUI.Add(ShipType.TwoTile, 3);
        _shipsForUI.Add(ShipType.ThreeTile, 2);
        _shipsForUI.Add(ShipType.FourTile, 1);

        _tileController = tileController;
    }

    public void AddShip(Ship ship)
    {
        _ships.Add(ship);
        ship.ShipDestroyed += OnShipDestroyed;

        _shipDictPlace[ship.ShipType]--;
    }

    public void RemoveShip(Ship ship)
    {
        _ships.Remove(ship);
        ship.ShipDestroyed -= OnShipDestroyed;

        _shipDictPlace[ship.ShipType]++;
    }

    public void ShipHit(Tile tile)
    {
        bool isFound = false;

        foreach (Ship ship in _ships)
        {
            //ship.Hit(tile);
            foreach (Tile shipTile in ship.ShipTiles)
            {
                if (shipTile.EqualsCoordinates(tile))
                {
                    ship.Hit(shipTile);
                    _tileController.ChangeTileMat(tile, true);
                    isFound = true;
                    break;
                }
            }
            if (isFound)
            {
                break;
            }
        }

        //_tileController.ChangeTileMat(tile, true);
    }

    private void OnShipDestroyed(Ship destroyedShip)
    {
        _tileController.DeleteShipNeighbors(destroyedShip);
        _ships.Remove(destroyedShip);
        _shipsForUI[destroyedShip.ShipType]--;

        ShipDestroyed?.Invoke();

        if (_ships.Count == 0)
        {
            AllShipsEnded?.Invoke();
        }

        IsShipDestroyed = true;
    }

    public void PlayAllShipIdleAniamtion()
    {
        foreach(Ship ship in _ships)
        {
            ship.StartPlayingAnimation();
        }
    }

    ~ShipController()
    {
        foreach (Ship ship in _ships)
        {
            ship.ShipDestroyed -= OnShipDestroyed;
        }
    }
}
