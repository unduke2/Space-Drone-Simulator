using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMovementSystem : ISystem
{
    private float elapsedTime;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        elapsedTime += deltaTime;

        foreach (var (movementData, transform, entity) in
                 SystemAPI.Query<RefRW<MovementData>, RefRW<LocalTransform>>().WithNone<PlayerTag>().WithEntityAccess())
        {
            float time = elapsedTime;
            float seed = entity.Index;

            float noiseX = UnityEngine.Mathf.PerlinNoise(seed + time, 0f);
            float noiseY = UnityEngine.Mathf.PerlinNoise(0f, seed + time);

            float pitch = math.sin(noiseY * math.PI * 2f) * movementData.ValueRO.PitchSensitivity;
            float yaw = math.cos(noiseX * math.PI * 2f) * movementData.ValueRO.YawSensitivity;
            float roll = math.sin(noiseX * math.PI * 2f) * movementData.ValueRO.RollSenstivity; // example

            quaternion currentRot = transform.ValueRO.Rotation;
            quaternion deltaRotation = quaternion.Euler(math.radians(pitch * deltaTime),
                                                        math.radians(yaw * deltaTime),
                                                        math.radians(roll * deltaTime));


            quaternion newRot = math.mul(currentRot, deltaRotation);

            float3 direction = movementData.ValueRO.Velocity;
            direction += new float3(
                UnityEngine.Random.Range(-0.05f, 0.05f),
                UnityEngine.Random.Range(-0.05f, 0.05f),
                UnityEngine.Random.Range(-0.05f, 0.05f)
            );
            direction = math.normalize(direction);
            movementData.ValueRW.Velocity = direction;

            float3 newPos = transform.ValueRO.Position + direction * (movementData.ValueRO.MaxSpeed * 0.5f) * deltaTime;

            transform.ValueRW = new LocalTransform
            {
                Position = newPos,
                Rotation = newRot,
                Scale = transform.ValueRO.Scale
            };
        }
    }
}
