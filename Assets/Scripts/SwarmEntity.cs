using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwarmEntity : MonoBehaviour
{
    public LinkedListNode<SwarmEntity> Node;
    public SpacialPartitionGrid<SwarmEntity> PartitionGrid;
    [HideInInspector] public SwarmSettings swarmSettings;

    Vector3 cohesionVector;
    Vector3 avoidanceVector;
    Vector3 allignmentVector;
    Vector3 obstacleVector;

    Vector3 velocity;

    public virtual void UpdateTick()
    {
        cohesionVector = Vector3.zero;
        avoidanceVector = Vector3.zero;
        allignmentVector = Vector3.zero;
        obstacleVector = Vector3.zero;

        Vector3 swarmCenter = Vector3.zero;
        Vector3 position = transform.position;

        int count = 0;
        int obstacleCount = 0;
        bool foundEnough = false;

        PartitionGrid.RunOpperationOnCells(swarmSettings.is2D? Grid3DUtilities.SwarmNeighbourIndexInOrder2D : Grid3DUtilities.SwarmNeighbourIndexInOrder, position, (list, neighbourIndex, cellIndex) =>
        {
            if (list != null)
            {
                if (!foundEnough)
                {
                    foreach (SwarmEntity entity in list)
                    {
                        if (entity.Node == this.Node) continue;
                        Transform entityTransform = entity.transform;
                        Vector3 entityPosition = entityTransform.position;
                        Vector3 offset = position - entityPosition;
                        float sqDist = Vector3.SqrMagnitude(offset);
                        if (sqDist < swarmSettings.SenseRadiusSq)
                        {
                            swarmCenter += entityPosition;
                            allignmentVector += entityTransform.forward;

                            if (sqDist < swarmSettings.AvoidanceRadiusSq)
                            {
                                avoidanceVector += offset / (1 + sqDist);
                            }


                            count++;
                        }
                        if (count >= swarmSettings.numOfEntitiesToConsider)
                        {
                            foundEnough = true;
                        }
                    }
                }
            }
            else
            {
                obstacleCount++;
                Vector3 obstaclePosition = PartitionGrid.GetPositionOfCell(cellIndex);
                Vector3 offset = position - obstaclePosition;
                float sqDist = Vector3.SqrMagnitude(offset);
                obstacleVector += offset / (.001f + sqDist);
            }
        });

        Vector3 acceleration = Vector3.zero;

        if (count > 0)
        {
            swarmCenter /= count;
            cohesionVector = swarmCenter - position;

            cohesionVector = Steer(cohesionVector) * swarmSettings.cohesionWeight;
            avoidanceVector = Steer(avoidanceVector) * swarmSettings.avoidanceWeight;
            allignmentVector = Steer(allignmentVector) * swarmSettings.alignmentWeight;
            acceleration += allignmentVector + avoidanceVector + cohesionVector;



            //Debug.DrawRay(position, cohesionVector, Color.green);
            //Debug.DrawRay(position, avoidanceVector, Color.red);
            //Debug.DrawRay(position, allignmentVector, Color.blue);

        }
        else
        {
            acceleration += Steer(Random.insideUnitSphere) * swarmSettings.randomWeight;
        }

        if (obstacleCount > 0)
        {
            obstacleVector = obstacleVector / obstacleCount;
            obstacleVector = Steer(obstacleVector) * 25;
            acceleration += obstacleVector;
            //Debug.DrawRay(position, obstacleVector, Color.magenta);
        }

        if(swarmSettings.is2D)
        {
            acceleration.y = 0;
        }
        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity.normalized;
        speed = Mathf.Clamp(speed, swarmSettings.minSpeed, swarmSettings.maxSpeed);
        velocity = dir * speed;

        transform.position = position + velocity * Time.deltaTime;
        transform.forward = dir;

    }

    Vector3 Steer(Vector3 vector)
    {
        Vector3 v = vector.normalized * swarmSettings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, swarmSettings.maxSteeringForce);
    }

    protected virtual void Start()
    {
        SwarmManager.Instance.AddSwarmEntity(this);
        velocity = swarmSettings.startSpeed * transform.forward;
    }

    protected virtual void OnDestroy()
    {
        SwarmManager.Instance.RemoveSwarmEntity(this);
    }
}
