using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Board _playerBoard;
    [SerializeField] private TileController _playerTileController;
    [SerializeField] private Move _move;

    private ShipController _shipController;

    private MoveState _moveState;
    private HitState _hitState;
    private StateMachine _stateMachine;

    private Tile _chosenTile;
    private List<Tile> _shipTiles = new List<Tile>();
    private List<Tile> _tiles = new List<Tile>();

    public event Action EnemyMove;
    public Tile ChosenTile => _chosenTile;
    public Move Move => _move;
    public TileController PlayerTileController => _playerTileController;
    public List<Tile> Tiles => _tiles;
    public List<Tile> ShipTiles => _shipTiles;

    public void Initialize(Board playerBoard, ShipController shipController)
    {
        _shipController = shipController;
        _tiles = _playerTileController.GetAllTiles();

        _playerBoard = playerBoard;
        _move = Move.Miss;

        _stateMachine = new StateMachine();
        _moveState = new MoveState(this, _stateMachine, _playerTileController, _shipController);
        _hitState = new HitState(this, _stateMachine, _playerTileController, _shipController, _chosenTile, _shipTiles);

        GameController.Instance.EnemyMove += OnMakeMove;
    }

    private void OnMakeMove()
    {
        //switch (_move)
        //{
        //    case Move.Destroy:
        //    case Move.Miss:

        //        _shipTiles.Clear();
        //        _tiles = _playerTileController.GetAllTiles();

        //        _stateMachine.Initialize(_moveState);

        //        break;

        //    default:
        //    case Move.Hit:

        //        Debug.Log("SHIP TILES ENEMY = " + _shipTiles.Count);

        //        _tiles = _playerTileController.GetAllTiles();

        //        if (!_shipTiles.Contains(_chosenTile))
        //        {
        //            _shipTiles.Add(_chosenTile);
        //        }

        //        _stateMachine.Initialize(_hitState);

        //        break;
        //}

        _stateMachine.Initialize(_moveState);
    }

    public void ChangeHitMove(Tile tile)
    {
        _move = Move.Hit;
        _chosenTile = tile;
        EnemyMove?.Invoke();
    }

    public void ChangeAfterHitHitMove(Tile tile)
    {
        _move = Move.AfterHitHit;
        _chosenTile = tile;
        EnemyMove?.Invoke();
    }

    public void ChangeAfterHitMissMove()
    {
        _move = Move.AfterHitMiss;
        EnemyMove?.Invoke();
    }

    public void ChangeMissMove()
    {
        _move = Move.Miss;
        EnemyMove?.Invoke();
    }

    public void ChangeDestroyMove()
    {
        _move = Move.Destroy;
        EnemyMove?.Invoke();
        _shipTiles.Clear();
    }

    private void OnDestroy()
    {
        GameController.Instance.EnemyMove -= OnMakeMove;
    }
}
