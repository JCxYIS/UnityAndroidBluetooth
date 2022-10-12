using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothManager
{
    const string CLASS_NAME = "com.jcxyis.unitybluetooth";

    private static AndroidJavaClass javaClass; // aka JC :P
    private static AndroidJavaObject javaInstance = null;   

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
            javaClass = new AndroidJavaClass(CLASS_NAME);
            javaInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance");
            Debug.Log("[BluetoothManager] Inited.");
        }                    
    }

    /* -------------------------------------------------------------------------- */
    

    public static bool StartDiscovery()
    {
        EnsureInstance();
        return javaInstance.Call<bool>("StartDiscovery");
    }

    public static string[] GetAvailableDevices()
    {
        EnsureInstance();
        return javaInstance.Call<string[]>("GetAvailableDevices");
    }

    public static bool Connect(string mac)
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Connect", mac);
    }
    
    public static bool Send(string data)
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Send", data);
    }

    public static string ReadLine()
    {
        EnsureInstance();
        return javaInstance.Call<string>("ReadLine");
    }

    public static void Stop()
    {
        EnsureInstance();
        javaInstance.Call("Stop");
    }

    /// <summary>
    /// show a Toast 
    /// </summary>
    public void Toast(string msg)
    {
        javaClass.CallStatic("Toast", msg);
    }
}
