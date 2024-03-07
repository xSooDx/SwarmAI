using Unity.VisualScripting;
using UnityEngine;

public static class Grid2DUtilities
{
    public delegate T SetGridValueDelegate<T>(int i, int j);
    public delegate void ActionOnGridValueDelegate<T>(T value, int i, int j);

    public static void ActionOnAllCells<T>(T[,] grid, ActionOnGridValueDelegate<T> action)
    {
        if (grid == null) return;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                action(grid[i, j], i, j);
            }
        }
    }

    public static Vector2Int[] Neighbours4 = new Vector2Int[4] {
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
    };


    public static Vector2Int[] Neighbours8 = new Vector2Int[8] {
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
    };

    public static bool IsIndexInsideGrid<T>(T[,] grid, int x, int y) => x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1);

    public static bool IsIndexInsideGrid<T>(T[,] grid, Vector2Int index) => IsIndexInsideGrid(grid, index.x, index.y);
}


public static class Grid3DUtilities
{
    public delegate T SetGridValueDelegate<T>(int x, int y, int z);
    public delegate void ActionOnGridValueDelegate<T>(T value, int x, int y, int z);

    public static void ActionOnAllCells<T>(T[,,] grid, ActionOnGridValueDelegate<T> action)
    {
        if (grid == null) return;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int k = 0; k < grid.GetLength(2); k++)
                {
                    action(grid[i, j, k], i, j, k);
                }
            }
        }
    }

    public static readonly Vector3Int[] NeighbourIndex = new Vector3Int[27] {
        new Vector3Int (-1, -1, -1),
        new Vector3Int(-1, -1, 0),
        new Vector3Int(-1, -1, 1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 1, -1),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 1, 1),

        new Vector3Int(0, -1, -1),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, -1, 1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 1, 1),

        new Vector3Int(1, -1, -1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(1, -1, 1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 1, -1),
        new Vector3Int(1, 1, 0),
        new Vector3Int(1, 1, 1),
    };

    public static Vector3Int[] Neighbours2D4 = new Vector3Int[4] {
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1,0, 0),
        new Vector3Int(0,0, -1),
        new Vector3Int(0,0, 1),
    };


    public static Vector3Int[] Neighbours2D8 = new Vector3Int[8] {
        new Vector3Int(-1,0, -1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int(0,0, -1),
        new Vector3Int(0,0, 1),
        new Vector3Int(1,0, -1),
        new Vector3Int(1,0, 0),
        new Vector3Int(1,0, 1),
    };

    public static bool IsIndexInsideGrid<T>(T[,,] grid, int x, int y, int z) => x >= 0 && y >= 0 && z >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1) && z < grid.GetLength(2);

    public static bool IsIndexInsideGrid<T>(T[,,] grid, Vector3Int index) => IsIndexInsideGrid(grid, index.x, index.y, index.z);

    public static Vector3 IndexToRelativePosition(float cellSize, int x, int y, int z)
    {
        return new Vector3(cellSize * x, cellSize * y, cellSize * z);
    }

    public static Vector3Int RelativePositionIndex(Vector3 relativePositon, float cellSize)
    {

        relativePositon /= cellSize;
        return new Vector3Int((int)relativePositon.x, (int)relativePositon.y, (int)relativePositon.z);
    }


    public static readonly Vector3Int[] SwarmNeighbourIndexInOrder = new Vector3Int[27] {
        new Vector3Int(0, 0, 0),

        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),

        new Vector3Int(-1, 0, -1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, 0, 1),

        new Vector3Int(1, 1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1, 0),

        new Vector3Int(0, 1, 1),
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, -1, 1),
        new Vector3Int(0, -1, -1),

        new Vector3Int(-1, -1, -1),
        new Vector3Int(1, -1, -1),
        new Vector3Int(-1, 1, -1),
        new Vector3Int(1, 1, -1),
        new Vector3Int(-1, -1, 1),
        new Vector3Int(1, -1, 1),
        new Vector3Int(-1, 1, 1),
        new Vector3Int(1, 1, 1),
    };

    public static readonly Vector3Int[] SwarmNeighbourIndexInOrder2D = new Vector3Int[9] {
         new Vector3Int(0, 0, 0),

        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, 0, 1),
    };
}