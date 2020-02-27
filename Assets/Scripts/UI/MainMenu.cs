using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainPanel;
    public Image[] helpScreens = new Image[5];
    public Text highScore;
    public int currentHelpScreen = -1;

    void Start()
    {
        string newText = "High Score: " + PlayerPrefs.GetInt(GameConstants.HighScorePlayerPref).ToString();
        highScore.text = newText;
        
           
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        ChangeHelpScreen(1);
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        currentHelpScreen = -1;
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ChangeHelpScreen(int direction)
    {
        foreach (Image screen in helpScreens)
            screen.enabled = false;
        currentHelpScreen += direction;

        if (currentHelpScreen < 0 || currentHelpScreen > 4) { CloseSettings(); }
        else
        {
            helpScreens[currentHelpScreen].enabled = true;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game.");
        Application.Quit();
    }
}
