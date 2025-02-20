using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private int _destroyedEnemies = 0;

    private int _enemiesToRespawn = 0;

    public int AmountForVictory = 10;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _respawnTime;
    private float _respawnTimer;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _respawnTimer = _respawnTime;
    }
    private void Update()
    {
        _respawnTimer -= Time.deltaTime;
        if (_respawnTimer <= 0 && _destroyedEnemies > 0)
        {
            RespawnEnemies();
            _respawnTimer = _respawnTime;
        }

        if (_destroyedEnemies >= AmountForVictory)
        {
            Debug.Log("You won!");
        }
    }

    private void RespawnEnemies()
    {
        for (int i = 0; i < _enemiesToRespawn; i++)
        {
            Instantiate(_enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            Debug.Log("Spawned new enemy!");
        }
        _enemiesToRespawn = 0;
    }
    public void RegisterDestroyedEnemy()
    {
        _destroyedEnemies++;
        _enemiesToRespawn++;

    }
    private Vector3 GetRandomSpawnPosition()
    {
        // Just for example, you can spawn at a random position within a certain range
        float range = 10f;
        return new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
    }
}