using System.Collections.Generic;
using System.Linq;

public abstract class State
{
    protected Enemy enemy;
    protected StateMachine stateMachine;
    protected List<Tile> tiles;
    protected TileController playerTileController;
    protected ShipController shipController;

    protected State(Enemy enemy, StateMachine stateMachine, TileController playerTileController, ShipController shipController)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.tiles = playerTileController.PlacableTiles.Union(playerTileController.UnplacableTiles).ToList();
        this.shipController = shipController;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
}
