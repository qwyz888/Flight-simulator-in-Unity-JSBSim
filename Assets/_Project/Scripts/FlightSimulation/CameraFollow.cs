using Infrastructure.Services.Input.Core;
using UnityEngine;
using VContainer;

namespace FlightSimulation
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Orbit")]
        [SerializeField] private bool enableOrbit = true;
        [SerializeField] private Vector2 orbitSensitivity = new Vector2(120f, 80f);
        [SerializeField] private float minPitch = -10f;
        [SerializeField] private float maxPitch = 80f;

        [Header("Camera Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float smoothSpeed = 5f;

        private float _yawAngle;
        private float _pitchAngle;
        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            Vector3 e = transform.eulerAngles;
            _yawAngle = e.y;
            _pitchAngle = e.x;
        }

        void LateUpdate()
        {
            if (target == null) return;

            Vector2 look = Vector2.zero;
            if (_inputService != null && _inputService.Gameplay != null && _inputService.Gameplay.Look != null)
                look = _inputService.Gameplay.Look.Value;

            if (enableOrbit && look.sqrMagnitude > 0f)
            {
                _yawAngle += look.x * orbitSensitivity.x * Time.deltaTime;
                _pitchAngle -= look.y * orbitSensitivity.y * Time.deltaTime;
                _pitchAngle = Mathf.Clamp(_pitchAngle, minPitch, maxPitch);
            }

            Quaternion rot = Quaternion.Euler(_pitchAngle, _yawAngle, 0f);
            Vector3 rotatedOffset = rot * offset;

            Vector3 desiredPosition = target.position + rotatedOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }
}