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

        // Query for all enemies that have EnemyMovementData + LocalTransform
        foreach (var (movementData, transform, entity) in
                 SystemAPI.Query<RefRW<MovementData>, RefRW<LocalTransform>>().WithNone<PlayerTag>().WithEntityAccess())
        {
            // Update the time offset
  
            // Sample noise based on randomSeed + timeOffset
            float time = elapsedTime;
            float seed = entity.Index;

            // Basic "Perlin" approach (or use Unity.Mathematics.noise if you want)
            float noiseX = UnityEngine.Mathf.PerlinNoise(seed + time, 0f);
            float noiseY = UnityEngine.Mathf.PerlinNoise(0f, seed + time);

            // Calculate pitch/yaw/roll
            float pitch = math.sin(noiseY * math.PI * 2f) * movementData.ValueRO.PitchSensitivity;
            float yaw = math.cos(noiseX * math.PI * 2f) * movementData.ValueRO.YawSensitivity;
            float roll = math.sin(noiseX * math.PI * 2f) * movementData.ValueRO.RollSenstivity; // example

            // Rotate the enemy
            // If using a LocalTransform, it has a Rotation (quaternion).
            quaternion currentRot = transform.ValueRO.Rotation;

            // Construct a small rotation for each axis in degrees or radians
            quaternion deltaRotation = quaternion.Euler(math.radians(pitch * deltaTime),
                                                        math.radians(yaw * deltaTime),
                                                        math.radians(roll * deltaTime));

            // Combine the old rotation with the new incremental rotation
            quaternion newRot = math.mul(currentRot, deltaRotation);

            // Optionally, randomize direction or store it
            // For a truly random direction, keep it in movementData, or do mild changes:
            float3 dir = movementData.ValueRO.Velocity;
            dir += new float3(
                UnityEngine.Random.Range(-0.05f, 0.05f),
                UnityEngine.Random.Range(-0.05f, 0.05f),
                UnityEngine.Random.Range(-0.05f, 0.05f)
            );
            dir = math.normalize(dir);
            movementData.ValueRW.Velocity = dir;

            // Move forward (or move in `dir`) by movementSpeed
            float3 newPos = transform.ValueRO.Position + dir * movementData.ValueRO.MaxSpeed/2 * deltaTime;

            // Write back to the transform
            transform.ValueRW = new LocalTransform
            {
                Position = newPos,
                Rotation = newRot,
                Scale = transform.ValueRO.Scale
            };
        }
    }
}
