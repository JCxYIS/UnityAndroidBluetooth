## Unity Bluetooth (Android)

Connect with bluetooth in Unity (Android).

## Features
- Scan/Connect to remote device
    - No need to pair the device beforehand
    - Support PIN code input
- Send/Write Message

## Demo
<img src="https://i.imgur.com/SiCdH6U.png" style="max-height: 400px" />
<img src="https://i.imgur.com/efwZzib.png" style="max-height: 400px" />
<img src="https://i.imgur.com/Tw0lsmE.png" style="" />


## Notes

### AndroidManifest Permission
These are permissions required!! You **MUST** add them to your `AndroidManifest.xml` manifest.

```xml
    <!-- Bluetooth -->
    <uses-permission android:name="android.permission.BLUETOOTH"/>
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN"/>
    <!-- For scanning nearby devices -->
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
```

After you install the app, you may need to manually enable the permissions under android application settings.

<img src="https://i.imgur.com/33vq1ev.png" style="max-height: 400px" />


## Project Stucture
- `UnityBluetoohArduino` Contains the simple arduino project, for testing send/write functions.
- `UnityBluetooth-AndroidStudio` The Android Studio project, to write the android-specified code, and export the .jar plugin to integrate with unity. (the jar is under `./app/release`)
- `UnityBluetooth-Unity` The Unity project, the jar and library is already here. *(You only need this to build your unity app.)*
