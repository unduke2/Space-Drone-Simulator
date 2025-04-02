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
            EnemyData data = other.gameObject.GetComponentInParent<EnemyData>();
            if (data != null)
            {
                EnemyManager.Instance.RegisterDestroyedEnemy(data.EnemyID);
            } else
            {
                Debug.Log("EnemyData is null");
            }

            Destroy(other.gameObject);  
            Destroy(gameObject);     

            Debug.Log("Collided with enemy");
        }
        else if (layerName == "Level")
        {
            Destroy(gameObject);  
            Debug.Log("Collided with level");
        }
    }
}
