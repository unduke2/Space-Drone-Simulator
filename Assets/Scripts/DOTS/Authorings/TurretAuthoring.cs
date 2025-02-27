using UnityEngine;
using Unity.Entities;
public class TurretAuthoring : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float DestroyTimer;
    public GameObject Turret;
    public float FireRate;
    public float ProjectileSpeed;


    public class Baker : Baker<TurretAuthoring> 
    {
        public override void Bake(TurretAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new TurretData
            {
                TurretEntity = GetEntity(authoring.Turret, TransformUsageFlags.Dynamic),
                FireRate = authoring.FireRate,
                ProjectileSpeed = authoring.ProjectileSpeed,
                ProjectileDestroyTimer = authoring.DestroyTimer,
            });


        }
    }

}
