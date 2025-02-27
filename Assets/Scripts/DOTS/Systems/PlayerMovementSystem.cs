using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMovementSystem : ISystem
{
    private float _speedMultiplier;
    private float2 _moveInput;
    private float2 _lookInput;

 public void OnUpdate(ref SystemState state)
    {
        _moveInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _lookInput = new float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));



        foreach (var (transform, movementData, playerMovementData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<MovementData>, RefRO<PlayerMovementData>>().WithPresent<PlayerMovementData>().WithAll<PlayerTag>()) 
        {
            var forward = transform.ValueRO.Forward();

            _speedMultiplier = Input.GetButton("Jump") ? playerMovementData.ValueRO.BoostMultiplier : 1f;


            var dot = math.dot(forward, movementData.ValueRO.Velocity);
            if (dot < -0.1)
            {
                movementData.ValueRW.Velocity *= 1f - (playerMovementData.ValueRO.InvertDirectionDamping * SystemAPI.Time.DeltaTime);
            }

            movementData.ValueRW.Velocity += forward * _moveInput.y * (playerMovementData.ValueRO.Acceleration * _speedMultiplier) * SystemAPI.Time.DeltaTime;
         




            if (math.length(movementData.ValueRW.Velocity) > movementData.ValueRO.MaxSpeed * _speedMultiplier)
            {
                movementData.ValueRW.Velocity = math.lerp(movementData.ValueRO.Velocity, math.normalize(movementData.ValueRO.Velocity) * (movementData.ValueRO.MaxSpeed * _speedMultiplier), SystemAPI.Time.DeltaTime * playerMovementData.ValueRO.SpeedLimitRate);
            }

            if (math.all(_moveInput == float2.zero) && (math.length(movementData.ValueRO.Velocity) > 0))
            {
                movementData.ValueRW.Velocity = math.lerp(movementData.ValueRO.Velocity, float3.zero, SystemAPI.Time.DeltaTime * playerMovementData.ValueRO.Drag);
            }

            if (math.length(movementData.ValueRO.Velocity) < 0.01f) 
            {
                movementData.ValueRW.Velocity = Vector3.zero;
            }



            var currentPitch = -_lookInput.y * movementData.ValueRO.PitchSensitivity * SystemAPI.Time.DeltaTime;
            var currentYaw = _lookInput.x * movementData.ValueRO.YawSensitivity * SystemAPI.Time.DeltaTime;
            var currentRoll = -_moveInput.x * movementData.ValueRO.RollSenstivity * SystemAPI.Time.DeltaTime;

            var eulerRotation = new float3(currentPitch, currentYaw, currentRoll);
            var quaternionRotation = quaternion.Euler(eulerRotation);

            transform.ValueRW.Rotation = math.mul(transform.ValueRO.Rotation, quaternionRotation);

            transform.ValueRW.Position += movementData.ValueRO.Velocity * SystemAPI.Time.DeltaTime;





        }
    }
}
