using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ReadyButton _readyButton;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;

    private bool _isPlayerTurn = false;
    private bool _isEnemyTurn = false;
    private bool _isGameStarted = false;

    private ShipController _playerShipController;
    private ShipController _enemyShipController;

    public static GameController Instance;
    public event Action EnemyMove;

    public bool IsPlayerTurn => _isPlayerTurn;
    public bool IsEnemyTurn => _isEnemyTurn;
    public bool IsGameStarted => _isGameStarted;

    public void Initialize(ShipController playerShipController, ShipController enemyShipController)
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _playerShipController = playerShipController;
        _enemyShipController = enemyShipController;

        _enemy.EnemyMove += OnEnemyMove;
        _player.PlayerMove += OnPlayerMove;

        _readyButton.PlayerReady += OnPlayerReady;

        _playerShipController.AllShipsEnded += OnAllPlayerShipsEnded;
        _enemyShipController.AllShipsEnded += OnAllComputerShipsEnded;
    }

    private void OnPlayerReady()
    {
        _isGameStarted = true;

        _playerShipController.PlayAllShipIdleAniamtion();
        _enemyShipController.PlayAllShipIdleAniamtion();

        int random = UnityEngine.Random.Range(0, 2);

        if (random == 0)
        {
            _isPlayerTurn = true;
            _isEnemyTurn = false;
            UIController.Instance.PlayerMove();
        }
        else
        {
            _isPlayerTurn = false;
            _isEnemyTurn = true;
            EnemyMove?.Invoke();
            UIController.Instance.EnemyMove();
        }
    }

    private void OnPlayerMove()
    {
        if (_player.Move == Move.Hit)
        {
            _isPlayerTurn = true;
            _isEnemyTurn = false;
            UIController.Instance.PlayerMove();
        }
        else
        {
            _isPlayerTurn = false;
            _isEnemyTurn = true;
            EnemyMove?.Invoke();
            UIController.Instance.EnemyMove();
        }
    }

    private void OnEnemyMove()
    {
        if (_enemy.Move == Move.Hit || _enemy.Move == Move.Destroy || _enemy.Move == Move.AfterHitHit)
        {
            _isEnemyTurn = true;
            _isPlayerTurn = false;
            EnemyMove?.Invoke();
            UIController.Instance.EnemyMove();
        }
        else
        {
            _isEnemyTurn = false;
            _isPlayerTurn = true;
            UIController.Instance.PlayerMove();
        }
    }

    private void OnAllPlayerShipsEnded()
    {
        UIController.Instance.EnemyWon();
    }

    private void OnAllComputerShipsEnded()
    {
        UIController.Instance.PlayerWon();
    }

    private void OnDestroy()
    {
        _readyButton.PlayerReady -= OnPlayerReady;
        _player.PlayerMove -= OnPlayerMove;
        _enemy.EnemyMove -= OnEnemyMove;
        _playerShipController.AllShipsEnded -= OnAllPlayerShipsEnded;
        _enemyShipController.AllShipsEnded -= OnAllComputerShipsEnded;
    }
}
