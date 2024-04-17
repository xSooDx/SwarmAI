using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{

    public static SwarmManager Instance { get; private set; }

    public SwarmEntity m_SwarmEntityPrefab;
    public FlowFieldGrid m_FlowFieldGrid;
    public Vector3Int m_SimulationGridSize = new Vector3Int(100, 100, 1);
    public float m_SwarmPartitionGridCellSize;
    public SwarmSettings SwarmSettings;
    [Header("DEBUG")]
    public bool showGrid;

    public SwarmSettings SettingsClone { get; private set; }

    HashSet<SwarmEntity> m_SwarmSet;
    public SpacialPartitionGrid<SwarmEntity> m_PartitionGrid { get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        m_PartitionGrid = new SpacialPartitionGrid<SwarmEntity>(m_SwarmPartitionGridCellSize, m_SimulationGridSize);
        m_SwarmSet = new HashSet<SwarmEntity>();
        SettingsClone = SwarmSettings.Clone();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UpdateLoop());
        m_FlowFieldGrid.CalculateFlowField();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var entity in m_SwarmSet)
        {
            Vector3 oldPosition = entity.transform.position;
            // MoveEntity;
            entity.UpdateTick();
            m_PartitionGrid.Move(entity.node, oldPosition, entity.transform.position);
        }
    }

    public void AddSwarmEntity(SwarmEntity swarmEntity)
    {
        swarmEntity.node = m_PartitionGrid.Add(swarmEntity, swarmEntity.transform.position);
        swarmEntity.partitionGrid = m_PartitionGrid;
        swarmEntity.swarmSettings = SettingsClone;
        swarmEntity.flowGrid = m_FlowFieldGrid;
        if (swarmEntity.swarmSettings.HasUpdateTick)
        {
            m_SwarmSet.Add(swarmEntity);
        }
    }

    public void RemoveSwarmEntity(SwarmEntity swarmEntity)
    {
        m_PartitionGrid.Remove(swarmEntity.node);
        if (swarmEntity.swarmSettings.HasUpdateTick)
        {
            m_SwarmSet.Remove(swarmEntity);
        }
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            foreach (var entity in m_SwarmSet)
            {
                Vector3 oldPosition = entity.transform.position;
                // MoveEntity;
                entity.UpdateTick();
                m_PartitionGrid.Move(entity.node, oldPosition, entity.transform.position);
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            m_PartitionGrid?.DrawDebugGizmos();
        }

        Gizmos.color = Color.yellow;
        Vector3 size = (Vector3)m_SimulationGridSize * m_SwarmPartitionGridCellSize;
        Vector3 center = size / 2;

        Gizmos.DrawWireCube(center, size);
    }
}
