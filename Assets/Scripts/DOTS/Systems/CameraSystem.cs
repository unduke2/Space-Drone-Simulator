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
        foreach (var (movementData, cameraData) in SystemAPI.Query<RefRO<MovementData>, RefRO<CameraData>>().WithPresent<CameraData>().WithAll<PlayerTag>())
        {
            var followTarget = state.EntityManager.GetComponentData<LocalToWorld>(cameraData.ValueRO.FollowTarget);
            var deltaTime = SystemAPI.Time.DeltaTime;
            // Smoothly lerp the camera to match the player
            Vector3 oldPos = Camera.main.transform.position;
            Quaternion oldRot = Camera.main.transform.rotation;

            Vector3 newPos = math.lerp(
                oldPos,
                followTarget.Position,
                deltaTime * cameraData.ValueRO.FollowStep
            );

            Quaternion newRot = math.slerp(
                oldRot,
                followTarget.Rotation,
                deltaTime * cameraData.ValueRO.FollowStep
            );

            Camera.main.transform.position = math.lerp(Camera.main.transform.position, newPos, 1f);
            Camera.main.transform.rotation = math.slerp(Camera.main.transform.rotation, newRot, 1f); 

            float velocityMagnitude = math.length(movementData.ValueRO.Velocity);

            float t = math.unlerp(0f, 300f, velocityMagnitude);

            float smoothT = math.smoothstep(0, 1, 1/t);

            Camera.main.fieldOfView = math.lerp(cameraData.ValueRO.MinFOV, cameraData.ValueRO.MaxFOV, smoothT);
        }
    }

    
}
