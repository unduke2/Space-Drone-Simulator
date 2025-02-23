using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;



    private InputAction _moveAction;
    private InputAction _boostAction;
    private InputAction _lookAction;
    private InputAction _fireAction;


    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool IsBoostPressed { get; private set; }

    public bool IsFirePressed { get; private set; }


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



        _moveAction = InputSystem.actions.FindAction("Move");
        _boostAction = InputSystem.actions.FindAction("Boost");
        _lookAction = InputSystem.actions.FindAction("Look");
        _fireAction = InputSystem.actions.FindAction("Fire");
        _boostAction.performed += OnBoost;
        _boostAction.canceled += OnBoost;
        _fireAction.performed += OnFire;
        _fireAction.canceled += OnFire;


        _moveAction.Enable();
        _boostAction.Enable();
        _lookAction.Enable();
        _fireAction.Enable();


    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _boostAction.Disable();
        _lookAction.Disable();
        _fireAction.Disable();

        _boostAction.performed -= OnBoost;
        _boostAction.canceled -= OnBoost;
        _fireAction.performed -= OnFire;
        _fireAction.canceled -= OnFire;
    }

    private void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        LookInput = _lookAction.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        IsBoostPressed = context.performed;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        IsFirePressed = context.performed;
    }

}
