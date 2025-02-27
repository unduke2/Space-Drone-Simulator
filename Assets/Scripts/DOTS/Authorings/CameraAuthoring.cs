using UnityEngine;
using Unity.Entities;
public class CameraAuthoring : MonoBehaviour
{
    public float MaxFOV;
    public float MinFOV;
    public float FollowStep;
    public Vector3 FollowOffset;
    public GameObject FollowTarget;

    public class Baker : Baker<CameraAuthoring>
    {
        public override void Bake(CameraAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new CameraData
            {
                FollowStep = authoring.FollowStep,
                MinFOV = authoring.MinFOV,
                MaxFOV = authoring.MaxFOV,
                FollowOffset = authoring.FollowOffset,
                FollowTarget = GetEntity(authoring.FollowTarget, TransformUsageFlags.Dynamic),
            });
        }
    }
}
