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

        Random rand = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));

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
            state.EntityManager.SetComponentData(droneEntity, new LocalTransform
            {
                Position = new float3
                {
                    x = rand.NextFloat(-config.Bound, config.Bound),
                    y = rand.NextFloat(100, config.Bound),
                    z = rand.NextFloat(-config.Bound, config.Bound),
                },
                Scale = 1f,
                Rotation = quaternion.identity
            });

        }
    }
}
