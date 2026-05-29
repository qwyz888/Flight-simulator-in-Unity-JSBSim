using TMPro;
using UnityEngine;
using VContainer;

namespace FlightSimulation
{
    public class FlightInfoUI: MonoBehaviour
    {
        [Inject]
        private JSBSimManager _jsbsim;

        [Header("UI")]
        [SerializeField]
        private TMP_Text throttleText;

        [SerializeField]
        private TMP_Text speedText;

        [SerializeField]
        private TMP_Text altitudeText;

        [SerializeField]
        private TMP_Text pitchText;

        [SerializeField]
        private TMP_Text rollText;

        [SerializeField]
        private TMP_Text yawText;

        [SerializeField]
        private TMP_Text engineText;

        private void Update()
        {
            if (_jsbsim == null)
                return;

            AircraftState state =
                _jsbsim.GetCurrentState();

            if (state == null)
                return;

            throttleText.text =
                $"THR: {Mathf.RoundToInt(_jsbsim.throttle * 100f)}%";

            speedText.text =
                $"SPD: {state.velocity.airspeed:F0} kt";

            altitudeText.text =
                $"ALT: {state.position.alt:F0} ft";

            pitchText.text =
                $"PITCH: {(state.orientation.pitch * Mathf.Rad2Deg):F1}°";

            rollText.text =
                $"ROLL: {(state.orientation.roll * Mathf.Rad2Deg):F1}°";

            yawText.text =
                $"YAW: {(state.orientation.yaw * Mathf.Rad2Deg):F1}°";

            engineText.text =
                $"RPM: {state.engine.rpm:F0}";
        }
    }
}