#include <SoftwareSerial.h>

SoftwareSerial bluetooth(10, 11); // (RX, TX)

void setup() {
  // Set baud rates
  Serial.begin(9600);
  bluetooth.begin(9600);
}

void loop() {  

  // Get input from serial, and send to remote via bluetooth
  if(Serial.available() > 0) {
    String input = Serial.readString();  
    // teststr.trim(); // remove /n or /r at the end
    bluetooth.println(input);
  }

  // Get message from blutetooth, and print in serial
  if(bluetooth.available()) {
    String msg = bluetooth.readString();
    Serial.println("[From Bluetooth] " + msg);
  }  
  
  delay(10); // ms
}