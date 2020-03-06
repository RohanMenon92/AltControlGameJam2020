using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
           newText = "!!! NEW High Score: " + currentManager.GetScore() + " !!!";
        }
        else
        {
           newText = "Game Score: " + currentManager.GetScore();
        }

        gameOverScore.text = newText;

        CanvasGroup gameplayCanvas = gameplayScreen.GetComponent<CanvasGroup>();
        // Fade Out Gameplay
        DOTween.To(() => gameplayCanvas.alpha, x => gameplayCanvas.alpha = x, 0.0f, 1.0f).OnComplete(() => {
            gameplayScreen.SetActive(false);
        });

        CanvasGroup gameOverCanvas = gameOverScreen.GetComponent<CanvasGroup>();
        // Fade in GameOver
        gameOverCanvas.alpha = 0;
        gameOverScreen.SetActive(true);
        DOTween.To(() => gameOverCanvas.alpha, x => gameOverCanvas.alpha = x, 1.0f, 1.0f).OnComplete(() => {
            StartCoroutine(Restart());
        });
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(GameConstants.GameOverScreenTime);
        SceneManager.LoadScene(0);
    }
}
