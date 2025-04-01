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
        foreach (var (movementData, cameraData) in SystemAPI.Query<RefRO<MovementData>, RefRO<CameraData>>().WithPresent<CameraData>().WithAll<PlayerTag>())
        {

            var followTransform = state.EntityManager.GetComponentData<LocalToWorld>(cameraData.ValueRO.FollowTarget);

            Camera.main.transform.position = followTransform.Position;
            Camera.main.transform.rotation = followTransform.Rotation;

            float velocityMagnitude = math.length(movementData.ValueRO.Velocity);
            float ratio = math.unlerp(300f, 0f, velocityMagnitude);
            float smoothed = math.smoothstep(0f, 1f, ratio);

            Camera.main.fieldOfView = math.lerp(cameraData.ValueRO.MinFOV,
                cameraData.ValueRO.MaxFOV,
                smoothed
                );
        }
    }
}
