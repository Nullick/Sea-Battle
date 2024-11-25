using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = Vector2Int.one;
    [SerializeField] private Health _health;
    [SerializeField] private Direction _direction;
    [SerializeField] private ShipType _shipType;
    [SerializeField] private List<Tile> _shipTiles = new List<Tile>();
    [SerializeField] private Animation _animation;
    [SerializeField] private ParticleSystem _explosion;

    public Vector2Int Size => _size;
    public Direction Direction => _direction;
    public ShipType ShipType => _shipType;
    public List<Tile> ShipTiles => _shipTiles;

    public bool IsPlaced;
    public event Action<Ship> ShipClicked;
    public event Action<Ship> ShipDestroyed;

    public void Initialize(Health health, ShipType shipType)
    {
        _health = health;
        _health.Died += OnShipDied;

        _direction = Direction.Left;
        _shipType = shipType;
        _shipTiles.Clear();
        IsPlaced = false;
        _explosion.Stop();
    }

    public void ChangeDirection(Direction direction)
    {
        if (direction == Direction.Left)
        {
            _direction = Direction.Down;
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (direction == Direction.Down)
        {
            _direction = Direction.Left;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void AddShipTiles(Direction direction)
    {
        Tile tile = new Tile((int)transform.position.x, (int)transform.position.z);
        _shipTiles.Clear();

        switch (direction)
        {
            case Direction.Left:
                for (int i = 0; i < _size.x; i++)
                {
                    _shipTiles.Add(tile);
                    tile = new Tile(tile.X + 2, tile.Z);
                }
                break;
            case Direction.Down:
                for (int i = 0; i < _size.x; i++)
                {
                    _shipTiles.Add(tile);
                    tile = new Tile(tile.X, tile.Z + 2);
                }
                break;
        }
    }

    private void OnMouseOver()
    {
        if (!GameController.Instance.IsGameStarted)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (IsPlaced)
                {
                    ShipClicked?.Invoke(this);
                }
            }
        }
    }

    public void Hit(Tile tile)
    {
        foreach (Tile shipTile in _shipTiles)
        {
            if (shipTile.EqualsCoordinates(tile))
            {
                _explosion.Play();
                _health.Reduce();
            }
        }
    }

    public void StartPlayingAnimation()
    {
        _animation = this.GetComponentInChildren<Animation>();
        _animation.Play("Idle");
        this.transform.position = new Vector3(this.transform.position.x, -.3f, this.transform.position.z);

        switch (_direction)
        {
            case Direction.Left:
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case Direction.Down:
                this.transform.rotation = Quaternion.Euler(0, -180, 0);
                break;
        }

    }

    private void OnShipDied()
    {
        this.gameObject.SetActive(true);
        ShipDestroyed?.Invoke(this);
        _animation.Play("Sinking");
    }

    private void OnDestroy()
    {
        _health.Died -= OnShipDied;
    }
}
