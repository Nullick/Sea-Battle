using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private MapController _mapController;
    [SerializeField] private EnemyMapController _enemyMapController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ProjectileController _projectileController;
    [SerializeField] private AudioController _auidioController;
    private ShipController _playerShipController;
    private ShipController _enemyShipController;

    [Header("Other")]
    [SerializeField] private Board _board;
    [SerializeField] private Board _boardComputer;
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;

    private void Start()
    {
        _mainCamera.Initialize();

        _board.Initialize();
        _mapController.Initialize(_board);
        
        _boardComputer.Initialize();
        _boardComputer.gameObject.SetActive(false);
        _enemyMapController.Initialize(_boardComputer);

        _playerShipController = _mapController.ShipController;
        _enemyShipController = _enemyMapController.ShipController;

        _gameController.Initialize(_playerShipController, _enemyShipController);

        _player.Initialize(_boardComputer, _enemyShipController);
        _enemy.Initialize(_board, _playerShipController);
        _projectileController.Initialized();

        _uiController.Initialize(_enemyShipController);
        _auidioController.Initialize();
    }
}
