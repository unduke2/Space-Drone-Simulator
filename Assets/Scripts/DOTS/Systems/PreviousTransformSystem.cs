using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(PlayerMovementSystem))]
public partial struct PreviousTransformSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PreviousTransform>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, previous, entity) in
                 SystemAPI.Query<RefRO<LocalToWorld>, RefRW<PreviousTransform>>().WithEntityAccess())
        {
            previous.ValueRW.Position = transform.ValueRO.Position;
            previous.ValueRW.Rotation = transform.ValueRO.Rotation;
        }
    }
}
