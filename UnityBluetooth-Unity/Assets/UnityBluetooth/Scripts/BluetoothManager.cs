using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity Bluetooth (Android)
/// https://github.com/JCxYIS/UnityBluetooth
/// </summary>
public class BluetoothManager
{
    private static AndroidJavaClass javaClass; // aka JC :P
    private static AndroidJavaObject javaInstance = null;   

     /* -------------------------------------------------------------------------- */

    [System.Serializable]
    public struct BluetoothDevice
    {
        public string name;
        public string mac;        
    }

     /* -------------------------------------------------------------------------- */

    private static void EnsureInstance()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            throw new System.NotSupportedException("BluetoothManager currently only works on Android.");
        }

        if(javaInstance == null)
        {
            // unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            // context = activity.Call<AndroidJavaObject>("getApplicationContext");
            javaClass = new AndroidJavaClass("com.jcxyis.unitybluetooth.BluetoothManager");
            javaInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance");
            Debug.Log("[BluetoothManager] Inited.");
        }                    
    }

    /* -------------------------------------------------------------------------- */
    

    /// <summary>
    /// Start scanning nearby bluetooth devices
    /// </summary>
    public static void StartDiscovery()
    {
        EnsureInstance();
        javaInstance.Call("StartDiscovery");
    }

    /// <summary>
    /// Get the list of nearby devices.
    /// Don't forget to call StartDiscovery() first!
    /// </summary>
    /// <returns>A string list. String form: <c>"{Name}|{MAC}"</c></returns>
    public static List<BluetoothDevice> GetAvailableDevices()
    {
        EnsureInstance();
        string[] devices = javaInstance.Call<string[]>("GetAvailableDevices");
        List<BluetoothDevice> deviceList = new List<BluetoothDevice>();
        string[] deviceRawStr;
        for(int i = 0; i < devices.Length; i++)
        {
            deviceRawStr = devices[i].Split('|');
            deviceList.Add(new BluetoothDevice
            {
                name = deviceRawStr[0],
                mac = deviceRawStr[1]
            });
        }
        return deviceList;
    }

    /// <summary>
    /// Connect to a device with the specified address.
    /// </summary>
    /// <param name="mac">Address</param>
    /// <param name="pin">If the device hasn't been bonded and it has PIN, then this field is required.</param>
    /// <returns></returns>
    public static bool Connect(string mac, string pin = "")
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Connect", mac, pin);
    }

    /// <summary>
    /// Send data to the remote device.
    /// You need to append newline (e.g. '\n' or '\r\n') by yourself.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>    
    public static bool Send(string data)
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Send", data);
    }

    /// <summary>
    /// Input stream buffer size
    /// </summary>
    /// <returns></returns>
    public static int Available()
    {
        return javaInstance.Call<int>("Available");
    }

    /// <summary>
    /// Read a line from input stream
    /// </summary>
    /// <returns></returns>
    public static string ReadLine()
    {
        EnsureInstance();
        return javaInstance.Call<string>("ReadLine");
    }

    /// <summary>
    /// Stop the connection
    /// </summary>
    public static void Stop()
    {
        EnsureInstance();
        javaInstance.Call("Stop");
    }

    /* -------------------------------------------------------------------------- */

    /// <summary>
    /// Show a Toast (bottom message)
    /// </summary>
    public static void Toast(string msg)
    {
        EnsureInstance();
        javaClass.CallStatic("Toast", msg);
    }

    /* -------------------------------------------------------------------------- */

    public static string[] StrArrTest(int len)
    {
        EnsureInstance();
        return javaInstance.Call<string[]>("StrArrTest", len);
    }
}
