using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MovementData : IComponentData
{
    public float3 Velocity;
    public float MaxSpeed;
    public float YawSensitivity;
    public float PitchSensitivity;
    public float RollSenstivity;
}
