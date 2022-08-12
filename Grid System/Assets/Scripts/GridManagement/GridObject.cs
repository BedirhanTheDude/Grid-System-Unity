using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GridObject
{
    private float _gridSize; 
    private float _height;
    private float _width;
    private Collider _collider;
    public bool isOccupied = false;

    private float[,] gridList;
    private Vector3 coordinates = Vector3.zero;

    public Collider Collider {
        get { return _collider; } set { _collider = value; }
    }

    public float Size{
        get { return _gridSize; } set { _gridSize = value; }
    }

    private GridObject(float height, float width, float gridSize)
    {
        this._height = height;
        this._width = width;
        this._gridSize = gridSize;
        gridList = new float[((int)height), (int)width];
    }

    public static List<GridObject> GenerateGrids(float height, float width, float gridSize)
    {
        List<GridObject> list = new List<GridObject>();

        for (int x = 0; x < height; x++)
        {
            for (int z = 0; z < width; z++)
            {
                GridObject grid = new GridObject(x, z, gridSize);
                list.Add(grid);
                grid.SignInCoordinates(x, z);
            }
        }
        return list;
    }

    private void SignInCoordinates(int x, int z)
    {
        this.coordinates.x = x;
        this.coordinates.z = z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(coordinates.x, 0, coordinates.z) * _gridSize;
    }

    public void DrawGridGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.GetPosition(), new Vector3(1,0,1) * _gridSize);
    }    
}
