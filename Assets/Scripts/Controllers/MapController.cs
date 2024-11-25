using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private TileController _tileController;
    [SerializeField] private ShipClickController _shipClickController;
    [SerializeField] private ReadyButton _readyButton;

    private Ship _flyingShip;
    private ShipController _shipController;

    private List<Tile> _cornerTiles;
    private Tile _leftDown, _leftUp, _rightDown, _rightUp;

    private Camera _mainCamera;

    private bool isPlayerReady = false;

    public ShipController ShipController => _shipController;

    public void Initialize(Board board)
    {
        _mainCamera = Camera.main;
        _board = board;
        _tileController.Initialize(_board);

        _cornerTiles = _board.GetCornerTiles();

        _leftDown = _cornerTiles[0];
        _leftUp = _cornerTiles[1];
        _rightUp = _cornerTiles[2];
        _rightDown = _cornerTiles[3];//придумать через цикл

        _shipClickController.Initialize(_tileController);
        _shipClickController.ShipToChangePlace += OnShipClicked;

        _readyButton.PlayerReady += OnPlayerReady;

        _shipController = new ShipController(_tileController);
    }

    public void StartPlacing(Ship shipPrefab)
    {
        if (_flyingShip != null)
        {
            Destroy(_flyingShip.gameObject);
        }

        if (_shipController.ShipDict[shipPrefab.ShipType] <= 0)
        {
            Debug.Log("Enough Ships of this type");
        }
        else
        {
            _flyingShip = Instantiate(shipPrefab);

            switch(shipPrefab.ShipType)
            {
                case ShipType.FourTile:

                    Health fourTileShipHealth = new Health(4);
                    _flyingShip.Initialize(fourTileShipHealth, ShipType.FourTile);

                    break;

                case ShipType.ThreeTile:

                    Health threeTileShipHealth = new Health(3);
                    _flyingShip.Initialize(threeTileShipHealth, ShipType.ThreeTile);

                    break;

                case ShipType.TwoTile:

                    Health twoTileShipHealth = new Health(2);
                    _flyingShip.Initialize(twoTileShipHealth, ShipType.TwoTile);

                    break;

                case ShipType.OneTile:

                    Health oneTileShipHealth = new Health(1);
                    _flyingShip.Initialize(oneTileShipHealth, ShipType.OneTile);

                    break;
            }
        }
    }

    private void PlaceShip()
    {
        if (_flyingShip != null)
        {
            var plane = new Plane(Vector3.up, _board.GetFirstTilePosition().transform.position);
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float position))
            {
                Vector3 mousePosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(mousePosition.x);
                int z = Mathf.RoundToInt(mousePosition.z);
                _flyingShip.transform.position = new Vector3(x, 0f, z);

                if (Input.GetMouseButtonDown(0))
                {
                    _flyingShip.AddShipTiles(_flyingShip.Direction);

                    if (CanBePlaced(_flyingShip))
                    {
                        _tileController.AddUnplacableTiles(_flyingShip);

                        _flyingShip.IsPlaced = true;
                        _shipClickController.AddShips(_flyingShip);
                        //_board.AddShipsOnBoard(_flyingShip);

                        _shipController.AddShip(_flyingShip);

                        _flyingShip = null;
                    }
                }
            }
        }
    }

    private bool CanBePlaced(Ship placableShip)
    {
        return IsOnBoard(placableShip.transform.position, placableShip.Size, placableShip.Direction)
            && IsEnoughShips(placableShip.ShipType)
            && IsPlaceNotTaken(_flyingShip)
            && IsOnTile(placableShip.transform.position)
            && placableShip.IsPlaced == false;
    }

    private bool IsPlaceNotTaken(Ship placableShip)
    {
        List<Tile> shipTiles = placableShip.ShipTiles;
        List<Tile> unplacableTiles = _tileController.UnplacableTiles;
        bool available = true;

        foreach(Tile unplacableTile in unplacableTiles)
        {
            foreach(Tile shipTile in shipTiles)
            {
                if(unplacableTile.X == shipTile.X
                    && unplacableTile.Z == shipTile.Z)
                {
                    available = false; break;
                }
            }
        }

        return available;
    }

    private bool IsOnTile(Vector3 shipPosition)
    {
        if (shipPosition.x % 2 == 0 || shipPosition.z % 2 == 0)//переделать на проверку по тайлу (box collider)
        {
            return false;
        }

        return true;
    }

    private bool IsOnBoard(Vector3 shipPosition, Vector2Int shipSize, Direction direction)
    {
        bool available = true;

        switch (direction)
        {
            case Direction.Down:
                if (shipPosition.z < _leftDown.Z || shipPosition.z + shipSize.x * 2 - 2 > _leftUp.Z) available = false;
                if (shipPosition.x < _leftDown.X || shipPosition.x > _rightDown.X) available = false;
                break;
            case Direction.Left:
                if (shipPosition.x < _leftDown.X || shipPosition.x + shipSize.x * 2 - 2 > _rightDown.X) available = false;
                if (shipPosition.z < _leftDown.Z || shipPosition.z > _leftUp.Z) available = false;
                break;
        }

        return available;
    }

    private bool IsEnoughShips(ShipType shipType)
    {
        foreach (var shipTypeDict in _shipController.ShipDict)
        {
            if (_shipController.ShipDict[shipType] > 0)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        PlaceShip();

        if (Input.GetKeyDown(KeyCode.R) && _flyingShip != null)
        {
            _flyingShip.ChangeDirection(_flyingShip.Direction);
        }
    }

    private void OnPlayerReady()
    {
        isPlayerReady = true;
        this.transform.gameObject.SetActive(false);
    }

    private void OnShipClicked(Ship ship)
    {
        if(isPlayerReady == false)
        {
            if(_flyingShip == null)
            {
                _flyingShip = ship;
                _shipClickController.RemoveShips(_flyingShip);
                //_board.RemoveShipOnBoard(_flyingShip);
                _shipController.RemoveShip(_flyingShip);
            }
        }
    }

    private void OnDestroy()
    {
        _shipClickController.ShipToChangePlace -= OnShipClicked;
        _readyButton.PlayerReady -= OnPlayerReady;
    }
}
