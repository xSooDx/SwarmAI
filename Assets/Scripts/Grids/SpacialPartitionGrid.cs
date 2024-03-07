using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SpacialPartitionGrid<T>
{

    public delegate void OnCellOperation(LinkedList<T> list, int neighbourIndex, Vector3Int cellIndex);
    public float m_CellSize { get; private set; }
    public Vector3Int m_GridDimentions { get; private set; }

    private LinkedList<T>[,,] m_GridData;

    public SpacialPartitionGrid(float cellSize, Vector3Int dimentions)
    {
        m_CellSize = cellSize;
        m_GridDimentions = dimentions;

        m_GridData = new LinkedList<T>[dimentions.x, dimentions.y, dimentions.z];
        Grid3DUtilities.ActionOnAllCells(m_GridData, (LinkedList<T> val, int x, int y, int z) =>
        {
            m_GridData[x, y, z] = new LinkedList<T>();
        });
    }

    public LinkedListNode<T> Add(T data, Vector3 position)
    {
        Vector3Int index = Grid3DUtilities.RelativePositionIndex(position, m_CellSize);
        return Add(data, index);
    }

    public LinkedListNode<T> Add(T data, Vector3Int index)
    {
        return m_GridData[index.x, index.y, index.z].AddFirst(data);
    }

    public void Add(LinkedListNode<T> node, Vector3 position)
    {
        Vector3Int index = Grid3DUtilities.RelativePositionIndex(position, m_CellSize);
        Add(node, index);
    }

    public void Add(LinkedListNode<T> node, Vector3Int index)
    {
        m_GridData[index.x, index.y, index.z].AddFirst(node);
    }

    public void Move(LinkedListNode<T> node, Vector3 oldPosition, Vector3 newPosition)
    {
        Vector3Int oldIndex = Grid3DUtilities.RelativePositionIndex(oldPosition, m_CellSize);
        Vector3Int newIndex = Grid3DUtilities.RelativePositionIndex(newPosition, m_CellSize);

        if (oldIndex.Equals(newIndex) || !Grid3DUtilities.IsIndexInsideGrid(m_GridData, newIndex.x, newIndex.y, newIndex.z))
        {
            return;
        }

        node.List.Remove(node);

        Add(node, newIndex);
    }

    public void Remove(LinkedListNode<T> node)
    {
        node.List.Remove(node);
    }

    public void RunOpperationOnCells(Vector3Int[] neighbourOffsetIndices, Vector3 position, OnCellOperation operetionOnCell) => RunOpperationOnCells(neighbourOffsetIndices, Grid3DUtilities.RelativePositionIndex(position, m_CellSize), operetionOnCell);


    public void RunOpperationOnCells(Vector3Int[] neighbourOffsetIndices, Vector3Int index, OnCellOperation operetionOnCell) => RunOpperationOnCells(neighbourOffsetIndices, index.x, index.y, index.z, operetionOnCell);


    public void RunOpperationOnCells(Vector3Int[] neighbourOffsetIndices, int x, int y, int z, OnCellOperation operetionOnCell)
    {
        for (int i = 0; i < neighbourOffsetIndices.Length; i++)
        {
            int x1 = x + neighbourOffsetIndices[i].x;
            int y1 = y + neighbourOffsetIndices[i].y;
            int z1 = z + neighbourOffsetIndices[i].z;
            if (Grid3DUtilities.IsIndexInsideGrid(m_GridData, x1, y1, z1))
            {
                operetionOnCell(m_GridData[x1, y1, z1], i, new Vector3Int(x1, y1, z1));
            }
            else
            {
                operetionOnCell(null, i, new Vector3Int(x1, y1, z1));
            }
        }
    }

    public Vector3 GetPositionOfCell(Vector3Int index) => GetPositionOfCell(index.x, index.y, index.z);

    public Vector3 GetPositionOfCell(int x, int y, int z)
    {
        return Grid3DUtilities.IndexToRelativePosition(m_CellSize, x, y, z);
    }

    public void DrawDebugGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = (Vector3)m_GridDimentions * m_CellSize;
        Vector3 center = size / 2;

        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.white * new Color(1, 1, 1, .05f);

        for (int i = 0; i < m_GridDimentions.x; i++)
        {
            Gizmos.DrawWireCube(new Vector3(m_CellSize * (i + 0.5f), center.y, center.z), new Vector3(m_CellSize, size.y, size.z));
        }

        for (int i = 0; i < m_GridDimentions.y; i++)
        {
            Gizmos.DrawWireCube(new Vector3(center.x, m_CellSize * (i + 0.5f), center.z), new Vector3(size.x, m_CellSize, size.z));
        }

        for (int i = 0; i < m_GridDimentions.z; i++)
        {
            Gizmos.DrawWireCube(new Vector3(center.x, center.y, m_CellSize * (i + 0.5f)), new Vector3(size.x, size.y, m_CellSize));
        }

    }
}
