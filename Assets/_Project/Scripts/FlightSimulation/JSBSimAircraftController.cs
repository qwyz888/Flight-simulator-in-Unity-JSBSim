using UnityEngine;
using VContainer;

namespace FlightSimulation
{
    public class AircraftController : MonoBehaviour
    {
        private FlightInputProvider _input;

        private JSBSimManager _jsbsim;

        [Inject]
        public void Construct(FlightInputProvider flightInput, JSBSimManager jsbsim)
        {
            _input = flightInput;
            _jsbsim = jsbsim;
        }

        private void Start()
        {
            if (_jsbsim == null)
            {
                Debug.LogError("JSBSimManager was not injected!");
                enabled = false;
                return;
            }

            Debug.Log("AircraftController initialized");
        }

        private void Update()
        {
            if (_jsbsim == null)
                return;

            HandleInput();

            UpdateTransform();
        }

        private void HandleInput()
        {
            FlightInputData input =
                _input.GetInput();

            _jsbsim.roll =
                input.Roll;

            _jsbsim.pitch =
                -input.Pitch;

            _jsbsim.yaw =
                input.Yaw;

            _jsbsim.throttle =
                input.Throttle;

        }

        void UpdateTransform()
        {
            Vector3 unityPosition = _jsbsim.GetUnityPosition();
            Quaternion unityRotation = _jsbsim.GetUnityRotation();
            transform.position = unityPosition;
            transform.rotation = unityRotation;
        }
    }
}