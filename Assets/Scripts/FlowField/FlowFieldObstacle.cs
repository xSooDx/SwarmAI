using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlowFieldObstacle : MonoBehaviour
{
    [Range(byte.MinValue, byte.MaxValue)]
    public byte m_FlowFieldModifier = byte.MaxValue;
}
