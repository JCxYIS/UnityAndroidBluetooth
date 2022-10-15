// #define USE_HC0506 true  // if use HC-05/HC-06, uncomment this
#define USE_ESP32 true  // if use esp32, uncomment this


#if USE_HC0506
#include <SoftwareSerial.h> // HC-05/ HC-06
SoftwareSerial bluetooth(12, 13); // Manually set (RX, TX) of HC-05/ HC-06

#elif USE_ESP32
#include <BluetoothSerial.h> // built in bluetooth, e.g. esp32
BluetoothSerial bluetooth; // built in bluetooth, e.g. esp32

#endif




void setup() {
  // Set baud rates
  Serial.begin(9600);
  
#if USE_HC0506
  bluetooth.begin(9600); // HC baud rate
#endif

#if USE_ESP32
  bluetooth.begin("My ESP32 Bluetooth"); // esp32: set device name
  bluetooth.setPin("1234"); // no use?
#endif
}

void loop() {  

  // Get input from serial, and send to remote via bluetooth
  if(Serial.available()) {
    String s = Serial.readString();
    bluetooth.print(s);
    Serial.print("BLUETOOTH SEND >> " + s);
  }

  // Get message from blutetooth, and print in serial
  if(bluetooth.available()) {
    // String msg = bluetooth.readString();
    // Serial.println("[From Bluetooth] " + msg);
    String s = bluetooth.readString();
    Serial.print("BLUETOOTH GET << " + s);
  }  
  
  // delay(10); // ms
}