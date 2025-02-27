using Unity.Entities;
using UnityEngine;

public struct TurretData : IComponentData
{
    public float FireRate;
    public Entity TurretEntity;
    public Entity ProjectilePrefab;
    public float ProjectileSpeed;
    public float ProjectileDestroyTimer;
}
