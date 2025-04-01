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
        float deltaTime = SystemAPI.Time.DeltaTime;

        _moveInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _lookInput = new float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        foreach (var (transform, movementData, playerMovementData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<MovementData>, RefRO<PlayerMovementData>>().WithPresent<PlayerMovementData>().WithAll<PlayerTag>())
        {
            var forward = transform.ValueRO.Forward();
            bool isBoosting = Input.GetButton("Jump");

            _speedMultiplier = isBoosting ? playerMovementData.ValueRO.BoostMultiplier : 1f;

            float forwardDot = math.dot(forward, movementData.ValueRO.Velocity);

            if (forwardDot < -0.1)
            {
                movementData.ValueRW.Velocity *= 1f - (playerMovementData.ValueRO.InvertDirectionDamping * deltaTime);
            }

            movementData.ValueRW.Velocity += forward * _moveInput.y * (playerMovementData.ValueRO.Acceleration * _speedMultiplier * deltaTime);

            float currentSpeed = math.length(movementData.ValueRO.Velocity);
            float maxSpeed = movementData.ValueRO.MaxSpeed * _speedMultiplier;

            if (currentSpeed > maxSpeed)
            {
                movementData.ValueRW.Velocity = math.lerp(movementData.ValueRO.Velocity, math.normalize(movementData.ValueRO.Velocity) * (movementData.ValueRO.MaxSpeed * _speedMultiplier), SystemAPI.Time.DeltaTime * playerMovementData.ValueRO.SpeedLimitRate);
            }

            if (math.all(_moveInput == float2.zero) && currentSpeed > 0)
            {
                movementData.ValueRW.Velocity = math.lerp(movementData.ValueRO.Velocity, float3.zero, SystemAPI.Time.DeltaTime * playerMovementData.ValueRO.Drag);
            }

            if (math.length(movementData.ValueRO.Velocity) < 0.01f)
            {
                movementData.ValueRW.Velocity = float3.zero;
            }

            float pitch = -_lookInput.y * movementData.ValueRO.PitchSensitivity * deltaTime;
            float yaw = _lookInput.x * movementData.ValueRO.YawSensitivity * deltaTime;
            float roll = -_moveInput.x * movementData.ValueRO.RollSenstivity * deltaTime;

            var rotationDelta = quaternion.Euler(new float3(pitch, yaw, roll));

            transform.ValueRW.Rotation = math.mul(transform.ValueRO.Rotation, rotationDelta);
            transform.ValueRW.Position += movementData.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
        }
    }
}
