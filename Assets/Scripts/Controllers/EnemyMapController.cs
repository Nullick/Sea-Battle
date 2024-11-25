using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMapController : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private TileController _tileController;
    [SerializeField] private ReadyButton _button;
    [Header("Ships")]
    [SerializeField] private Ship _oneTileShip;
    [SerializeField] private Ship _twoTileShip;
    [SerializeField] private Ship _threeTileShip;
    [SerializeField] private Ship _fourTileShip;
    //[SerializeField] private List<Ship> _shipsOnBoard;

    [SerializeField] private List<Ship> _ships;
    private List<Tile> _cornerTiles;
    private Tile _leftDown, _leftUp, _rightDown, _rightUp;
    private Ship _shipToPlace;
    private ShipController _shipController;

    public ShipController ShipController => _shipController;

    public void Initialize(Board board)
    {
        _board = board;
        _tileController.Initialize(_board);

        _button.PlayerReady += OnPlayerReady;

        _cornerTiles = _board.GetCornerTiles();

        _leftDown = _cornerTiles[0];
        _leftUp = _cornerTiles[1];
        _rightUp = _cornerTiles[2];
        _rightDown = _cornerTiles[3];

        _shipController = new ShipController(_tileController);

        _shipToPlace = null;
    }

    private void OnPlayerReady()
    {
        _board.gameObject.SetActive(true);
        CreateShips();
        PlaceShips();
    }

    private void CreateShips()
    {
        for (int i = 0; i < 4; i++)
        {
            Health oneTileShipHealth = new Health(1);
            Ship instanceShip = Instantiate(_oneTileShip);
            instanceShip.Initialize(oneTileShipHealth, ShipType.OneTile);
            _ships.Add(instanceShip);
            _shipToPlace = null;
        }

        for (int i = 0; i < 3; i++)
        {
            Health twoTileShipHealth = new Health(2);
            Ship instanceShip = Instantiate(_twoTileShip);
            instanceShip.Initialize(twoTileShipHealth, ShipType.TwoTile);
            _ships.Add(instanceShip);
        }

        for(int i = 0; i < 2; i++)
        {
            Health threeTileShipHealth = new Health(3);
            Ship instanceShip = Instantiate(_threeTileShip);
            instanceShip.Initialize(threeTileShipHealth, ShipType.ThreeTile);
            _ships.Add(instanceShip);
        }

        Health fourTileShipHealth = new Health(4);
        _shipToPlace = Instantiate(_fourTileShip);
        _shipToPlace.Initialize(fourTileShipHealth, ShipType.FourTile);
        _ships.Add(_shipToPlace);

        _shipToPlace = null;

        foreach(Ship ship in _ships)
        {
            ship.gameObject.SetActive(false);
        }
    }

    private void PlaceShips()
    {
        while(_ships.Count > 0)
        {
            List<Tile> placableTiles = _tileController.PlacableTiles;

            int randomTileIndex = Random.Range(0, placableTiles.Count);

            if(_shipToPlace == null)
            {
                foreach(Ship ship in _ships)
                {
                    _shipToPlace = ship;
                    _shipToPlace.transform.position = new Vector3(placableTiles[randomTileIndex].X, .5f, placableTiles[randomTileIndex].Z);

                    int randomDirection = Random.Range(0, 2);

                    switch(randomDirection)
                    {
                        case 0:
                            _shipToPlace.ChangeDirection(Direction.Left);
                            break;
                        case 1:
                            _shipToPlace.ChangeDirection(Direction.Down);
                            break;
                    }

                    _shipToPlace.AddShipTiles(_shipToPlace.Direction);
                    _ships.Remove(ship);
                    break;
                }
            }

            if (CanBePlaced(_shipToPlace))
            {
                _tileController.AddUnplacableTiles(_shipToPlace);
                //_board.AddShipsOnBoard(_shipToPlace);

                _shipController.AddShip(_shipToPlace);

                _shipToPlace = null;
            }
            else
            {
                _ships.Add(_shipToPlace);
                _shipToPlace = null;
            }
        }
    }

    private bool CanBePlaced(Ship placableShip)
    {
        return IsOnBoard(placableShip)
            && IsPlaceNotTaken(placableShip);
    }

    private bool IsOnBoard(Ship placableShip)
    {
        bool available = true;
        Vector3 shipPosition = placableShip.transform.position;

        switch (placableShip.Direction)
        {
            case Direction.Left:
                if (shipPosition.x + placableShip.Size.x * 2 - 2 > _rightDown.X) available = false;
                break;
            case Direction.Down:
                if (shipPosition.z + placableShip.Size.x * 2 - 2 > _leftUp.Z) available = false;
                break;
        }

        return available;
    }

    private bool IsPlaceNotTaken(Ship placableShip)
    {
        List<Tile> shipTiles = placableShip.ShipTiles;
        List<Tile> unplacableTiles = _tileController.UnplacableTiles;
        
        for(int i = 0; i < unplacableTiles.Count; i++)
        {
            for(int j = 0; j < shipTiles.Count; j++)
            {
                if (unplacableTiles[i].X == shipTiles[j].X 
                    && unplacableTiles[i].Z == shipTiles[j].Z)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OnDestroy()
    {
        _button.PlayerReady -= OnPlayerReady;
    }
}
