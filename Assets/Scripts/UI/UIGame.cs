using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    PlayerScript player;
    public Image healthImage;
    public Image energyImage;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        healthImage.fillAmount = player.health / GameConstants.maxHealth;
        energyImage.fillAmount = player.energy / GameConstants.maxEnergy;
    }
}
