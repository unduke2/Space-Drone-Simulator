using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    InputAction moveAction;
    InputAction boostAction;
    InputAction lookAction;


    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _currentBoost = 1f;
    [SerializeField] private float _boostMultiplier = 2f;
    [SerializeField] private float _rollSpeed = 1f;
    [SerializeField] private float _mouseSensitivity = 1f;
    [SerializeField] private float _inertiaFactor = 0.5f;

    private float _inertiaTimer = 0.1f;
    private float _boostTimer = 3f;

    private Vector3 _velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        boostAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlying();
    }

    private void HandleFlying()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * _mouseSensitivity * Time.deltaTime;

        //_velocity.z += moveInput.y * _speed * Time.deltaTime;

        Vector3 movement = transform.forward * moveInput.y * _speed * Time.deltaTime;

        transform.Rotate(-lookInput.y, lookInput.x, -moveInput.x * _rollSpeed * Time.deltaTime);

        if (boostAction.IsPressed())
        {
            _currentBoost = _boostMultiplier;
            _inertiaFactor = 0.9f;
        }
        else
        {
            _currentBoost = 1f;
            _boostTimer -= Time.deltaTime;

            if (_boostTimer <= 0f)
            {
                _inertiaFactor = 0.6f;
                _boostTimer = 3f;
            }
        }

        _velocity += movement * _currentBoost;




        if (moveInput == Vector2.zero)
        {
            _inertiaTimer -= Time.deltaTime;

            if (_inertiaTimer <= 0f)
            {
                _velocity *= _inertiaFactor;
                _inertiaTimer = 0.1f;
            }

        }


        transform.Translate(_velocity * Time.deltaTime, Space.World);

    }
}
