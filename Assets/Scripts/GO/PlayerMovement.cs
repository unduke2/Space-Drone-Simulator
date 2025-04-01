namespace Implementation.GameObject
{

    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerCamera _playerCamera;

        private Vector3 _velocity;

        [SerializeField] private float _invertDirSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _topSpeed;
        [SerializeField] private float _sensitivity;
        [SerializeField] private float _rollSpeed;
        [SerializeField] private float _drag;
        [SerializeField] private float _boostMultiplier;
        private float _speedMultiplier = 1f;
        private float _currentPitch;
        private float _currentYaw;
        private float _currentRoll;
        [SerializeField] private float _slowDownFactor;


        void Start()
        {

            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update()
        {
            HandleFlying();
        }


        public void HandleFlying()
        {
            float dot = Vector3.Dot(transform.forward, _velocity);

            if (dot < -0.1f)
            {
                _velocity *= 1f - (_invertDirSpeed * Time.deltaTime);
            }

            _velocity += transform.forward * GameInput.Instance.MoveInput.y * (_acceleration * _speedMultiplier) * Time.deltaTime;

            if (_velocity.magnitude > _topSpeed * _speedMultiplier)
            {
                _velocity = Vector3.Lerp(_velocity, _velocity.normalized * (_topSpeed * _speedMultiplier), Time.deltaTime * _slowDownFactor);
            }

            _speedMultiplier = GameInput.Instance.IsBoostPressed ? _boostMultiplier : 1f;

            if (GameInput.Instance.MoveInput == Vector2.zero && _velocity.magnitude > 0)
            {
                _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * Time.deltaTime);
            }

            _currentPitch = -GameInput.Instance.LookInput.y * _sensitivity * Time.deltaTime;
            _currentYaw = GameInput.Instance.LookInput.x * _sensitivity * Time.deltaTime;
            _currentRoll = -GameInput.Instance.MoveInput.x * _rollSpeed * Time.deltaTime;

            transform.Rotate(_currentPitch, _currentYaw, _currentRoll);
            transform.Translate(_velocity * Time.deltaTime, Space.World);


            if (_velocity.magnitude < 0.01f)
            {
                _velocity = Vector3.zero;
            }

            _playerCamera.UpdateFOV(_velocity.magnitude);

        }
    }
}