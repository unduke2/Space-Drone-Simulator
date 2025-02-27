using UnityEngine;
using Unity.Entities;

public class DroneAuthoring : MonoBehaviour
{
    public GameObject DronePrefab;
    public GameObject FollowTarget;
    

    public class Baker : Baker<DroneAuthoring> 
    {
        public override void Bake(DroneAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Drone
            {
                DronePrefab = GetEntity(authoring.DronePrefab, TransformUsageFlags.Dynamic),
                FollowTarget = GetEntity(authoring.FollowTarget, TransformUsageFlags.Dynamic)
                

            });


            //AddComponent(entity, new TurretData
            //{
            //    TurretEntity = GetEntity(authoring.Turret, TransformUsageFlags.Dynamic),
                
          
            //});
            
        }
    }
}
