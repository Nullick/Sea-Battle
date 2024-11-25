using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Board _computerBoard;
    [SerializeField] private TileController _computerTileController;
    [SerializeField] private Move _move;

    private List<Tile> _tiles = new List<Tile>();
    private ShipController _shipController;

    private Camera _mainCamera;

    public event Action PlayerMove;
    public Move Move => _move;
    public TileController ComputerTileController => _computerTileController;

    public void Initialize(Board computerBoard, ShipController shipController)
    {
        _computerBoard = computerBoard;

        _shipController = shipController;

        _mainCamera = Camera.main;
        _move = Move.Miss;
    }

    private IEnumerator ChooseTile()
    {
        var plane = new Plane(Vector3.up, _computerBoard.GetFirstTilePosition().transform.position);
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float position))
        {
            if (Input.GetMouseButtonDown(2))//поменять на лкм
            {
                _tiles = _computerTileController.GetAllTiles();

                Vector3 mousePosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(mousePosition.x);
                int z = Mathf.RoundToInt(mousePosition.z);

                Tile tileToChoose = new Tile(x, z);

                foreach (Tile tile in _tiles)
                {
                    if (tile.EqualsCoordinates(tileToChoose) && !tile.IsChecked)
                    {
                        AudioController.Instance.PlayShotSound();
                        ProjectileController.Instance.SpawnPlayerProjectile(tile);
                        if (_computerTileController.IsShipTile(tileToChoose))
                        {
                            yield return new WaitForSeconds(1);
                            AudioController.Instance.PlayExplosionSound();
                            _move = Move.Hit;
                            PlayerMove?.Invoke();
                            tile.ChangeChecked();
                            _shipController.ShipHit(tile);

                            break;
                        }
                        else
                        {
                            yield return new WaitForSeconds(1);
                            _move = Move.Miss;
                            _computerTileController.ChangeTileMat(tile, false);

                            _computerTileController.RemoveTile(tile);
                            PlayerMove?.Invoke();
                            AudioController.Instance.PlayMissSound();
                        }
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!GameController.Instance.IsPlayerTurn)
        {
            return;
        }

        StartCoroutine(ChooseTile());
    }
}
