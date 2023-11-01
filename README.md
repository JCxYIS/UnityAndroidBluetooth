## Unity Bluetooth (Android)

Connect with bluetooth in Unity (Android).

## Features
- Scan/Connect to remote device
    - No need to pair the device beforehand
    - Support PIN code input
- Send/Write Message

## Demo
<img src="https://i.imgur.com/SiCdH6U.png" height="400" /> <img src="https://i.imgur.com/Tw0lsmE.png" height="400" />




## Project Stucture
- `UnityBluetoohArduino` Contains the simple arduino project, for testing send/write functions.
- `UnityBluetooth-AndroidStudio` The Android Studio project, to write the android-specified code, and export the .jar plugin to integrate with unity. (the jar is under `./app/release`)
- `UnityBluetooth-Unity` The Unity project, the aar library is already in here. *(You only need this to build your unity app.)*
    - You may be interested in `BluetoothManager`

## Notes

After you install the app, you may need to manually enable the permissions under android application settings.

<img src="https://i.imgur.com/33vq1ev.png" height="400" />
