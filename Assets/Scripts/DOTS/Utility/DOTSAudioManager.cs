using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class DOTSAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource turretSource;
    public AudioClip fireClip;

    public AudioSource flyingSource;
    public AudioClip flyingClip;

    [Header("Flying Sound Settings")]
    public float minPitch = 0.8f;
    public float maxPitch = 1.4f;
    public float maxExpectedSpeed = 300f;

    private EntityManager _entityManager;
    private EntityQuery _fireQuery;
    private EntityQuery _playerQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        _fireQuery = _entityManager.CreateEntityQuery(typeof(FireAudioTag));
        _playerQuery = _entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<MovementData>(),
            ComponentType.ReadOnly<PlayerTag>()
        );

        if (flyingSource != null)
        {
            flyingSource.clip = flyingClip;
            flyingSource.loop = true;
            flyingSource.Play();
        }
    }

    void Update()
    {
        PlayFireSound();
        UpdateFlyingSound();
    }

    private void PlayFireSound()
    {
        using var entities = _fireQuery.ToEntityArray(Unity.Collections.Allocator.Temp);

        foreach (var entity in entities)
        {
            if (turretSource != null && fireClip != null)
                turretSource.PlayOneShot(fireClip);

            _entityManager.RemoveComponent<FireAudioTag>(entity);
        }
    }

    private void UpdateFlyingSound()
    {
        if (flyingSource == null || !_playerQuery.TryGetSingleton(out MovementData movement)) return;

        float speed = math.length(movement.Velocity);
        float normalized = math.clamp(speed / maxExpectedSpeed, 0f, 1f);

        flyingSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalized);
        flyingSource.volume = normalized;
    }
}
