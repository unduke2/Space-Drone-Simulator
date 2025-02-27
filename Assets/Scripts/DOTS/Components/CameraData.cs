using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct CameraData : IComponentData, IEnableableComponent
{
    public Entity FollowTarget;
    public float MinFOV;
    public float MaxFOV;
    public float FollowStep;
    public float3 FollowOffset;
}
