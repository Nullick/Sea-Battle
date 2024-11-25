using System.Collections;
using UnityEngine;

public class MoveState : State
{
    private Tile chosenTile;

    public MoveState(Enemy enemy, StateMachine stateMachine, TileController playerTileController, ShipController shipController) : base(enemy, stateMachine, playerTileController, shipController)
    {
        this.playerTileController = playerTileController;
        this.tiles = enemy.Tiles;
        this.shipController = shipController;

        //shipController.ShipDestroyed += OnShipDestroyed;
    }

    public override void Enter()
    {
        tiles = enemy.Tiles;

        int randomTileIndex = Random.Range(0, tiles.Count);
        chosenTile = tiles[randomTileIndex];

        ProjectileController.Instance.SpawnEnemyProjectile(chosenTile);

        if (!chosenTile.IsChecked)
        {
            AudioController.Instance.PlayShotSound();
            if (playerTileController.IsShipTile(chosenTile))
            {

                //chosenTile.ChangeChecked();
                //shipController.ShipHit(chosenTile);
                //AudioController.Instance.PlayExplosionSound();
                //Exit();

                //if (shipController.IsShipDestroyed)
                //{
                //    enemy.ChangeDestroyMove();
                //    shipController.IsShipDestroyed = false;
                //}
                enemy.StartCoroutine(ChooseHitTile());
            }
            else
            {
                enemy.StartCoroutine(ChooseTile());
            }
        }
    }

    private IEnumerator ChooseHitTile()
    {
        yield return new WaitForSeconds(1f);
        chosenTile.ChangeChecked();
        shipController.ShipHit(chosenTile);
        AudioController.Instance.PlayExplosionSound();
        playerTileController.RemoveTile(chosenTile);
        playerTileController.ChangeTileMat(chosenTile, true);
        //enemy.ChangeHitMove(chosenTile);
        Exit();
    }

    private IEnumerator ChooseTile()
    {
        yield return new WaitForSeconds(1f);
        AudioController.Instance.PlayMissSound();
        enemy.ChangeMissMove();
        playerTileController.ChangeTileMat(chosenTile, false);
        playerTileController.RemoveTile(chosenTile);
        Exit();
    }

    //private void OnShipDestroyed()
    //{
    //    enemy.ChangeDestroyMove();
    //    Exit();
    //}

    public override void Exit()
    {
        //if (enemy.Move == Move.Destroy)
        //{
        //    enemy.ChangeDestroyMove();
        //}

        if (!chosenTile.IsChecked)
        {
            enemy.ChangeMissMove();
        }
        else
        {
            enemy.ChangeHitMove(chosenTile);
        }
    }

    //~MoveState()
    //{
    //    shipController.ShipDestroyed -= OnShipDestroyed;
    //}
}
