using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileData : IComponentData
{
    public float3 Velocity;
    public float DestroyTimer;
}
