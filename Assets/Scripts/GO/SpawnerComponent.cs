using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] private int EnemyCount;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float _bounds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < EnemyCount; i++)
        {

            Instantiate(EnemyPrefab, new Vector3(Random.Range(-_bounds, _bounds), Random.Range(-_bounds, _bounds), Random.Range(-_bounds, _bounds)), Quaternion.identity);
        }
    }
}
