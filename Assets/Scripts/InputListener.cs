using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class InputListener : MonoBehaviour
{
    // End Matrix Outputs

    public string comPort;
    public int baudRate;
    public int timeout;
    public float pingPerFrames;

    public enum InputListenerType {
        MainMenu,
        GamePlay
    }; 

    public InputListenerType inputType;

    GameManager gameManager;

    MainMenu mainMenu;
    PlayerScript playerScript;
    SerialPort stream;

    public bool hasError = false;
    // Start is called before the first frame update
    void Start()
    {
        if(inputType == InputListenerType.GamePlay)
        {
            gameManager = FindObjectOfType<GameManager>();
        } else if(inputType == InputListenerType.MainMenu)
        {
            mainMenu = FindObjectOfType<MainMenu>();
        }
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
    }

    void OnGetValue(GameConstants.InputSignals signal, float val)
    {
        //Debug.Log(signal.ToString() + " " + val);
        if (inputType == InputListenerType.GamePlay)
        {
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
            }
        }
        else if (inputType == InputListenerType.MainMenu)
        {
            switch (signal)
            {
                case GameConstants.InputSignals.B1:
                    mainMenu.StartGame();
                    // Call control 1 with val
                    break;
                case GameConstants.InputSignals.B2:
                    mainMenu.OpenSettings();
                    // Call control 1 with val
                    break;
                case GameConstants.InputSignals.B3:
                    //mainMenu.ExitGame();
                    // Call control 1 with val
                    break;
            }
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

    public void RequestArduino()
    {
        try
        {
            stream.Write("a");
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
