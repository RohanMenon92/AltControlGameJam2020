using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    PlayerScript player;
    public Image healthImage;
    public Image energyImage;
    public Text scoreBoard;
    private GameManager currentManager;
    public GameObject gameplayScreen;
    public GameObject gameOverScreen;
    public Text gameOverScore;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        currentManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        string newText = "Score: " + currentManager.GetScore();
        scoreBoard.text = newText;
        healthImage.fillAmount = player.health / GameConstants.maxHealth;
        energyImage.fillAmount = player.energy / GameConstants.maxEnergy;
    }
    public void gameOver(bool newHighScore) {
        string newText;
        if (newHighScore)
        {
           newText = "!!! NEW High Score: " + currentManager.score + " !!!";
        }
        else
        {
           newText = "Game Score: " + currentManager.score;
        }

        gameOverScore.text = newText;
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void restart() {
        SceneManager.LoadScene(0);
    }
}
