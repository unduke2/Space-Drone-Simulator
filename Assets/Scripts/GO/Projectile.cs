using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 Velocity;
    [SerializeField] private float _destroyTimer;

    private void Update()
    {
        _destroyTimer -= Time.deltaTime;
        if (_destroyTimer < 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Velocity, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Enemy")
        {
            EnemyManager.Instance.RegisterDestroyedEnemy();
            Destroy(other.gameObject);  // Destroy the enemy
            Destroy(gameObject);        // Destroy the projectile

            Debug.Log("Collided with enemy");
        }
        else if (layerName == "Level")
        {
            Destroy(gameObject);        // Destroy the projectile if hitting level
            Debug.Log("Collided with level");
        }
    }
}
