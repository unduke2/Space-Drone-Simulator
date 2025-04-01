using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PreviousTransform : IComponentData
{
    public float3 Position;
    public quaternion Rotation;
}
