using UnityEngine;
using Unity.Entities;

public class SpawnerAuthoring : MonoBehaviour
{
    public int Count;
    public int Bound;
    public GameObject DronePrefab;
    public GameObject ProjectilePrefab;

    class Baker : Baker<SpawnerAuthoring> 
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new SpawnerConfig
            {
                Count = authoring.Count,
                Bound = authoring.Bound,
                DronePrefab = GetEntity(authoring.DronePrefab, TransformUsageFlags.Dynamic),
                ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct SpawnerConfig : IComponentData
{
    public int Count;
    public int Bound;
    public Entity DronePrefab;
    public Entity ProjectilePrefab; 
}
