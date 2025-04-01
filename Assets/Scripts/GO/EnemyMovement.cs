using System.Timers;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyData _enemyData;

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _yawSpeed = 2f;
    [SerializeField] private float _pitchSpeed = 1f;
    private float _time = 0f;

    private Vector3 randomDirection; 

    private Transform _playerTransform;


    private void Start()
    {
        _enemyData = GetComponent<EnemyData>();

        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }


    private void Update()
    {
        _time += Time.deltaTime;

        float noiseX = Mathf.PerlinNoise(_enemyData.EnemyID * _time, 0f);
        float noiseY = Mathf.PerlinNoise(0f, _enemyData.EnemyID * _time);

        float pitch = Mathf.Sin(noiseY * Mathf.PI * 2) * _pitchSpeed;
        float yaw = Mathf.Cos(noiseX * Mathf.PI * 2) * _yawSpeed;

        randomDirection += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
        randomDirection = randomDirection.normalized; 

        transform.Rotate(pitch * Time.deltaTime, yaw * Time.deltaTime, 0f);

        Vector3 moveDirection = randomDirection * _movementSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World); 
    }
}
