using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public Transform PlayerTransform;

    public int AmountForVictory = 10;

    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private float _respawnTime;
    [SerializeField] private float _bounds;

    [SerializeField] private int EnemyCount;

    private float _respawnTimer;

    private GameObject[] _enemies;

    private List<int> _destroyedEnemiesID;

    private int _destroyedEnemiesCount = 0;


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
        _enemies = new GameObject[EnemyCount];
        _destroyedEnemiesID = new List<int>();
        for (int i = 0; i < _enemies.Length; i++)
        {

            SpawnEnemy(i, out _enemies[i]);
        }
    }


    private void SpawnEnemy(int id, out GameObject enemy)
    {
        GameObject enemyGO = Instantiate(_enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        var enemyMovement = enemyGO.GetComponent<EnemyMovement>();
        var enemyData = enemyGO.GetComponent<EnemyData>();
        enemyData.EnemyID = id;
        enemyMovement.SetPlayerTransform(PlayerTransform);
        enemy = enemyGO;
        Debug.Log("Spawned enemy!");

    }


    private void Update()
    {
        _respawnTimer -= Time.deltaTime;
        if (_respawnTimer <= 0 && _destroyedEnemiesID.Count > 0)
        {
            RespawnEnemies();
            _respawnTimer = _respawnTime;
        }

        if (_destroyedEnemiesCount >= AmountForVictory)
        {
            Debug.Log("You won!");
        }
    }


    private void RespawnEnemies()
    {
        Debug.Log($"Respawning {_destroyedEnemiesID.Count} enemies ");
        foreach (var id in _destroyedEnemiesID)
        {
            SpawnEnemy(id, out GameObject enemy);
            _enemies[id] = enemy;
        }
        _destroyedEnemiesID.Clear();
    }


    public void RegisterDestroyedEnemy(int enemyID)
    {
        Debug.Log($"Registering destroyed enemy with ID: {enemyID}");
        _destroyedEnemiesID.Add(enemyID);
        _destroyedEnemiesCount++;
    }


    private Vector3 GetRandomSpawnPosition()
    {
        float range = _bounds;
        return new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
    }
}