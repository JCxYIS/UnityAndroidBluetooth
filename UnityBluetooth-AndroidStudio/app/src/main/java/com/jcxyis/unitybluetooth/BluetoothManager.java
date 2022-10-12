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
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.UUID;

@SuppressLint("MissingPermission")
public class BluetoothManager extends UnityPlayerActivity {
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


    // --- Constructor ---
    public BluetoothManager() {

    }


    // --- Bt ---

    // Start Discovering devices
    public void StartDiscovery() {
        // init
        // Get device list
        IntentFilter filter = new IntentFilter(BluetoothDevice.ACTION_FOUND);
        filter.addAction(BluetoothAdapter.ACTION_DISCOVERY_FINISHED);
        this.registerReceiver(discoverFinishHandler, filter);

        // Input pin code in code
//        IntentFilter filter2 = new IntentFilter(BluetoothDevice.ACTION_PAIRING_REQUEST);
//        this.registerReceiver(pairRequestHandler, filter2);

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
    public boolean Connect(String mac) {
        connectedDevice = bt.getRemoteDevice(mac);
        UUID myUUID = UUID.fromString("00001101-0000-1000-8000-9487010C8763"); // random uuid :P
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

    // Read line
    public String ReadLine(){
        try {
            InputStream inputStream = socket.getInputStream();
            if(inputStream.available() > 0){
                BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
                String r = reader.readLine();
                Log.v("tManager", "Get "+r);
                return r;
            }
            else return "";
        } catch (Exception e) {
            return "";
        }
    }

    // Stop
    @SuppressLint("MissingPermission")
    public void Stop() {
        bt.disable();
        Log.i("BtManager", "Stop! BT Shut down");
    }

    // --- Intent filters ---
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
                }
            }
            // end discovery
            else if (BluetoothAdapter.ACTION_DISCOVERY_FINISHED.equals(action)) {
                Log.v("BtManager","Discovery Finished ");
            }
        }
    };

    // https://stackoverflow.com/questions/35519321/android-bluetooth-pairing-without-user-enter-pin-and-confirmation-using-android
    private final BroadcastReceiver pairRequestHandler = new BroadcastReceiver() {
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (action.equals(BluetoothDevice.ACTION_PAIRING_REQUEST)) {
                try {
                    BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
                    int pin=intent.getIntExtra("android.bluetooth.device.extra.PAIRING_KEY", 1234);
                    //the pin in case you need to accept for an specific pin
                    Log.d("BtManager", "Start Pairing with PIN....");
                    byte[] pinBytes;
                    pinBytes = (""+pin).getBytes("UTF-8");
                    device.setPin(pinBytes);
                    // setPairing confirmation if neeeded
                    device.setPairingConfirmation(true);
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


}
