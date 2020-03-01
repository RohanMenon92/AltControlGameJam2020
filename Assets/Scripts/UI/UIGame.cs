using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    PlayerScript player;
    public Image healthImage;
    public Image energyImage;
    public Text scoreBoard;
    private GameManager currentManager;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        currentManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        string newText = "Score: " + currentManager.score;
        scoreBoard.text = newText;
        healthImage.fillAmount = player.health / GameConstants.maxHealth;
        energyImage.fillAmount = player.energy / GameConstants.maxEnergy;
    }
}
