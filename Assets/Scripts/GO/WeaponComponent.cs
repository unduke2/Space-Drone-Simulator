using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponComponent : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private float _projectileSpeed;
    InputAction fireAction;

    private bool IsFiring = false;

    private void Start()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    private void Update()
    {
        if (fireAction.IsPressed() && !IsFiring)
        {
            IsFiring = true;
            HandleFire();
        }

        if (!fireAction.IsPressed())
        {
            IsFiring = false;
        }
    }
    private void HandleFire()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _cannon.transform.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();;
        if (projectileComponent != null)
        {           
            projectileComponent.Velocity = _cannon.transform.forward * _projectileSpeed * Time.deltaTime;
        }


    }
}
