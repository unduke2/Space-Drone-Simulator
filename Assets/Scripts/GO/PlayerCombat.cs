using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _turret;

    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireRate;
    private float _nextFireTime = 0f;

    private bool IsFirePressed;


    private void Update()
    {
        IsFirePressed = GameInput.Instance.IsFirePressed;
        if (IsFirePressed)
        {
            HandleFire();
        }
    }


    private void HandleFire()
    {
        if (InCooldown())
        {
            return;
        }

        GameObject projectile = Instantiate(_projectilePrefab, _turret.transform.position, Quaternion.identity);

        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        if (projectileComponent != null)
        {           
            projectileComponent.Velocity = _turret.transform.forward * _projectileSpeed * Time.deltaTime;
        }

        _nextFireTime = Time.time + (1/_fireRate);
    }


    private bool InCooldown()
    {
        return Time.time < _nextFireTime ? true : false;
    }
}
