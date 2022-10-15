## Unity Bluetooth (Android)

Connect with bluetooth **without PAIRING** in Unity (Android).

## Available Features
- Scan/Connect to remote device
    - No need to pair the device beforehand
    - Support PIN code input
- Send/Write Message

## Demo
TODO

## Project Stucture
- `UnityBluetoohArduino` Contains the simple arduino project, for testing send/write functions.
- `UnityBluetooth-AndroidStudio` The Android Studio project, to write the android-specified code, and export the .jar plugin to integrate with unity.
- `UnityBluetooth-Unity` The Unity project, the jar and library is already here. *(You may just use this folder to build your app.)*


## Notes

### AndroidManifest Permission
These are permissions required!! You MUST add them to your `AndroidManifest.xml` manifest

```
    <!-- Bluetooth -->
    <uses-permission android:name="android.permission.BLUETOOTH"/>
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN"/>
    <!-- For scanning nearby devices -->
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
```

After you install the app, you may need to manually enable the permissions under android settings.