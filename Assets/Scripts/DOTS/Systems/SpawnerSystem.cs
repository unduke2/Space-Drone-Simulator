using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<SpawnerConfig>();
        var rand = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));

        float minY = 100f;

        for (int i = 0; i < config.Count; i++)
        {
            var droneEntity = state.EntityManager.Instantiate(config.DronePrefab);

            if (i == 0)
            {
                state.EntityManager.AddComponent<PlayerTag>(droneEntity);
                state.EntityManager.SetComponentEnabled<PlayerMovementData>(droneEntity, true);
                state.EntityManager.SetComponentEnabled<CameraData>(droneEntity, true);
            }
            else
            {
                state.EntityManager.SetComponentEnabled<PlayerMovementData>(droneEntity, false);
                state.EntityManager.SetComponentEnabled<CameraData>(droneEntity, false);
                state.EntityManager.AddComponent<EnemyTag>(droneEntity);
            }

            float3 position = new float3
            {
                x = rand.NextFloat(-config.Bound, config.Bound),
                y = rand.NextFloat(minY, config.Bound),
                z = rand.NextFloat(-config.Bound, config.Bound)
            };

            state.EntityManager.SetComponentData(droneEntity, new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 1f,
            });
        }
    }
}
