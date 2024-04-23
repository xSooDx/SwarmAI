using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmFood : MonoBehaviour
{
    public SpacialPartitionGrid<SwarmEntity> partitionGrid;

    Vector3Int[] checkVec = { Vector3Int.zero };

    public float collisionRadiusSq = 4f;
    public int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        partitionGrid = SwarmManager.Instance.m_PartitionGrid;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        partitionGrid.RunOpperationOnCells(checkVec, transform.position, (LinkedList<SwarmEntity> list, int neighbourIndex, Vector3Int cellIndex) =>
        {
            if (list == null) return;
            foreach (var entity in list)
            {
                if (Vector3.SqrMagnitude(entity.transform.position - transform.position) < collisionRadiusSq)
                {
                    //Destroy(entity.gameObject);
                    health--;
                    if (health <= 0)
                    {
                        Destroy(this.gameObject);
                        break;
                    }
                }
            }
        });

    }
}
