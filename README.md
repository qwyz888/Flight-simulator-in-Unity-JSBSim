Flight simulator (Unity) — JSBSim bridge

UA
====
Коротко
----
Цей репозиторій містить Unity-проєкт, який взаємодіє із JSBSim через невеликий Python "bridge" (UDP). Unity надсилає керування (throttle, aileron, elevator, rudder) в Python, а Python/JSBSim відправляє назад телеметрію (позиція, орієнтація, швидкість тощо). Файли JSBSim зберігаються у Assets/StreamingAssets/JSBSim.

Швидкий гайд по встановленню (Windows)
----
1) Встановити Python 3.8+ з https://www.python.org/downloads/ (встановити Add to PATH).
2) Встановити binding JSBSim для Python (якщо потрібен): в PowerShell виконати: pip install jsbsim
3) Якщо пакет не знаходить дані JSBSim, встановіть змінну оточення JSBSIM_ROOT на шлях до папки з даними (наприклад Assets/StreamingAssets/JSBSim):
   setx JSBSIM_ROOT "F:\\UnityHub\\Projects\\Flight-simulator-in-Unity-JSBSim\\Assets\\StreamingAssets\\JSBSim"
4) Запуск мосту вручну (опціонально): у корені репозиторію запустити: python bridge_jsbsim.py

Запуск з Unity
----
- Компонент JSBSimManager запускає Python-скрипт при старті. За замовчуванням scriptPath = "jsbsim_bridge.py", а в репозиторії файл називається "bridge_jsbsim.py" — змініть scriptPath в інспекторі або перейменуйте файл.
- Порти за замовчуванням: Unity → Python (controls) = UDP 5555, Python → Unity (telemetry) = UDP 5556.

Короткий опис взаємодії
----
- Unity (JSBSimManager) запускає Python bridge і використовує UDP: відправляє JSON з керуванням, отримує JSON з телеметрією.
- Python-скрипт створює JSBSim FGFDMExec, застосовує отримані команди до властивостей (наприклад propulsion/throttle-cmd-norm, fcs/*-cmd-norm), крокує симуляцію з налаштованим DT і відправляє назад телеметрію.
- Unity десеріалізує AircraftState і оновлює трансформ літака (позицію та орієнтацію). Конвертація орієнтацій з NED → Unity реалізована у JSBSimManager.GetUnityRotation().

EN
====
Summary
----
This repository contains a Unity flight-simulator project integrated with JSBSim via a small Python bridge (UDP). Unity sends control inputs (throttle, aileron, elevator, rudder) to Python; Python/JSBSim runs the flight dynamics and returns telemetry (position, attitude, speed, etc.). JSBSim data files are stored in Assets/StreamingAssets/JSBSim.

Quick installation (Windows)
----
1) Install Python 3.8+ from https://www.python.org/ and enable Add to PATH.
2) Install the JSBSim Python package if using the Python binding: pip install jsbsim
3) If the package cannot find the JSBSim data, set JSBSIM_ROOT to the data folder included in this repo (Assets/StreamingAssets/JSBSim):
   setx JSBSIM_ROOT "F:\\UnityHub\\Projects\\Flight-simulator-in-Unity-JSBSim\\Assets\\StreamingAssets\\JSBSim"
4) Run the bridge manually (optional): python bridge_jsbsim.py

Running from Unity
----
- JSBSimManager component will try to start the Python script on Start. The scriptPath default is "jsbsim_bridge.py" while the repository contains "bridge_jsbsim.py" — update scriptPath in the inspector or rename the file.
- Default ports: Unity -> Python (controls) UDP 5555, Python -> Unity (telemetry) UDP 5556. These are configurable in JSBSimManager.

How Unity and JSBSim interact
----
- Unity launches/attaches to the Python bridge and opens UDP sockets. Unity serializes control inputs as JSON and sends them to the Python bridge.
- The Python bridge runs a JSBSim FGFDMExec instance, applies control values to JSBSim properties, steps the simulation at a configured DT, and sends telemetry JSON back to Unity.
- Unity deserializes telemetry into AircraftState and updates the aircraft transform. Rotation conversion (JSBSim NED → Unity) is implemented in JSBSimManager.

Troubleshooting
----
- If Unity does not receive telemetry: check that the Python bridge is running, firewall rules, and matching UDP ports. Inspect Logs folder created by JSBSimManager for runtime messages.
- Ensure scriptPath in JSBSimManager matches the actual Python filename in the repo or rename the script accordingly.
- If JSBSim cannot find aircraft data, point JSBSIM_ROOT to Assets/StreamingAssets/JSBSim or install a JSBSim distro with data.

Key files
----
- bridge_jsbsim.py — Python bridge that runs JSBSim and speaks UDP JSON.
- Assets/_Project/Scripts/FlightSimulation/JSBSimManager.cs — Unity side manager that launches Python and handles UDP send/receive.
- Assets/_Project/Scripts/FlightSimulation/JSBSimAircraftController.cs — updates GameObject transform from telemetry.

License / Notes
----
This README provides quick start instructions. For deeper JSBSim configuration examine the files in Assets/StreamingAssets/JSBSim (aircraft, systems, engines, etc.).
