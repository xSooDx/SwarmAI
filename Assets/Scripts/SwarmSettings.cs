using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Swarms/Swarm Settings")]
public class SwarmSettings : ScriptableObject
{
    public bool is2D = false;
    public float senseRadius = 3f;
    public float avoidanceRadius = 2f;

    public float cohesionWeight = 1;
    public float avoidanceWeight = 1;
    public float alignmentWeight = 1;
    public float randomWeight = 1;
    public float flowWeight = 1;

    public float minSpeed = 0;
    public float maxSpeed = 5;
    public float startSpeed = 0;

    public float maxSteeringForce;

    public bool HasUpdateTick = true;

    public int numOfEntitiesToConsider = 16;

    public float SenseRadiusSq { get; private set; }
    public float AvoidanceRadiusSq { get; private set; }

    public SwarmSettings Clone()
    {
        SwarmSettings newSettings = CreateInstance<SwarmSettings>();
        newSettings.is2D = this.is2D;
        newSettings.senseRadius = this.senseRadius;
        newSettings.avoidanceRadius = this.avoidanceRadius;
        newSettings.cohesionWeight = this.cohesionWeight;
        newSettings.avoidanceWeight = this.avoidanceWeight;
        newSettings.alignmentWeight = this.alignmentWeight;
        newSettings.randomWeight = this.randomWeight;
        newSettings.flowWeight = this.flowWeight;
        newSettings.minSpeed = this.minSpeed;
        newSettings.maxSpeed = this.maxSpeed;
        newSettings.maxSteeringForce = this.maxSteeringForce;
        newSettings.HasUpdateTick = this.HasUpdateTick;
        newSettings.numOfEntitiesToConsider = this.numOfEntitiesToConsider;
        newSettings.SenseRadiusSq = this.SenseRadiusSq;
        newSettings.AvoidanceRadiusSq = this.AvoidanceRadiusSq;

        return newSettings;
    }

    public void OnValuesChanged()
    {
        SenseRadiusSq = senseRadius * senseRadius;
        AvoidanceRadiusSq = avoidanceRadius * avoidanceRadius;
    }

    private void OnValidate()
    {
        OnValuesChanged();
    }
}
