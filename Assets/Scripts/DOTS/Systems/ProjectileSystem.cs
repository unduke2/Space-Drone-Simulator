using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

        [ReadOnly] public NativeArray<LocalTransform> DroneTransforms;
        [ReadOnly] public NativeArray<Entity> DroneEntities;

        public EntityCommandBuffer.ParallelWriter ECB;

        public float DeltaTime;

        void Execute(Entity entity, [EntityIndexInQuery] int entityInQueryIndex,
                     ref ProjectileData projectileData,
                     ref LocalTransform transform)
        {
            transform.Position += projectileData.Velocity * DeltaTime;

            projectileData.DestroyTimer -= DeltaTime;

            if (projectileData.DestroyTimer <= 0)
            {
                ECB.DestroyEntity(entityInQueryIndex, entity);
                return;
            }

            for (int i = 0; i < DroneTransforms.Length; i++)
            {
                float dist = math.distance(transform.Position, DroneTransforms[i].Position);
                if (dist < 25f)
                {
                    ECB.DestroyEntity(entityInQueryIndex, entity);
                    ECB.DestroyEntity(entityInQueryIndex, DroneEntities[i]);
                    return;
                }
            }
        }
    }
}

