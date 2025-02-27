using Unity.Entities;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public float MaxSpeed;
    public float YawSensitivity;
    public float PitchSensitivity;
    public float RollSensitivity;
    public class Baker : Baker<MovementAuthoring> 
    {
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new MovementData
            {
                MaxSpeed = authoring.MaxSpeed,
                YawSensitivity = authoring.YawSensitivity,
                PitchSensitivity = authoring.PitchSensitivity,
                RollSenstivity = authoring.RollSensitivity,
            });
        }
    }
   
}
