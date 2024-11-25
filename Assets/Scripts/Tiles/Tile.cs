using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{
    [SerializeField] private int _x, _z;
    private bool _isChecked = false;

    public Tile Left, Right, Front, Back;

    public int X => _x;
    public int Z => _z;
    public bool IsChecked => _isChecked;

    public Tile(Tile front, Tile back, Tile left, Tile right, int x, int z)
    {
        Front = front;
        Back = back;
        Left = left;
        Right = right;
        this._x = x;
        this._z = z;
    }

    public Tile(int x, int z)
    {
        this._x = x;
        this._z = z;
    }

    public List<Tile> GetTileNeighbors()
    {
        List<Tile> tiles = new List<Tile> { Front, Back, Left, Right };
        tiles.RemoveAll(item => item == null);
        return tiles;
    }

    public bool EqualsCoordinates(Tile tile)
    {
        return this.X == tile.X && this.Z == tile.Z;
    }

    public void ChangeChecked()
    {
        _isChecked = true;
    }
}