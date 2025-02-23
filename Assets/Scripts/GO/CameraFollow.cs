using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _smoothStep;
    float _minVelocity = 0f;
    float _maxVelocity = 300f;
    float _minFOV = 60f;
    float _maxFOV = 120f;
    public float _currentFOV;
    private float _targetFOV;
    void Start()
    {
        _targetFOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    private void Update()
    {



        transform.position = Vector3.Lerp(transform.position, _followTarget.position, _smoothStep * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _followTarget.rotation, _smoothStep * Time.deltaTime);

    }

    public void UpdateFOV(float velocityMagnitude)
    {
        float t = Mathf.InverseLerp(_minVelocity, _maxVelocity, velocityMagnitude);

        float targetFOV = Mathf.SmoothStep(_minFOV, _maxFOV, t);

        Camera.main.fieldOfView = targetFOV;

    }
}
