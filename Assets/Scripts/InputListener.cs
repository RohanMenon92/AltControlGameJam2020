using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class InputListener : MonoBehaviour
{
    // Matrix Outputs
    string thrust0 = "0x00,0x00,0x00,0x00,0x70,0x20,0x20,0x00";
    string thrust1 = "0x80,0x80,0x80,0x00,0x70,0x20,0x20,0x00";
    string thrust2 = "0xC0,0xC0,0xC0,0x00,0x70,0x20,0x20,0x00";
    string thrust3 = "0xE0,0xE0,0xE0,0x00,0x70,0x20,0x20,0x00";
    string thrust4 = "0xF0,0xF0,0xF0,0x00,0x70,0x20,0x20,0x00";
    string thrust5 = "0xF8,0xF8,0xF8,0x00,0x70,0x20,0x20,0x00";
    string thrust6 = "0xFC,0xFC,0xFC,0x00,0x70,0x20,0x20,0x00";
    string thrust7 = "0xFC,0xFC,0xFC,0x00,0x70,0x20,0x20,0x00";
    string thrust8 = "0xFF,0xFF,0xFF,0x00,0x70,0x20,0x20,0x00";

    string rudderM4 = "0xF8,0xF8,0x00,0x1E,0x3C,0x78,0x3C,0x1E";
    string rudderM3 = "0x78,0x38,0x00,0x1E,0x2C,0x58,0x2C,0x1E";
    string rudderM2 = "0x38,0x18,0x00,0x1E,0x24,0x48,0x24,0x1E";
    string rudderM1 = "0x18,0x08,0x00,0x12,0x24,0x48,0x24,0x12";
    string rudder0 = "0x18,0x18,0x00,0x24,0x24,0x24,0x24,0x24";
    string rudder1 = "0x18,0x10,0x00,0x48,0x24,0x12,0x24,0x48";
    string rudder2 = "0x1C,0x18,0x00,0x78,0x24,0x12,0x24,0x78";
    string rudder3 = "0x1E,0x1C,0x00,0x78,0x34,0x1A,0x34,0x78";
    string rudder4 = "0x1F,0x1F,0x00,0x78,0x3C,0x1E,0x3C,0x78";

    string aimM3 = "0x00,0x10,0x3C,0x1C,0x3E,0x74,0xE0,0x40";
    string aimM2 = "0x00,0x00,0x18,0xFC,0xFC,0x18,0x00,0x00";
    string aimM1 = "0x40,0xE0,0x74,0x3E,0x1C,0x3C,0x10,0x00";
    string aim0 = "0x18,0x18,0x18,0x18,0x3C,0x18,0x00,0x00";
    string aim1 = "0x02,0x07,0x0E,0x3C,0x38,0x18,0x00,0x00";
    string aim2 = "0x00,0x00,0x18,0x3F,0x3F,0x18,0x00,0x00";
    string aim3 = "0x18,0x18,0x18,0x18,0x3C,0x18,0x00,0x00";


    // End Matrix Outputs

    public string comPort;
    public int baudRate;
    public int timeout;
    public float pingPerFrames;

    GameManager gameManager;
    PlayerScript playerScript;
    SerialPort stream;

    public bool hasError = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerScript = FindObjectOfType<PlayerScript>();
        stream = new SerialPort(comPort, baudRate);
        stream.ReadTimeout = 10000;
        try
        {
            stream.Open();
        }
        catch (Exception)
        {
            hasError = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % pingPerFrames == 0 && !hasError)
        {
            // Ask for data
            RequestArduino();
            // Wait for recieveing data asynchronously
            StartCoroutine
            (
                AsynchronousReadFromArduino
                ((string s) => ProcessInput(s),     // Callback
                    () => Debug.LogError("Error!"),
                    timeout // Timeout (milliseconds)
                )
            );
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(Time.realtimeSinceStartup % pingPerMilliseconds);
        //if(Time.realtimeSinceStartup % pingPerMilliseconds == 0)
        //{
        //    Debug.Log("WriteToArduino");

        //}
    }

    void OnGetValue(GameConstants.InputSignals signal, float val)
    {
        //Debug.Log(signal.ToString() + " " + val);
        switch (signal)
        {
            case GameConstants.InputSignals.P1:
                gameManager.UpdateThrustInput(val);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.P2:
                gameManager.UpdateRudderAngle(val);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.P3:
                gameManager.UpdateAimAngle(val);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.P4:
                gameManager.UpdatePreciseAimAngle(val);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.B1:
                gameManager.UpdateRechargeButton(val == 1.0f);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.B2:
                gameManager.UpdateFireButton(val == 1.0f);
                // Call control 1 with val
                break;

            case GameConstants.InputSignals.B3:
                gameManager.UpdateShieldButton(val == 1.0f);
                // Call control 1 with val
                break;

                //case ProcessSignals.C3:
                //    Debug.Log(signal.ToString() + " " + val);
                //    // Call control 1 with val
                //    break;

                //case ProcessSignals.C4:
                //    Debug.Log(signal.ToString() + " " + val);
                //    // Call control 1 with val
                //    break;

                //case ProcessSignals.C5:
                //    Debug.Log(signal.ToString() + " " + val);
                //    // Call control 1 with val
                //    break;

        }
    }

    public void ProcessInput(string input)
    {
        string[] sentValues = input.Split('&');
        int i = 0;
        foreach (string val in sentValues)
        {
            if(val != "")
            {
                OnGetValue((GameConstants.InputSignals)i, float.Parse(val));
                i++;
            }
        }

        var controlValues = Enum.GetValues(typeof(GameConstants.InputSignals));
    }

    // Can be made a lot better by generating out of hexadecimal rather than strings
    string CreateThrustMatrix()
    {
        string thrustMatrix = thrust0;
        if (playerScript.currThrust < 0.1f)
        {
            thrustMatrix = thrust0;
        } else if (playerScript.currThrust < 0.15f)
        {
            thrustMatrix = thrust1;
        }
        else if (playerScript.currThrust < 0.3f)
        {
            thrustMatrix = thrust2;
        }
        else if (playerScript.currThrust < 0.4f)
        {
            thrustMatrix = thrust3;
        }
        else if (playerScript.currThrust < 0.5f)
        {
            thrustMatrix = thrust4;
        }
        else if (playerScript.currThrust < 0.6f)
        {
            thrustMatrix = thrust5;
        }
        else if (playerScript.currThrust < 0.7f)
        {
            thrustMatrix = thrust6;
        }
        else if (playerScript.currThrust < 0.8f)
        {
            thrustMatrix = thrust7;
        }
        else if (playerScript.currThrust < 1.0f)
        {
            thrustMatrix = thrust8;
        }
        return thrustMatrix;
    }

    string CreateRudderMatrix()
    {
        string rudderMatrix = rudder0;
        if (playerScript.currRudderAngle < -0.4f)
        {
            rudderMatrix = rudderM4;
        }
        else if (playerScript.currRudderAngle < -0.3f)
        {
            rudderMatrix = rudderM3;
        }
        else if (playerScript.currRudderAngle < -0.2f)
        {
            rudderMatrix = rudderM2;
        }
        else if (playerScript.currRudderAngle < -0.1f)
        {
            rudderMatrix = rudderM1;
        }
        else if (playerScript.currRudderAngle > -0.1f && playerScript.currRudderAngle < 0.1f)
        {
            rudderMatrix = rudder0;
        }
        else if (playerScript.currRudderAngle > 0.1f)
        {
            rudderMatrix = rudder1;
        }
        else if (playerScript.currRudderAngle > 0.2f)
        {
            rudderMatrix = rudder2;
        }
        else if (playerScript.currRudderAngle > 0.3f)
        {
            rudderMatrix = rudder3;
        }
        else if (playerScript.currRudderAngle > 0.4f)
        {
            rudderMatrix = rudder4;
        }
        return rudderMatrix;
    }

    string CreateAimMatrix()
    {
        string aimMatrix = aim0;

        if (playerScript.currAimAngle < -0.3f)
        {
            aimMatrix = aimM3;
        }
        else if (playerScript.currAimAngle < -0.2f)
        {
            aimMatrix = aimM2;
        }
        else if (playerScript.currAimAngle < -0.1f)
        {
            aimMatrix = aimM1;
        }
        else if (playerScript.currAimAngle > -0.1f && playerScript.currAimAngle < 0.1f)
        {
            aimMatrix = aim0;
        }
        else if (playerScript.currAimAngle > 0.1f)
        {
            aimMatrix = aim1;
        }
        else if (playerScript.currAimAngle > 0.2f)
        {
            aimMatrix = aim2;
        }
        else if (playerScript.currAimAngle > 0.3f)
        {
            aimMatrix = aim3;
        }
        return aimMatrix;
    }

    string CreateMessageToSend()
    {
        string messageToSend = "";
        messageToSend += CreateRudderMatrix();
        messageToSend += "&";
        messageToSend += CreateThrustMatrix();
        messageToSend += "&";
        messageToSend += CreateAimMatrix();

        return messageToSend;
    }

    public void RequestArduino()
    {
        try
        {
            stream.Write(CreateMessageToSend());
            stream.BaseStream.Flush();
        }
        catch (Exception)
        {
            hasError = true;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;
        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }
}
