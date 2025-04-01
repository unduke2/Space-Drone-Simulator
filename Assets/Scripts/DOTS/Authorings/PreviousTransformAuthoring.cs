using Unity.Entities;
using UnityEngine;

public class PreviousTransformAuthoring : MonoBehaviour
{
    public class Baker : Baker<PreviousTransformAuthoring> 
    {
        public override void Bake(PreviousTransformAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new PreviousTransform
            {
                Position = authoring.transform.position,
                Rotation = authoring.transform.rotation
            });

        }
    }
}
