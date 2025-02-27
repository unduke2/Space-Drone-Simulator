using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var droneQuery = SystemAPI.QueryBuilder()
       .WithAll<EnemyTag, LocalTransform>()
       .Build();


        var droneTransforms = droneQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var droneEntities = droneQuery.ToEntityArray(state.WorldUpdateAllocator);

        var job = new ProjectileCollisionJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecb,
            DroneTransforms = droneTransforms,
            DroneEntities = droneEntities,
        };
        job.ScheduleParallel();

        
        
    }
    [BurstCompile]
    public partial struct ProjectileCollisionJob : IJobEntity
    {
        // Read-only arrays of drones
        [ReadOnly]
        public NativeArray<LocalTransform> DroneTransforms;
        [ReadOnly]
        public NativeArray<Entity> DroneEntities;

        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;

        // Each Execute() is called once per projectile
        void Execute(Entity entity, [EntityIndexInQuery] int entityInQueryIndex,
                     ref ProjectileData projectileData,
                     ref LocalTransform transform)
        {
            // 1) Update projectile movement
            transform.Position += projectileData.Velocity * DeltaTime;

            // 2) Decrement lifetime
            projectileData.DestroyTimer -= DeltaTime;
            if (projectileData.DestroyTimer <= 0)
            {
                ECB.DestroyEntity(entityInQueryIndex, entity);
                return;
            }

            // 3) Collision check against all drones (naïve approach)
            for (int i = 0; i < DroneTransforms.Length; i++)
            {
                float3 dronePos = DroneTransforms[i].Position;
                float dist = math.distance(transform.Position, dronePos);

                // Example collision radius
                if (dist < 25f)
                {
                    // Destroy projectile
                    ECB.DestroyEntity(entityInQueryIndex, entity);

                    // Destroy this drone
                    ECB.DestroyEntity(entityInQueryIndex, DroneEntities[i]);

                    // No need to check other drones
                    return;
                }
            }
        }
    }
}

