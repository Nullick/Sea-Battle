using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipClickController : MonoBehaviour
{
   [SerializeField] private List<Ship> _shipList = new List<Ship>();
    private TileController _tileController;

    public event Action<Ship> ShipToChangePlace;
    public event Action AllShipsPlaced;
    public event Action NotAllShipsPlaced;

    public void Initialize(TileController tileController)
    {
        _tileController = tileController;
    }

    private void OnDestroy()
    {
        foreach (Ship ship in _shipList)
            ship.ShipClicked -= OnShipClicked;
        _shipList.Clear();
    }

    public void AddShips(Ship ship)
    {
        _shipList.Add(ship);
        ship.ShipClicked += OnShipClicked;

        if(_shipList.Count == 10)
        {
            AllShipsPlaced?.Invoke();
        }
    }

    public void RemoveShips(Ship ship)
    {
        _shipList.Remove(ship);
        ship.ShipClicked -= OnShipClicked;
        NotAllShipsPlaced?.Invoke();
    }

    private void OnShipClicked(Ship ship)
    {
        ship.IsPlaced = false;
        _tileController.ChangeUnplacableTiles(ship);
        ShipToChangePlace?.Invoke(ship);
    }
}
