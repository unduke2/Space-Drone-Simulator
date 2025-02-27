using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Rendering;

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
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var spawnerConfig = SystemAPI.GetSingleton<SpawnerConfig>();    
        _elapsedTime += Time.deltaTime;

        // Loop over entities that have both the PlayerTag and TurretData components
        foreach (var turretData in SystemAPI.Query<RefRO<TurretData>>().WithAll<PlayerTag>())
        {
            var turretEntity = turretData.ValueRO.TurretEntity;
            //var turretTransform = state.EntityManager.GetComponentData<LocalTransform>(turretEntity);
            var ecb = ecbSingleton.CreateCommandBuffer(state.World.Unmanaged);

            var projectileTransform = state.EntityManager.GetComponentData<LocalTransform>(spawnerConfig.ProjectilePrefab);

            if (Input.GetMouseButton(0))
            {
                // Fire rate control
                if (_elapsedTime < _nextFireTime)
                {
                    return;  // Skip the firing logic until the next allowed time
                }
                // Instantiate the projectile
                var projectile = state.EntityManager.Instantiate(spawnerConfig.ProjectilePrefab);

                if (projectile != Entity.Null)  // Check if the projectile was successfully instantiated
                {
                    Debug.Log("instantiated projectile");
                    // Get the turret's transform
                    var turretTransform = state.EntityManager.GetComponentData<LocalToWorld>(turretEntity);
                    projectileTransform.Position = turretTransform.Position;



                    state.EntityManager.SetComponentData(projectile, projectileTransform);

                    // Set the initial velocity for the projectile
                    state.EntityManager.SetComponentData(projectile, new ProjectileData
                    {
                        Velocity = turretData.ValueRO.ProjectileSpeed * math.normalize(turretTransform.Forward),
                        DestroyTimer = turretData.ValueRO.ProjectileDestroyTimer,
                        
                    });




                } else { Debug.Log("Projectile is null"); }
                // Update the time for the next allowed shot
                _nextFireTime = _elapsedTime + (1 / turretData.ValueRO.FireRate);
            }
        }

    
    }
}
