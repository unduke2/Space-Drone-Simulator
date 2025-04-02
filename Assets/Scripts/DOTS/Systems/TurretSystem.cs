using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct TurretSystem : ISystem
{
    private float _nextFireTime;
    private float _elapsedTime;

    public void OnCreate(ref SystemState state)
    {
        _nextFireTime = 0f;
        _elapsedTime = 0f;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<SpawnerConfig>(out var spawnerConfig))
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var ecb = ecbSingleton.CreateCommandBuffer(state.World.Unmanaged);

        foreach (var turretData in SystemAPI.Query<RefRO<TurretData>>().WithAll<PlayerTag>())
        {
            var turretEntity = turretData.ValueRO.TurretEntity;
            var projectileTransform = state.EntityManager.GetComponentData<LocalTransform>(spawnerConfig.ProjectilePrefab);

            if (!Input.GetMouseButton(0))
            {
                continue;
            }

            if (_elapsedTime < _nextFireTime)
            {
                continue;
            }

            var projectile = state.EntityManager.Instantiate(spawnerConfig.ProjectilePrefab);
            if (projectile == Entity.Null)
            {
                continue;
            }

            var turretTransform = state.EntityManager.GetComponentData<LocalToWorld>(turretEntity);
            projectileTransform.Position = turretTransform.Position;

            state.EntityManager.SetComponentData(projectile, projectileTransform);

            state.EntityManager.SetComponentData(projectile, new ProjectileData
            {
                Velocity = turretData.ValueRO.ProjectileSpeed * math.normalize(turretTransform.Forward),
                DestroyTimer = turretData.ValueRO.ProjectileDestroyTimer,

            });

            ecb.AddComponent<FireAudioTag>(turretEntity); 



            _nextFireTime = _elapsedTime + (1 / turretData.ValueRO.FireRate);
        }
    }


}
