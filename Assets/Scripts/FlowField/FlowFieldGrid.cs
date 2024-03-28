using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class FlowFieldGrid : MonoBehaviour
{
    public Vector2Int m_GridSize = Vector2Int.one * 10;
    public float m_GridCellSize = 1;
    public LayerMask m_GridMask;

    Vector3[,] m_FlowGrid;
    Vector2[,] m_FlowDirection;
    byte[,] m_CostField;
    float[,] m_IntegrationField;
    Queue<Vector2Int> m_IntegrationQueue;

    [Header("DEBUG")]

    public bool debugCostField;
    public bool debugIntegrationField;
    public bool debugFlowField;
    [Space]
    public bool debugTestVector;
    public Vector3 testVec;

    Vector2Int minIndexVal;
    Vector2Int maxIndexVal;


    private void Start()
    {
        CalculateFlowField();
    }

    public void CalculateFlowField()
    {
        m_IntegrationQueue = new Queue<Vector2Int>(m_GridSize.x * m_GridSize.y / 2);
        m_FlowGrid = new Vector3[m_GridSize.x, m_GridSize.y];
        m_IntegrationField = new float[m_GridSize.x, m_GridSize.y];
        m_CostField = new byte[m_GridSize.x, m_GridSize.y];
        m_FlowDirection = new Vector2[m_GridSize.x, m_GridSize.y];

        maxIndexVal = m_GridSize - Vector2Int.one;
        minIndexVal = Vector2Int.zero;

        for (int i = 0; i < m_GridSize.x; i++)
        {
            for (int j = 0; j < m_GridSize.y; j++) m_IntegrationField[i, j] = float.MaxValue;
        }

        //float startTime = Time.realtimeSinceStartup;
        CalculateCostField();
        CalculateIntegrationField();
        CalculateFlowFieldVectors();
        //float endTime = Time.realtimeSinceStartup;
        //Debug.Log($"FlowField Time: {endTime - startTime}");
    }

    void CalculateCostField()
    {

        Grid2DUtilities.ActionOnAllCells(m_FlowGrid, (Vector3 value, int i, int j) =>
        {
            Collider[] obstacles = Physics.OverlapBox(IndexToWorldSpace(i, j), Vector3.one * m_GridCellSize, Quaternion.identity, m_GridMask);
            if (obstacles.Length > 0)
            {
                if (obstacles[0].TryGetComponent<FlowFieldObstacle>(out FlowFieldObstacle ffo))
                {
                    byte cost = ffo.m_FlowFieldModifier;
                    m_CostField[i, j] = cost;
                    if (cost == 0)
                    {
                        m_IntegrationQueue.Enqueue(new Vector2Int(i, j));
                        m_IntegrationField[i, j] = 0;
                    }
                }
                else
                {
                    m_CostField[i, j] = byte.MaxValue;
                }
            }
            else
            {
                m_CostField[i, j] = 1;
            }
        });
    }

    void CalculateIntegrationField()
    {
        while (m_IntegrationQueue.Count > 0)
        {
            Vector2Int index = m_IntegrationQueue.Dequeue();
            foreach (Vector2Int n in Grid2DUtilities.Neighbours8)
            {
                Vector2Int p = n + index;
                if (IsIndexInsideGrid(p))
                {
                    byte cost = m_CostField[p.x, p.y];
                    if (cost < byte.MaxValue)
                    {
                        float value = m_IntegrationField[index.x, index.y] + 0.2f * (n.x & 1) * (n.y & 1); ;
                        value += cost;

                        if (value < m_IntegrationField[p.x, p.y])
                        {
                            m_IntegrationField[p.x, p.y] = value;
                            m_IntegrationQueue.Enqueue(p);
                        }
                    }
                    else
                    {
                        m_IntegrationField[p.x, p.y] = short.MaxValue;
                    }
                }
            }
        }
    }

    void CalculateFlowFieldVectors()
    {
        Grid2DUtilities.ActionOnAllCells(m_FlowGrid, (Vector3 val, int x, int y) =>
        {
            Vector3 flowValue = Vector3.zero;
            float currentCellValue = m_IntegrationField[x, y];
            if (currentCellValue == 0)
            {
                m_FlowGrid[x, y] = Vector3.zero;
                return;
            }
            bool isWall = false;
            bool useMin = false;
            bool isOOB = false;
            float currentMin = float.MaxValue;
            Vector2 minFlowDir = Vector2.zero;
            Vector2 organicFlowDir = Vector2.zero;
            foreach (Vector2Int v in Grid2DUtilities.Neighbours8)
            {
                bool isCellOOB = false;
                Vector2Int idx = new Vector2Int(x + v.x, y + v.y);
                if (!IsIndexInsideGrid(idx.x, idx.y))
                {
                    isCellOOB = isOOB = useMin = true;
                }
                else if(!useMin)
                {
                    isWall = useMin = m_IntegrationField[idx.x, idx.y] == short.MaxValue;
                }

                if (!isCellOOB)
                {
                    //count++;
                    if (useMin)
                    {
                        if(m_IntegrationField[idx.x, idx.y] < currentMin)
                        {
                            currentMin = m_IntegrationField[idx.x, idx.y];
                            minFlowDir = v;
                        }
                        //minFlowDir += (Vector2)v * -m_IntegrationField[idx.x, idx.y];
                    }
                    else
                    {
                        organicFlowDir += (Vector2)v * (1.0f / m_IntegrationField[idx.x, idx.y]);
                    }
                }
            }
            Vector2 vec = (useMin ? minFlowDir : organicFlowDir).normalized;
            m_FlowDirection[x, y] = vec;
            if (isOOB || isWall) vec *= 10f;
            m_FlowGrid[x, y] = new Vector3(vec.x, 0, vec.y);
        });

        //Grid2DUtilities.ActionOnAllCells(m_FlowGrid, (Vector3 val, int x, int y) =>
        //{
        //    Vector3 flowValue = Vector3.zero;
        //    short currentCellValue = m_IntegrationField[x, y];
        //    if (currentCellValue == 0)
        //    {
        //        m_FlowGrid[x, y] = Vector3.zero;
        //        return;
        //    }
        //    float largestDiff = 0;
        //    Vector2Int flowDir = Vector2Int.zero;
        //    for (int i = -1; i <= 1; i++)
        //    {
        //        for (int j = -1; j <= 1; j++)
        //        {
        //            if (IsIndexInsideGrid(x + i, y + j))
        //            {
        //                float diff = currentCellValue;
        //                diff -= m_IntegrationField[x + i, y + j];
        //                diff -= 0.2f * (i & 1) * (j & 1);
        //                if (diff > largestDiff)
        //                {
        //                    largestDiff = diff;
        //                    flowValue = new(i, 0, j);
        //                    flowDir = new Vector2Int(i, j);
        //                }
        //            }
        //        }
        //    }
        //    m_FlowDirection[x, y] = flowDir;
        //    m_FlowGrid[x,y] = flowValue;
        //});
    }


    public Vector2Int WorldPosToCellIndex(Vector3 position)
    {
        Vector3 pos = position - transform.position;
        Vector2Int index = Vector2Int.zero;
        index.x = Mathf.RoundToInt(pos.x / m_GridCellSize);
        index.y = Mathf.RoundToInt(pos.z / m_GridCellSize);

        return index;
    }

    public Vector3 IndexToWorldSpace(int x, int y)
    {
        Vector3 offset = Vector3.zero;
        offset.x = m_GridCellSize * x;
        offset.z = m_GridCellSize * y;
        return transform.position + offset;
    }
    public Vector3 IndexToWorldSpace(Vector2Int index) => IndexToWorldSpace(index.x, index.y);

    public bool IsIndexInsideGrid(int x, int y) => x >= 0 && y >= 0 && x < m_GridSize.x && y < m_GridSize.y;

    public bool IsIndexInsideGrid(Vector2Int index) => IsIndexInsideGrid(index.x, index.y);

    public bool IsWorldPosInsideGrid(Vector3 position) => IsIndexInsideGrid(WorldPosToCellIndex(position));

    private void OnDrawGizmos()
    {

        Vector3 size = new Vector3(m_GridSize.x, 0, m_GridSize.y) * m_GridCellSize;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + size / 2, size);

        //ActionOnAllCells((cellValue, i, j) =>
        //{
        //    Vector3 cellPosition = IndexToWorldSpace(i, j);
        //    Gizmos.DrawWireCube(cellPosition, Vector3.one * m_GridCellSize);

        //    Debug.DrawLine(cellPosition, cellPosition + cellValue * m_GridCellSize, Color.red);
        //    if (m_CostField != null)
        //    {
        //        Gizmos.color = Color.Lerp(Color.green, Color.red, (float)m_CostField[i, j] / sbyte.MaxValue);
        //        Gizmos.DrawCube(cellPosition, Vector3.one * m_GridCellSize);
        //    }
        //});

        if (debugFlowField)
        {

            Grid2DUtilities.ActionOnAllCells(m_FlowGrid, (cellValue, i, j) =>
            {
                Vector3 cellPosition = IndexToWorldSpace(i, j);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(cellPosition, Vector3.one * m_GridCellSize);


                Debug.DrawLine(cellPosition, cellPosition + cellValue * 0.5f * m_GridCellSize, Color.red);
                Debug.DrawLine(cellPosition, cellPosition + new Vector3(m_FlowDirection[i, j].x, 0, m_FlowDirection[i, j].y) * 0.5f * m_GridCellSize, Color.red);
                //Debug.DrawRay(cellPosition, new Vector3(m_FlowDirection[i, j].x, 0, m_FlowDirection[i, j].y) * 0.5f * m_GridCellSize, Color.red);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(cellPosition, .1f);
                //if (m_CostField != null)
                //{
                //    Gizmos.color = Color.red;
                //    Gizmos.DrawCube(cellPosition, Vector3.one * m_GridCellSize);
                //}
            });
        }


        if (debugTestVector)
        {
            Vector2Int a = WorldPosToCellIndex(testVec);

            Vector3 offset = Vector3.zero;
            offset.x = m_GridCellSize * a.x;
            offset.z = m_GridCellSize * a.y;

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(testVec, 1.2f);

            Gizmos.color = IsIndexInsideGrid(a.x, a.y) ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + offset, Vector3.one * m_GridCellSize);

            Gizmos.color = IsWorldPosInsideGrid(testVec) ? Color.cyan : Color.yellow;
            Gizmos.DrawWireCube(testVec, Vector3.one * m_GridCellSize * 0.5f);
        }

        if (debugCostField)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.blue;
            Grid2DUtilities.ActionOnAllCells(m_CostField, (byte value, int i, int j) =>
            {
                Vector3 cellPosition = IndexToWorldSpace(i, j);
                Handles.Label(cellPosition + Vector3.up, "C: " + value, style);
            });
        }

        if (debugIntegrationField)
        {
            GUIStyle style2 = new GUIStyle();
            style2.normal.textColor = Color.white;

            Grid2DUtilities.ActionOnAllCells(m_IntegrationField, (float value, int i, int j) =>
            {
                Vector3 cellPosition = IndexToWorldSpace(i, j);
                Handles.Label(cellPosition + Vector3.up * 2, "I: " + value, style2);
            });

            GUIStyle style3 = new GUIStyle();
            style3.normal.textColor = Color.cyan;

            Grid2DUtilities.ActionOnAllCells(m_IntegrationField, (float value, int i, int j) =>
            {
                Vector3 cellPosition = IndexToWorldSpace(i, j);
                Handles.Label(cellPosition + Vector3.up * 3, "I: " + WorldPosToCellIndex(cellPosition), style3);
            });
        }
    }

    public Vector3 GetValueAtPosition(Vector3 position)
    {
        Vector2Int index = WorldPosToCellIndex(position);
        index.Clamp(minIndexVal, maxIndexVal);
        return m_FlowGrid[index.x, index.y];
    }

    public Vector2 GetDirectionAtIndex(int x, int y) => m_FlowDirection[x, y];

    public Vector2 GetDirectionAtIndex(Vector2Int m_Position) => GetDirectionAtIndex(m_Position.x, m_Position.y);

    internal Vector3 NearestGridPosition(Vector3 point)
    {
        return IndexToWorldSpace(WorldPosToCellIndex(point));
    }

    // ToDo: Find Nearest Position inside grid
}
