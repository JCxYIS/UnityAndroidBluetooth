using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothManager
{
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
            javaClass = new AndroidJavaClass("com.jcxyis.unitybluetooth.BluetoothManager");
            javaInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance");
            Debug.Log("[BluetoothManager] Inited.");
        }                    
    }

    /* -------------------------------------------------------------------------- */
    

    public static void StartDiscovery()
    {
        EnsureInstance();
        javaInstance.Call("StartDiscovery");
    }

    public static string[] GetAvailableDevices()
    {
        EnsureInstance();
        return javaInstance.Call<string[]>("GetAvailableDevices");
    }

    public static bool Connect(string mac, string pin)
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Connect", mac, pin);
    }
    
    public static bool Send(string data)
    {
        EnsureInstance();
        return javaInstance.Call<bool>("Send", data);
    }

    public static int Available()
    {
        return javaInstance.Call<int>("Available");
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
    public static void Toast(string msg)
    {
        EnsureInstance();
        javaClass.CallStatic("Toast", msg);
    }

    public static string[] StrArrTest(int len)
    {
        EnsureInstance();
        return javaInstance.Call<string[]>("StrArrTest", len);
    }
}
