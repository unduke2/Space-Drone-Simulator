using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct CameraSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float alpha = math.clamp(deltaTime / Time.fixedDeltaTime, 0f, 1f);

        foreach (var (movementData, cameraData) in
                 SystemAPI.Query<RefRO<MovementData>, RefRO<CameraData>>()
                 .WithAll<PlayerTag>())
        {
            Entity followTarget = cameraData.ValueRO.FollowTarget;

            if (!state.EntityManager.HasComponent<PreviousTransform>(followTarget))
                continue;

            var current = state.EntityManager.GetComponentData<LocalToWorld>(followTarget);
            var previous = state.EntityManager.GetComponentData<PreviousTransform>(followTarget);

            float3 interpolatedPosition = math.lerp(previous.Position, current.Position, alpha);
            quaternion interpolatedRotation = math.slerp(previous.Rotation, current.Rotation, alpha);

            float3 finalPosition = interpolatedPosition;

            Camera.main.transform.position = finalPosition;
            Camera.main.transform.rotation = interpolatedRotation;

            float velocityMagnitude = math.length(movementData.ValueRO.Velocity);
            float t = math.unlerp(0f, 300f, velocityMagnitude);
            float smooth = math.smoothstep(1f, 0f, t);

            Camera.main.fieldOfView = math.lerp(cameraData.ValueRO.MinFOV, cameraData.ValueRO.MaxFOV, smooth);
        }
    }
}
