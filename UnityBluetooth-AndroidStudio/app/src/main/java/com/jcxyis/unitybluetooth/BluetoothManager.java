package com.jcxyis.unitybluetooth;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import android.annotation.SuppressLint;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.util.Log;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.UUID;

@SuppressLint("MissingPermission")
public class BluetoothManager  {
    // instance (expose for unity to get)
    private static final BluetoothManager _instance = new BluetoothManager();
    public static BluetoothManager getInstance() {
        return _instance;
    }

    // var
    public BluetoothAdapter bt;
    public ArrayList<BluetoothDevice> availableDevices = new ArrayList<BluetoothDevice>();
    public BluetoothDevice connectedDevice;
    public BluetoothSocket socket;
    public String usePin = "";


    // --- Constructor ---
    public BluetoothManager() {
        // Init intent filters
        // Get device list
        IntentFilter filter = new IntentFilter(BluetoothDevice.ACTION_FOUND);
        filter.addAction(BluetoothAdapter.ACTION_DISCOVERY_FINISHED);
        UnityPlayer.currentActivity.registerReceiver(discoverFinishHandler, filter);

        // Input pin code
        IntentFilter filter2 = new IntentFilter(BluetoothDevice.ACTION_PAIRING_REQUEST);
        UnityPlayer.currentActivity.registerReceiver(pairRequestHandler, filter2);
    }


    // --- Bt ---

    // Start Discovering devices
    public void StartDiscovery() {
        // init the list
        availableDevices.clear();

        // bt start
        bt = BluetoothAdapter.getDefaultAdapter();
        if(!bt.isEnabled()) {
            bt.enable();
        }

        bt.startDiscovery();
        Log.i("BtManager", "Start Discovery...");
    }

    // Get Discovery list, in format "Name|Mac"
    public String[] GetAvailableDevices() {
        ArrayList<String> result = new ArrayList<>();
        for (BluetoothDevice device: availableDevices) {
            result.add(device.getName() + '|' + device.getAddress());
        }
        String[] arr = new String[result.size()];
        arr = result.toArray(arr);
        return arr;
    }

    // Connect
    public boolean Connect(final String mac, final String pin) {
        connectedDevice = bt.getRemoteDevice(mac);
        usePin = pin;
        UUID myUUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB"); // idk, magic uuid
        try {
            // create a RFCOMM (SPP) connection
            socket = connectedDevice.createInsecureRfcommSocketToServiceRecord(myUUID);
            bt.cancelDiscovery();

            // socket start!
            socket.connect();
            Log.i("BtManager", "Successfully Connect!");
            return true;
        }
        catch (Exception e) {
            bt.startDiscovery();
            Log.e("BtManager", "Failed to connect!!! ");
            e.printStackTrace();
            return false;
        }
    }

    public boolean Send(final String message) {
        try {
            socket.getOutputStream().write(message.getBytes());
            Log.v("tManager", "Sent "+message);
            return true;
        }
        catch (Exception e) {
            Log.e("BtManager", "Failed to send message");
            e.printStackTrace();
            return false;
        }
//        return false;
    }

    // Available
    public int Available() {
        try {
            return socket.getInputStream().available();
        }
        catch (Exception e) {
            Log.w("BtManager", "Read Error (avail)");
            e.printStackTrace();
            return -1;
        }
    }

    // Read line
    public String ReadLine(){
        try {
            InputStream inputStream = socket.getInputStream();
            if(inputStream.available() > 0){
                BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
                String r = reader.readLine();
                Log.v("BtManager", "Get "+r);
                return r;
            }
            else return "";
        } catch (Exception e) {
            Log.w("BtManager", "Read Error");
            e.printStackTrace();
            return "";
        }
    }

    // Stop
    public void Stop() throws IOException {
        if(socket != null) {
            socket.close();
        }
        socket = null;
        connectedDevice = null;
//        UnityPlayer.currentActivity.unregisterReceiver(discoverFinishHandler);
//        UnityPlayer.currentActivity.unregisterReceiver(pairRequestHandler);
        Log.i("BtManager", "Stopped!");
    }

    // --- Intent filters ---

    // Append available device list
    // https://stackoverflow.com/questions/19683034/getting-the-address-and-names-of-all-available-bluetooth-devices-in-android
    private final BroadcastReceiver discoverFinishHandler = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();

            // When discovery finds a device
            if (BluetoothDevice.ACTION_FOUND.equals(action)) {
                // Get the BluetoothDevice object from the Intent
                BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
                if(!availableDevices.contains(device)) {
                    availableDevices.add(device);
                    Log.i("BtManager", "Discover: "+device.getName() + "("+ device.getAddress() + ")");
                }
            }
            // end discovery
            else if (BluetoothAdapter.ACTION_DISCOVERY_FINISHED.equals(action)) {
                Log.i("BtManager","Discovery Finished ");
            }
        }
    };

    // Programmatically input pin code on connect
    // https://stackoverflow.com/questions/35519321/android-bluetooth-pairing-without-user-enter-pin-and-confirmation-using-android
    private final BroadcastReceiver pairRequestHandler = new BroadcastReceiver() {
        public void onReceive(Context context, Intent intent) {
            // no need to use pin
            if(usePin.equals("")) {
                return;
            }
            // use pin
            String action = intent.getAction();
            if (action.equals(BluetoothDevice.ACTION_PAIRING_REQUEST)) {
                try {
                    Log.d("BtManager", "Start Connecting with PIN....");
                    abortBroadcast(); // hide prompt
                    BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
//                    int pin = intent.getIntExtra("android.bluetooth.device.extra.PAIRING_KEY", usePin);
                    byte[] pinBytes;
                    pinBytes = usePin.getBytes(StandardCharsets.UTF_8);
                    device.setPin(pinBytes);
                    device.setPairingConfirmation(true); // setPairing confirmation if needed
                } catch (Exception e) {
                    Log.e("BtManager", "Error occurs when trying to auto pair");
                    e.printStackTrace();
                }
            }
        }
    };


    // --- Utility --

    public static void Toast(final String msg) //傳入的參入必須為final，才可讓Runnable()內部使用
    {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable()
        {
            @Override
            public void run()
            {
                Toast.makeText(UnityPlayer.currentActivity, msg, Toast.LENGTH_SHORT).show();
            }
        });
    }

    public String[] StrArrTest(int len) {
        ArrayList<String> arl = new ArrayList<String>();
        for(int i = 0; i < len; i++) {
            arl.add("abcd123|00:11:22:AA:BB:CC");
        }
        String[] arr = new String[arl.size()];
        arr = arl.toArray(arr);
        return arr;
    }
}
