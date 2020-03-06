/* ALT GAME JAM CONTROLLER
 *
 *  Project:  Alt Game Jam 2020
 *  Author:   Marshall Kaspari, Rohan Menon
 *  Type:     Analog/Digital controller
 *  Board:    Teensyduino
 *
*/
#include <LedControl.h>

// ---- DEFINES ----------------------------------------------
// Matrix Outputs
byte thrust0[8] = {0x00,0x04,0x04,0x0e,0x00,0x00,0x00,0x00};
byte thrust1[8] = {0x00,0x04,0x04,0x0e,0x00,0x01,0x01,0x01};
byte thrust2[8] = {0x00,0x00,0x04,0x04,0x0e,0x00,0x03,0x03};
byte thrust3[8] = {0x00,0x04,0x04,0x0e,0x00,0x07,0x07,0x07};
byte thrust4[8] = {0x00,0x04,0x04,0x0e,0x00,0x0f,0x0f,0x0f};
byte thrust5[8] = {0x00,0x04,0x04,0x0e,0x00,0x1f,0x1f,0x1f};
byte thrust6[8] = {0x00,0x04,0x04,0x0e,0x20,0x3f,0x3f,0x3f};
byte thrust7[8] = {0x00,0x04,0x44,0x4e,0x60,0x7f,0x7f,0x7f};
byte thrust8[8] = {0x80,0x84,0xc4,0xce,0xe0,0xff,0xff,0xff};

byte rudderMinus4[8] = {0xF8,0xF8,0x00,0x1E,0x3C,0x78,0x3C,0x1E};
byte rudderMinus3[8] = {0x78,0x38,0x00,0x1E,0x2C,0x58,0x2C,0x1E};
byte rudderMinus2[8] = {0x38,0x18,0x00,0x1E,0x24,0x48,0x24,0x1E};
byte rudderMinus1[8] = {0x18,0x08,0x00,0x12,0x24,0x48,0x24,0x12};
byte rudder0[8] = {0x18,0x18,0x00,0x24,0x24,0x24,0x24,0x24};
byte rudder1[8] = {0x18,0x10,0x00,0x48,0x24,0x12,0x24,0x48};
byte rudder2[8] = {0x1C,0x18,0x00,0x78,0x24,0x12,0x24,0x78};
byte rudder3[8] = {0x1E,0x1C,0x00,0x78,0x34,0x1A,0x34,0x78};
byte rudder4[8] = {0x1F,0x1F,0x00,0x78,0x3C,0x1E,0x3C,0x78};

byte aimMinus3[8] = {0x02,0x07,0x1e,0x3c,0x3c,0x38,0x00,0x00};
byte aimMinus2[8] = {0x00,0x00,0x38,0x7f,0x7f,0x38,0x00,0x00};
byte aimMinus1[8] = {0x00,0x00,0x38,0x3c,0x3c,0x1e,0x07,0x02};
byte aim0[8] = {0x00,0x18,0x3c,0x3c,0x3c,0x18,0x18,0x18};
byte aim1[8] = {0x00,0x00,0x1c,0x3c,0x3c,0x78,0xe0,0x40};
byte aim2[8] = {0x00,0x00,0x1c,0xfe,0xfe,0x1c,0x00,0x00};
byte aim3[8] = {0x40,0xe0,0x78,0x3c,0x3c,0x1c,0x00,0x00};

char incomingByte;                  // for incoming serial data from host
int  Potentiometers[] = { 0,1,2,3 };      // Set the analog pins for the potentiometers
int  Buttons[] = { 2,9,11 };       // Set the digital pins for the buttons
int  LinearPots[] = {};       // Set the digital pins for the buttons
bool DebugMode = false;              // Enable or disable the debug mode

int potentiometerThrust = 0;
int potentiometerRudder = 1;
int potentiometerAim = 2;

int  TotalPots = (sizeof(Potentiometers) / sizeof(Potentiometers[0]));   // Calculate total Potentiometers
int  TotalButs = (sizeof(Buttons) / sizeof(Buttons[0]));                // Calculate total Buttons
int  TotalLPots = (sizeof(LinearPots) / sizeof(LinearPots[0]));                // Calculate total Buttons
int  Total = TotalPots + TotalButs + TotalLPots;                                      // Calculate total inputs

int DIN = 7;            // Data In pin
int CS = 5;            // Chip Select pin
int CLK = 6;            // Clock pin
int DevMax = 4;         // Maximum LED matrixes connected

int Brightness = 8;     // Brightnes of the LED Matrix (0 - 15)

String incoming = "";   // String incoming 

LedControl lc = LedControl(DIN, CLK, CS, DevMax);    // Call the LED Control function


// ---- DEFINES ----------------------------------------------


// ---- START OF SETUP ----------------------------------------
void setup() {

    Serial.begin(115200);            // Set the serial baudrate (115200 is the typical USB v2.0 speed)

    for (int i = 0; i < TotalButs; i++) {
        pinMode(Buttons[i], INPUT_PULLUP);  // Set the pins for the buttons as input with pullup properties 
    }
    for (int i = 0; i < TotalLPots; i++) {
        pinMode(LinearPots[i], INPUT_PULLUP);  // Set the pins for the buttons as input with pullup properties 
    }

    for (int i = 0; i < DevMax; i++) {
        lc.shutdown(i, false);               // Turning the display on
        lc.setIntensity(i, Brightness);      // Adjust the brightness
        lc.clearDisplay(i);                 // Clear the display
    }

    delay(500);               // Wait for 500ms to boot up
    Serial.println("Booted");       // Print the startup message
    delay(2000);                      // Wait for 2 seconds before start streaming
}
// ---- END OF SETUP ------------------------------------------



// ---- START OF LOOP -----------------------------------------
void loop() {
    if (Serial.available() > 0) {

      incomingByte = Serial.read();     //  Read the serial input and start sending information back

//    Debug Mode    
      if (DebugMode) {
          Serial.println(incomingByte);   //  Print the incoming byte (only use for debug mode)
      }

//     If the byte is empty then execute the following function
        if (incomingByte != '\0') {
          SetThrustMatrix();
          SetRudderMatrix();
          SetAimMatrix();
          SendData();
        }
    }

  //  Debug Mode (it will continuously send data)
  if (DebugMode) {
      SendData();
  }

}
// ---- END OF LOOP -----------------------------------------


// ---- START OF FUNCTIONS ----------------------------------

//  Prints the HEX bytes on the LED matrix 
void printByte(byte character[], int index)
{
    int i = 0;

    for (i = 0; i < 8; i++)
    {
        lc.setRow(index, i, character[i]);
    }
}

void SetThrustMatrix()
{
    float thrustValue = Potentiometer(potentiometerThrust);
    if (thrustValue < 0.1f)
    {
        printByte(thrust0, 0);
    } else if (thrustValue < 0.15f)
    {
        printByte(thrust1, 0);
    }
    else if (thrustValue < 0.3f)
    {
        printByte(thrust2, 0);
    }
    else if (thrustValue < 0.4f)
    {
        printByte(thrust3, 0);
    }
    else if (thrustValue < 0.5f)
    {
        printByte(thrust4, 0);
    }
    else if (thrustValue < 0.6f)
    {
        printByte(thrust5, 0);
    }
    else if (thrustValue < 0.7f)
    {
        printByte(thrust6, 0);
    }
    else if (thrustValue < 0.8f)
    {
        printByte(thrust7, 0);
    }
    else if (thrustValue < 0.9f)
    {
        printByte(thrust8, 0);
    }
}

void SetRudderMatrix()
{
    float rudderValue = Potentiometer(potentiometerRudder);
    if (rudderValue < 0.1f)
    {
        printByte(rudderMinus4, 1);
    }
    else if (rudderValue < 0.2f)
    {
        printByte(rudderMinus3, 1);
    }
    else if (rudderValue < 0.3f)
    {
        printByte(rudderMinus2, 1);
    }
    else if (rudderValue < 0.4f)
    {
        printByte(rudderMinus1, 1);
    }
    else if (rudderValue < 0.6f)
    {
        printByte(rudder0, 1);
    }
    else if (rudderValue < 0.7f)
    {
        printByte(rudder1, 1);
    }
    else if (rudderValue < 0.8f)
    {
        printByte(rudder2, 1);
    }
    else if (rudderValue < 0.9f)
    {
        printByte(rudder3, 1);
    }
    else if (rudderValue > 0.9f)
    {
        printByte(rudder4, 1);
    }
}

void SetAimMatrix()
{
    float aimValue = Potentiometer(potentiometerAim);
    if (aimValue < 0.1f)
    {
        printByte(aimMinus3, 3);
    }
    else if (aimValue < 0.2f)
    {
        printByte(aimMinus2, 3);
    }
    else if (aimValue < 0.3f)
    {
        printByte(aimMinus1, 3);
    }
    else if (aimValue < 0.4f)
    {
        printByte(aim0, 3);
    }
    else if (aimValue  < 0.6f)
    {
        printByte(aim1, 3);
    }
    else if (aimValue < 0.7f)
    {
        printByte(aim2, 3);
    }
    else if (aimValue > 0.8f)
    {        
      printByte(aim3, 3);
    }
}

//  function Send Data
void SendData() {

    String lineToSend = "";       // Set string lineToSend

//  Print the value of each potentiometer and button followed by an ampersent (&)   
//  If statement at the end is to not add an ampersent at the last value  

//    Iterate through total inputs   
    for (int i = 0; i < Total; i++) {

        bool isPot = i < TotalPots;
        bool isBot = i < TotalPots + TotalButs;                                                                  // is input a potentiometer?

  //      Debug Mode
  //      Prints [PTMTR] or [BUTTON] to identify what is what
        if (DebugMode) {
            lineToSend += isPot ? "[PTMTR]" : isBot ? "[BUTTON]" : "[LinPOTS]";                                              // Debug just to see values
        }

        lineToSend += isPot ? Potentiometer(Potentiometers[i]) : isBot ? Button(Buttons[i - TotalPots]) : LinearPot(LinearPots[i - TotalPots - TotalButs]);       // Add Button input or Potentiometer input based on {isPot} being a potentiometer 
        lineToSend += (i == Total - 1) ? "" : "&";                                                     // Add Seperator after checking if its the last one 
    }

    Serial.println(lineToSend);                                      //  Print the line    
}

//  function Potentiometer
float Potentiometer(int value) {
    //    Read the analog pin, devide by 1023 and return the value
    int val = analogRead(value);
    return (val - 1.0f) / 1022;
}

// function Lever
float LinearPot(int value) {

    //    Read the analog pin, devide by 1023 and return the value
    float val = (540 - analogRead(value)) / (float)540;
    return val > 0 ? val : 0;
}


//  function Button
bool Button(int pin) {

    //    Read the digital pin,  and return true or false
    return digitalRead(pin) == LOW;
}

// FUNCTIONS
//----------------------


// ---- END OF FUNCTIONS ------------------------------------

/***************************************************************************************** END OF FILE **********************************************************************************************/





/* -------------------------------------------------------------------------------------------------------------------------------------------------------------
 *  DO NOT USE THIS CODE
 *  ====================
 * -------------------------------------------------------------------------------------------------------------------------------------------------------------
 *


void loop()

{
    float mySensVals[] = {2, 4, -8, 3, 2};    // create the array with random values


    mySensVals[0] = 100000;      // change the value of the array
    mySensVals[1] = 40;     // change the value of the array

    Serial.print("{");
    for(int i = 0; i < 5; i++)
    {
      Serial.print(i);
      Serial.print("=");
      Serial.print(mySensVals[i]);
      Serial.print("&");

    }
    Serial.println("}");


    Serial.println("");
    Serial.println(mySensVals);

    Serial.println("end of data");
    Serial.println(" ");
//  delay (500);


// TEST STUFF - DO NOT USE
    Serial.println( Potentiometer(0) );
    Serial.println( Potentiometer(1) );

}
*/
