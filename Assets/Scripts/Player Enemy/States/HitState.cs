using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HitState : State
{
    private Tile tile;
    private List<Tile> shipTiles = new();

    public HitState(Enemy enemy, StateMachine stateMashine, TileController playerTileController, ShipController shipController, Tile tile, List<Tile> shipTiles) : base(enemy, stateMashine, playerTileController, shipController)
    {
        this.tile = tile;
        this.playerTileController = playerTileController;
        this.tiles = enemy.Tiles;
        this.shipController = shipController;
        this.shipTiles = shipTiles;

        //shipController.ShipDestroyed += OnShipDestroyed;
    }

    public override void Enter()
    {
        this.tile = enemy.ChosenTile;
        this.tiles = enemy.Tiles;
        this.shipTiles = enemy.ShipTiles;

        if (shipController.IsShipDestroyed)
        {
            enemy.StopCoroutine(ChooseNeighborTiles());
            enemy.StopCoroutine(ChooseTileAfterHit());
            enemy.ChangeDestroyMove();
            shipController.IsShipDestroyed = false;
        }

        if (enemy.Move == Move.Hit || enemy.Move == Move.AfterHitMiss)
        {
            enemy.StartCoroutine(ChooseNeighborTiles());
        }
        else if (enemy.Move == Move.AfterHitHit)
        {
            enemy.StartCoroutine(ChooseTileAfterHit());
        }

        Debug.Log("ShipTileCount " + shipTiles.Count);
    }

    private IEnumerator ChooseNeighborTiles()
    {
        yield return new WaitForSeconds(1f);

        List<Tile> possibleNeighbors = playerTileController.GetPossibleTileNeighbors(tile);
        Debug.Log(possibleNeighbors.Count);
        int randomTileIndex = UnityEngine.Random.Range(0, possibleNeighbors.Count);
        Debug.Log(randomTileIndex);

        Tile chosenTile = possibleNeighbors[randomTileIndex];

        if (playerTileController.IsShipTile(chosenTile))
        {
            ChosenTileHit(chosenTile);
            yield break;
        }
        else
        {
            ProjectileController.Instance.SpawnEnemyProjectile(chosenTile);
            AudioController.Instance.PlayMissSound();

            playerTileController.RemoveTile(chosenTile);
            playerTileController.ChangeTileMat(chosenTile, false);
            enemy.ChangeAfterHitMissMove();
        }
    }

    private IEnumerator ChooseTileAfterHit()
    {
        yield return new WaitForSeconds(1f);

        shipTiles = enemy.ShipTiles;

        foreach (Tile shipTile in shipTiles)
        {
            if (tile.Left == shipTile)
            {
                if (tile.Right != null && playerTileController.IsShipTile(tile.Right))
                {
                    ChosenTileHit(tile.Right);
                }
                else
                {
                    if (playerTileController.IsShipTile(tile.Left.Left))
                    {
                        ChosenTileHit(tile.Left.Left);
                    }
                }
            }

            if (tile.Right == shipTile)
            {
                if (tile.Left != null && playerTileController.IsShipTile(tile.Left))
                {
                    ChosenTileHit(tile.Left);
                }
                else
                {
                    if (playerTileController.IsShipTile(tile.Right.Right))
                    {
                        ChosenTileHit(tile.Right.Right);
                    }
                }
            }

            if (tile.Front == shipTile)
            {
                if (tile.Back != null && playerTileController.IsShipTile(tile.Back))
                {
                    ChosenTileHit(tile.Back);
                }
                else
                {
                    if (playerTileController.IsShipTile(tile.Front.Front))
                    {
                        ChosenTileHit(tile.Front.Front);
                    }
                }
            }

            if (tile.Back == shipTile)
            {
                if (tile.Front != null && playerTileController.IsShipTile(tile.Front))
                {
                    ChosenTileHit(tile.Front);
                }
                else
                {
                    if (playerTileController.IsShipTile(tile.Back.Back))
                    {
                        ChosenTileHit(tile.Back.Back);
                    }
                }
            }
        }
    }

    private void ChosenTileHit(Tile tile)
    {
        ProjectileController.Instance.SpawnEnemyProjectile(tile);
        AudioController.Instance.PlayExplosionSound();

        tile.ChangeChecked();
        playerTileController.ChangeTileMat(tile, true);
        enemy.ChangeAfterHitHitMove(tile);
        shipController.ShipHit(tile);

        if (shipController.IsShipDestroyed)
        {
            enemy.StopCoroutine(ChooseNeighborTiles());
            enemy.StopCoroutine(ChooseTileAfterHit());
            enemy.ChangeDestroyMove();
        }
    }

    //private void OnShipDestroyed()
    //{
    //    //enemy.StopAllCoroutines();
    //    enemy.StopCoroutine(ChooseNeighborTiles());
    //    enemy.StopCoroutine(ChooseTileAfterHit());
    //    enemy.ChangeDestroyMove();
    //    //Exit();
    //}

    public override void Exit()
    {
        Debug.Log("EXIT");
        
    }

    //~HitState()
    //{
    //    shipController.ShipDestroyed -= OnShipDestroyed;
    //}
}
