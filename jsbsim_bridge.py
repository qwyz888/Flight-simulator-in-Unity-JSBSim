import jsbsim
import socket
import json
import sys
import time
import math
import os
import traceback

class JSBSimBridge:

    def __init__(self, port=5555):

        self.port = port
        self.running = False

        self.origin_lat = 37.4
        self.origin_lon = -122.4
        self.origin_alt = 100.0

        self.sim_dt = 0.01

        self.fdm = None

    # =========================================================
    # INIT
    # =========================================================

    def initialize(self):

        try:
            print("[INFO] Initializing JSBSim")

            root = jsbsim.get_default_root_dir()

            self.fdm = jsbsim.FGFDMExec(root)

            self.fdm.set_aircraft_path(os.path.join(root, "aircraft"))
            self.fdm.set_engine_path(os.path.join(root, "engine"))
            self.fdm.set_systems_path(os.path.join(root, "systems"))

            self.fdm.set_dt(self.sim_dt)

            # =====================================================
            # LOAD CESSNA 172
            # =====================================================

            if not self.fdm.load_model("c172p"):
                print("[ERROR] Cannot load c172p")
                return False

            print("[SUCCESS] Loaded aircraft: c172p")

            # =====================================================
            # INITIAL CONDITIONS
            # =====================================================

            self.fdm["ic/lat-geod-deg"] = self.origin_lat
            self.fdm["ic/long-gc-deg"] = self.origin_lon

            self.fdm["ic/h-sl-ft"] = self.origin_alt
            self.fdm["ic/terrain-elevation-ft"] = self.origin_alt

            self.fdm["ic/u-fps"] = 0
            self.fdm["ic/v-fps"] = 0
            self.fdm["ic/w-fps"] = 0

            self.fdm["ic/psi-true-deg"] = 0
            self.fdm["ic/theta-deg"] = 0
            self.fdm["ic/phi-deg"] = 0

            self.fdm["ic/fuel-fraction"] = 1.0

            if not self.fdm.run_ic():
                print("[ERROR] run_ic failed")
                return False

            # =====================================================
            # START ENGINE
            # =====================================================

            self.fdm["propulsion/set-running"] = -1

            # release brakes
            try:
                self.fdm["gear/parking-brake"] = 0
            except:
                pass

            for _ in range(100):
                self.fdm.run()

            print("[SUCCESS] Aircraft initialized")

            return True

        except Exception as e:
            print("[ERROR]", e)
            traceback.print_exc()
            return False

    # =========================================================
    # CONTROLS
    # =========================================================

    def set_controls(self, throttle=0, roll=0, pitch=0, yaw=0):

        throttle = max(0.0, min(1.0, float(throttle)))
        roll = max(-1.0, min(1.0, float(roll)))
        pitch = max(-1.0, min(1.0, float(pitch)))
        yaw = max(-1.0, min(1.0, float(yaw)))

        # =====================================================
        # FIXED WING CONTROLS
        # =====================================================

        self.fdm["fcs/throttle-cmd-norm[0]"] = throttle

        self.fdm["fcs/aileron-cmd-norm"] = roll

        self.fdm["fcs/elevator-cmd-norm"] = pitch

        self.fdm["fcs/rudder-cmd-norm"] = yaw

    # =========================================================
    # STEP
    # =========================================================

    def step(self):

        if self.fdm.run():
            return self.get_state()

        return None

    # =========================================================
    # STATE
    # =========================================================

    def get_state(self):

        lat = float(self.fdm['position/lat-geod-deg'])
        lon = float(self.fdm['position/long-gc-deg'])
        alt = float(self.fdm['position/h-sl-ft'])

        lat_diff = lat - self.origin_lat
        lon_diff = lon - self.origin_lon
        alt_diff = alt - self.origin_alt

        m_per_deg_lat = 111320.0
        m_per_deg_lon = 111320.0 * math.cos(math.radians(lat))

        unity_x = lon_diff * m_per_deg_lon
        unity_y = alt_diff * 0.3048
        unity_z = lat_diff * m_per_deg_lat

        return {

            "position": {
                "lat": lat,
                "lon": lon,
                "alt": alt,
                "unity_x": unity_x,
                "unity_y": unity_y,
                "unity_z": unity_z
            },

            "orientation": {
                "roll": float(self.fdm['attitude/roll-rad']),
                "pitch": float(self.fdm['attitude/pitch-rad']),
                "yaw": float(self.fdm['attitude/psi-rad'])
            },

            "velocity": {
                "u": float(self.fdm['velocities/u-fps']),
                "v": float(self.fdm['velocities/v-fps']),
                "w": float(self.fdm['velocities/w-fps']),
                "airspeed": float(self.fdm['velocities/vc-kts'])
            }
        }

    # =========================================================
    # SERVER
    # =========================================================

    def run_server(self):

        sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

        sock.bind(("127.0.0.1", self.port))

        sock.settimeout(0.001)

        print(f"[INFO] Listening on {self.port}")

        self.running = True

        next_tick = time.perf_counter()

        while self.running:

            # =============================================
            # RECEIVE CONTROLS
            # =============================================

            try:

                data, _ = sock.recvfrom(2048)

                msg = json.loads(data.decode())

                self.set_controls(
                    throttle=msg.get("throttle", 0),
                    roll=msg.get("aileron", 0),
                    pitch=msg.get("elevator", 0),
                    yaw=msg.get("rudder", 0)
                )

            except socket.timeout:
                pass

            except Exception as e:
                print("[WARN]", e)

            # =============================================
            # SIM STEP
            # =============================================

            now = time.perf_counter()

            if now >= next_tick:

                state = self.step()

                next_tick += self.sim_dt

                if state:

                    sock.sendto(
                        json.dumps(state).encode(),
                        ("127.0.0.1", self.port + 1)
                    )

    # =========================================================
    # MAIN
    # =========================================================

if __name__ == "__main__":

    bridge = JSBSimBridge()

    if bridge.initialize():

        try:
            bridge.run_server()

        except KeyboardInterrupt:
            bridge.running = False
            print("Stopping...")