using Unity.Entities;
using UnityEngine;

public struct Drone : IComponentData
{
    public Entity DronePrefab;
    public Entity FollowTarget;

}
