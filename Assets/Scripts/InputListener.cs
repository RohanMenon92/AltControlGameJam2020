using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class InputListener : MonoBehaviour
{
    public string comPort;
    public int baudRate;
    public int timeout;
    public float pingPerMilliseconds;
    public GameManager gameManager;

    SerialPort stream;
    // Start is called before the first frame update
    void Start()
    {
        stream = new SerialPort(comPort, baudRate);
        stream.ReadTimeout = 10000;
        stream.Open();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(Time.realtimeSinceStartup % pingPerMilliseconds == 0)
        {
            Debug.Log("Called");
            WriteToArduino();
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

    void OnGetValue(GameConstants.ProcessSignals signal, float val)
    {
        Debug.Log(signal.ToString() + " " + val);
        switch (signal)
        {
            case GameConstants.ProcessSignals.P1:
                gameManager.UpdateThrustInput(val);
                // Call control 1 with val
                break;

            case GameConstants.ProcessSignals.P2:
                gameManager.UpdateRudderAngle(val);
                // Call control 1 with val
                break;

            case GameConstants.ProcessSignals.P3:
                gameManager.UpdateShieldAngle(val);
                // Call control 1 with val
                break;

            case GameConstants.ProcessSignals.B1:
                gameManager.UpdateRechargeButton(val == 1.0f);
                // Call control 1 with val
                break;

            case GameConstants.ProcessSignals.B2:
                gameManager.UpdateFireButton(val == 1.0f);
                // Call control 1 with val
                break;

            case GameConstants.ProcessSignals.B3:
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
                OnGetValue((GameConstants.ProcessSignals)i, float.Parse(val));
                i++;
            }
        }

        var controlValues = Enum.GetValues(typeof(GameConstants.ProcessSignals));
    }

    public void WriteToArduino()
    {
        stream.Write("a");
        stream.BaseStream.Flush();
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
