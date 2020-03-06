/* ALT GAME JAM CONTROLLER
 *  
 *  Project:  Alt Game Jam 2020
 *  Author:   Marshall Kaspari
 *  Type:     Analog/Digital controller
 *  Board:    Teensyduino
 *    
*/

// ---- DEFINES ----------------------------------------------
    char incomingByte;                  // for incoming serial data from host
    int  Potentiometers[] = {0,1,2,3};      // Set the analog pins for the potentiometers
    int  Buttons[] = {2,9,11};       // Set the digital pins for the buttons
    int  LinearPots[] = {};       // Set the digital pins for the buttons
	  bool DebugMode = false;              // Enable or disable the debug mode

    int  TotalPots = (sizeof(Potentiometers) / sizeof(Potentiometers[0]));   // Calculate total Potentiometers
    int  TotalButs = (sizeof(Buttons) / sizeof(Buttons[0]));                // Calculate total Buttons
    int  TotalLPots = (sizeof(LinearPots) / sizeof(LinearPots[0]));                // Calculate total Buttons
    int  Total = TotalPots + TotalButs + TotalLPots;                               	     // Calculate total inputs
	

// ---- DEFINES ----------------------------------------------


// ---- START OF SETUP ----------------------------------------
void setup()	{     
           
	  Serial.begin(115200);      			 // Set the serial baudrate (115200 is the typical USB v2.0 speed)
    
    for (int i=0; i<TotalButs; i++) {
      pinMode(Buttons[i], INPUT_PULLUP);  // Set the pins for the buttons as input with pullup properties 
    }
    for (int i=0; i<TotalLPots; i++) {
      pinMode(LinearPots[i], INPUT_PULLUP);  // Set the pins for the buttons as input with pullup properties 
    }
    delay(500);								// Wait for 500ms to boot up
    Serial.println("Booted");				// Print the startup message
    delay(2000);              				// Wait for 2 seconds before start streaming
	}
// ---- END OF SETUP ------------------------------------------



// ---- START OF LOOP -----------------------------------------
void loop() {

	if(Serial.available() > 0) {

		incomingByte = Serial.read();     //  Read the serial input and start sending information back

//		Debug Mode		
    	if (DebugMode) {
    		Serial.println(incomingByte);   //  Print the incoming byte (only use for debug mode)
    	}  

//  	If the byte is empty then execute the following function
		if (incomingByte != '\0') {

			SendData();
		}
	}
	
//	Debug Mode (it will continuously send data)
	if (DebugMode) {
		SendData();
	}

}
// ---- END OF LOOP -----------------------------------------


// ---- START OF FUNCTIONS ----------------------------------

//	function Send Data
	void SendData() {

	String lineToSend = "";       // Set string lineToSend
	
//  Print the value of each potentiometer and button followed by an ampersent (&)   
//  If statement at the end is to not add an ampersent at the last value  

// 		Iterate through total inputs   
		for(int i = 0; i< Total; i++) {
			
      bool isPot = i < TotalPots;                
      bool isBot = i < TotalPots + TotalButs;                                                                  // is input a potentiometer?
			
//			Debug Mode
//			Prints [PTMTR] or [BUTTON] to identify what is what
			if (DebugMode) {
				lineToSend += isPot ? "[PTMTR]" :  isBot ? "[BUTTON]" : "[LinPOTS]" ;                                              // Debug just to see values
			}

			lineToSend += isPot ? Potentiometer(Potentiometers[i]) : isBot ? Button(Buttons[i-TotalPots]) : LinearPot(LinearPots[i- TotalPots - TotalButs]);       // Add Button input or Potentiometer input based on {isPot} being a potentiometer 
			lineToSend += (i == Total - 1) ? "":"&";                                                     // Add Seperator after checking if its the last one 
		} 
		
		Serial.println(lineToSend);   																	 //  Print the line    
	}


//  function Potentiometer
  float Potentiometer(int value) {
    
//    Read the analog pin, devide by 1023 and return the value
    int val = analogRead(value);
    return (val - 1.0f)/1022;
  }

// function Lever
  float LinearPot(int value) {
    
//    Read the analog pin, devide by 1023 and return the value
    float val = (540 - analogRead(value))/(float)540;
    return val > 0 ? val : 0;
  }


//	function Button
	bool Button(int pin) {

// 		Read the digital pin,  and return true or false
		return digitalRead(pin) == LOW;
	}  
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
//	delay (500);


// TEST STUFF - DO NOT USE
	Serial.println( Potentiometer(0) );
	Serial.println( Potentiometer(1) );

}
*/
