using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPort : MonoBehaviour
{
    public GameConstants.GunTypes gunType = GameConstants.GunTypes.MachineGun;
    // Start is called before the first frame update

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(bool isEnemy)
    {
        GameObject newBullet = gameManager.GetBullet(gunType, transform);
        switch(gunType) {
            case GameConstants.GunTypes.MachineGun:
                // Get MachineGun Bullet from GameManager MachineGunBulletPool 
                // Set transform and set initial velocity of rigidbody component
                break;
            case GameConstants.GunTypes.ShotGun:
                // Get 5-10 ShotGun Bullets from GameManager.ShotGunBulletPool
                // Give random spread to rotation/initial position
                // Set Initial Velocity of rigidbody
                break;
            case GameConstants.GunTypes.LaserGun:
                // Create Laser Bullet Prefab and move to position/scale
                // Enable Laser Object Collider and do damage
                // Laser Bullet Automatically dissapears and returns to pool
                break;
        }
    }
}
