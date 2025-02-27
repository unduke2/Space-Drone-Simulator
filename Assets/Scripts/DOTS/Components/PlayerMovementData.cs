using Unity.Entities;
using UnityEngine;

public struct PlayerMovementData : IComponentData, IEnableableComponent
{
    public float InvertDirectionDamping;
    public float Drag;
    public float BoostMultiplier;
    public float SpeedLimitRate;
    public float Acceleration;

}
