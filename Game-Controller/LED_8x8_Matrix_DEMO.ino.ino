/*****************************************************************************
 * 
 * Title:   LED 8x8 Matrix Display 
 * Authors: Marshall Kaspari, Rohan Menon
 * 
 * Usage:   It accepts a hex string via serial and prints it on the LED matrix.
 *          Copy and paste the following example intn the serial monitoring.
 *     
 * Example: 0xAA,0x55,0xAA,0x55,0xAA,0x55,0xAA,0x55&0x3C,0x42,0xA5,0x81,0xBD,0x81,0x42,0x3C&0x18,0x18,0x18,0x18,0xFF,0x7E,0x3C,0x18
 * 
 * 
******************************************************************************/

#include <LedControl.h>
int DIN = 7;            // Data In pin
int CS =  5;            // Chip Select pin
int CLK = 6;            // Clock pin
int DevMax = 4;         // Maximum LED matrixes connected

int Brightness = 8;     // Brightnes of the LED Matrix (0 - 15)
int incomingByte = 0;   // For incoming serial data
String incoming = "";   // String incoming 

LedControl lc=LedControl(DIN,CLK,CS,DevMax);    // Call the LED Control function

void setup(){
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps

  for(int i = 0; i < DevMax-1; i++) {
    lc.shutdown(i,false);               // Turning the display on
    lc.setIntensity(i,Brightness);      // Adjust the brightness
    lc.clearDisplay(i);                 // Clear the display
  }
}


void loop() {
  // send data only when you receive data:
  if (Serial.available() > 0) {
    // read the incoming:
    incoming = Serial.readString();
    // Trim it
    incoming = incoming.trim();
    Serial.println(incoming);  

    incoming = incoming.replace("0x", "");
    incoming = incoming.replace(",", "");


    char* command = strtok(incoming.c_str(), "&");
    int index = 0;
    while (command != 0)
      {
          byte byte1[8] ;
          hexCharacterStringToBytes(byte1, command);
      
          // Split the command in two values
          printByte(byte1,index);
          
          index++;
          
          // Find the next command in input string
          command = strtok(0, "&");
      }    
        Serial.println(" ");
        Serial.flush();
    } 


// EXAMPLES
//----------------------
/*
    //Facial Expression
    byte smile[8]=   {0x3C,0x42,0xA5,0x81,0xA5,0x99,0x42,0x3C};
    byte neutral[8]= {0x3C,0x42,0xA5,0x81,0xBD,0x81,0x42,0x3C};
    byte sad[8]=   {0x3C,0x42,0xA5,0x81,0x99,0xA5,0x42,0x3C};

    //Arrow
    byte arrow_up[8]= {0x18,0x3C,0x7E,0xFF,0x18,0x18,0x18,0x18};
    byte arrow_down[8]= {0x18,0x18,0x18,0x18,0xFF,0x7E,0x3C,0x18};
   
    //Alternate Pattern
    byte d1[8]= {0xAA,0x55,0xAA,0x55,0xAA,0x55,0xAA,0x55};
    byte d2[8]= {0x55,0xAA,0x55,0xAA,0x55,0xAA,0x55,0xAA};

    //Moving car
    byte b1[8]= {0x00,0x00,0x00,0x00,0x18,0x3C,0x18,0x3C};
    byte b2[8]= {0x00,0x00,0x00,0x18,0x3C,0x18,0x3C,0x00};
    byte b3[8]= {0x00,0x00,0x18,0x3C,0x18,0x3C,0x00,0x00};
    byte b4[8]= {0x00,0x18,0x3C,0x18,0x3C,0x00,0x00,0x00};
    byte b5[8]= {0x18,0x3C,0x18,0x3C,0x00,0x00,0x00,0x00};
    byte b6[8]= {0x3C,0x18,0x3C,0x00,0x00,0x00,0x00,0x18};
    byte b7[8]= {0x18,0x3C,0x00,0x00,0x00,0x00,0x18,0x3C};
    byte b8[8]= {0x3C,0x00,0x00,0x00,0x00,0x18,0x3C,0x18};
    

//Random Stuff
    byte rs1[8]=   {0xFF,0x81,0xBD,0x99,0x99,0xBD,0x81,0xFF};
    byte rs2[8]=   {0xFF,0x91,0x89,0xDD,0xBB,0x91,0x89,0xFF};
    byte rs3[8]=   {0xFF,0x81,0xA5,0xBD,0xBD,0xA5,0x81,0xFF};    
    byte rs4[8]=   {0xFF,0x89,0x91,0xBB,0xDD,0x89,0x91,0xFF};


//Moving symbol
    printByte(rs1,0);
    delay(1000);
    printByte(rs2,0);
    delay(1000);
    printByte(rs3,0);
    delay(1000);
    printByte(rs4,0);
    delay(1000);

/*
//Moving car
    printByte(b1);
    delay(50);
    printByte(b2);
    delay(50);
    printByte(b3);
    delay(50);
    printByte(b4);
    delay(50);
    printByte(b5);
    delay(50);
    printByte(b6);
    delay(50);
    printByte(b7);
    delay(50);
    printByte(b8);
    delay(50);

//alternate pattern
    printByte(d1);
    delay(100);

    printByte(d2);
    delay(100);

//Arrow
    printByte(arrow_up);
    delay(2000);

    printByte(arrow_down);
    delay(2000);

   
//Facial Expression   

    printByte(smile);
    delay(1000);

    printByte(neutral);
    delay(1000);

    printByte(sad);    
    delay(1000);
   
//*/ 
}


// FUNCTIONS
//----------------------

//  Prints the HEX bytes on the LED matrix 
    void printByte(byte character [], int index)
    {
      int i = 0;
      
        for(i=0;i<8;i++)
        {
            lc.setRow(index,i,character[i]);
        }
    }


//  Converts a HEX string to a BYTES array
    void hexCharacterStringToBytes(byte *byteArray, const char *hexString)
    {
      bool oddLength = strlen(hexString) & 1;
    
      byte currentByte = 0;
      byte byteIndex = 0;
    
      for (byte charIndex = 0; charIndex < strlen(hexString); charIndex++)
      {
        bool oddCharIndex = charIndex & 1;
    
        if (oddLength)
        {
          // If the length is odd
          if (oddCharIndex)
          {
            // odd characters go in high nibble
            currentByte = nibble(hexString[charIndex]) << 4;
          }
          else
          {
            // Even characters go into low nibble
            currentByte |= nibble(hexString[charIndex]);
            byteArray[byteIndex++] = currentByte;
            currentByte = 0;
          }
        }
        else
        {
          // If the length is even
          if (!oddCharIndex)
          {
            // Odd characters go into the high nibble
            currentByte = nibble(hexString[charIndex]) << 4;
          }
          else
          {
            // Odd characters go into low nibble
            currentByte |= nibble(hexString[charIndex]);
            byteArray[byteIndex++] = currentByte;
            currentByte = 0;
          }
        }
      }
    }

//  Confirms whether the character is HEX or not
    byte nibble(char c)
    {
      if (c >= '0' && c <= '9')
        return c - '0';
    
      if (c >= 'a' && c <= 'f')
        return c - 'a' + 10;
    
      if (c >= 'A' && c <= 'F')
        return c - 'A' + 10;
    
      return 0;  // Not a valid hexadecimal character
    }
