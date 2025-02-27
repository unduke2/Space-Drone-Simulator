using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _shipFollowTarget;
    [SerializeField] private Transform _turretFollowTarget;
    [SerializeField] private float _smoothStep;
    float _minVelocity = 0f;
    float _maxVelocity = 300f;
    float _minFOV = 60f;
    float _maxFOV = 120f;
    public float _currentFOV;
    private float _targetFOV;
    private bool _isAiming = false;
    private bool _updatedView = false;
    void Start()
    {
        _targetFOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(_isAiming);
        if (GameInput.Instance.IsViewPressed && _updatedView == false) 
        {
            _updatedView = true;
            _isAiming = !_isAiming;

        }
        if (!GameInput.Instance.IsViewPressed)
        {
            _updatedView = false;
        }

        if (_isAiming)
        {
            transform.position = _turretFollowTarget.position;
            transform.rotation = _turretFollowTarget.rotation;
        }
        else
        {

            transform.position = Vector3.Lerp(transform.position, _shipFollowTarget.position, _smoothStep * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _shipFollowTarget.rotation, _smoothStep * Time.deltaTime);
        }

    }

    public void UpdateFOV(float velocityMagnitude)
    {
        float t = Mathf.InverseLerp(_minVelocity, _maxVelocity, velocityMagnitude);

        float targetFOV = Mathf.SmoothStep(_minFOV, _maxFOV, t);

        Camera.main.fieldOfView = targetFOV;

    }
}
