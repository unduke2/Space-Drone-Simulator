using UnityEngine;
using Unity.Entities;

public class ProjectileAuthoring : MonoBehaviour
{

    public float DestroyTimer;
    public float ProjectileSpeed;
    public class Baker : Baker<ProjectileAuthoring> 
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent<ProjectileData>(entity);
        }
    }
}
