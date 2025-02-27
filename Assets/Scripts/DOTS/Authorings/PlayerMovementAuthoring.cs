using UnityEngine;
using Unity.Entities;

public class PlayerMovementAuthoring : MonoBehaviour
{
    public float InvertDirectionDamping;
    public float Drag;
    public float BoostMultiplier;
    public float SpeedLimitRate;
    public float Acceleration;

    public class Baker : Baker<PlayerMovementAuthoring> 
    {
        public override void Bake(PlayerMovementAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerMovementData
            {
                InvertDirectionDamping = authoring.InvertDirectionDamping,
                Drag = authoring.Drag,  
                BoostMultiplier = authoring.BoostMultiplier,
                SpeedLimitRate = authoring.SpeedLimitRate,
                Acceleration = authoring.Acceleration,  
            });


        }
    }
}
