using UnityEngine;
using VContainer;

namespace FlightSimulation
{
    public class AircraftVisuals : MonoBehaviour
    {
        [Inject]
        private JSBSimManager jsbsim;

        [Header("Moving Parts")]
        [SerializeField] private Transform propeller;

        [SerializeField] private Transform leftAileron;
        [SerializeField] private Transform rightAileron;

        [SerializeField] private Transform elevatorLeft;
        [SerializeField] private Transform elevatorRight;

        [SerializeField] private Transform rudder;

        [Header("Settings")]
        [SerializeField] private float propellerSpeed = 1500f;

        [SerializeField] private float aileronAngle = 20f;

        [SerializeField] private float elevatorAngle = 15f;

        [SerializeField] private float rudderAngle = 25f;

        [SerializeField] private float smoothSpeed = 5f;

        private Quaternion leftAileronBaseRotation;
        private Quaternion rightAileronBaseRotation;

        private Quaternion elevatorLeftBaseRotation;
        private Quaternion elevatorRightBaseRotation;

        private Quaternion rudderBaseRotation;

        private float currentRoll;
        private float currentPitch;
        private float currentYaw;

        private void Start()
        {
            if (leftAileron != null)
                leftAileronBaseRotation = leftAileron.localRotation;

            if (rightAileron != null)
                rightAileronBaseRotation = rightAileron.localRotation;

            if (elevatorLeft != null)
                elevatorLeftBaseRotation = elevatorLeft.localRotation;

            if (elevatorRight != null)
                elevatorRightBaseRotation = elevatorRight.localRotation;

            if (rudder != null)
                rudderBaseRotation = rudder.localRotation;
        }

        private void Update()
        {
            if (jsbsim == null)
                return;

            AnimatePropeller();
            AnimateControlSurfaces();
        }

        private void AnimatePropeller()
        {
            if (propeller == null)
                return;

            float rpm =
                jsbsim.GetCurrentState()?.engine?.rpm ?? 0f;

            float speed =
                rpm * propellerSpeed * Time.deltaTime;

            propeller.Rotate(Vector3.forward, speed);
        }

        private void AnimateControlSurfaces()
        {
            currentRoll = Mathf.Lerp(
                currentRoll,
                jsbsim.roll,
                Time.deltaTime * smoothSpeed);

            currentPitch = Mathf.Lerp(
                currentPitch,
                jsbsim.pitch,
                Time.deltaTime * smoothSpeed);

            currentYaw = Mathf.Lerp(
                currentYaw,
                jsbsim.yaw,
                Time.deltaTime * smoothSpeed);

            // Clamp
            currentRoll = Mathf.Clamp(currentRoll, -1f, 1f);
            currentPitch = Mathf.Clamp(currentPitch, -1f, 1f);
            currentYaw = Mathf.Clamp(currentYaw, -1f, 1f);

            // Ailerons
            if (leftAileron != null)
            {
                leftAileron.localRotation =
                    leftAileronBaseRotation *
                    Quaternion.Euler(
                        currentRoll * aileronAngle,
                        0,
                        0);
            }

            if (rightAileron != null)
            {
                rightAileron.localRotation =
                    rightAileronBaseRotation *
                    Quaternion.Euler(
                        -currentRoll * aileronAngle,
                        0,
                        0);
            }

            // Elevators
            if (elevatorLeft != null)
            {
                elevatorLeft.localRotation =
                    elevatorLeftBaseRotation *
                    Quaternion.Euler(
                        currentPitch * elevatorAngle,
                        0,
                        0);
            }

            if (elevatorRight != null)
            {
                elevatorRight.localRotation =
                    elevatorRightBaseRotation *
                    Quaternion.Euler(
                        currentPitch * elevatorAngle,
                        0,
                        0);
            }

            // Rudder
            if (rudder != null)
            {
                rudder.localRotation =
                    rudderBaseRotation *
                    Quaternion.Euler(
                        0,
                        currentYaw * rudderAngle,
                        0);
            }
        }
    }
}