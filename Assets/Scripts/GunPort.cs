using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPort : MonoBehaviour
{
    public GameConstants.GunTypes gunType = GameConstants.GunTypes.MachineGun;
    // Start is called before the first frame update

    GameManager gameManager;

    public float firePerSeconds;
    bool canFire = true;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(bool isEnemy, Transform firedShip)
    {
        // Check if it can fire
        if(canFire)
        {
            switch (gunType)
            {
                case GameConstants.GunTypes.MachineGun:
                    BulletScript newBullet = gameManager.GetBullet(gunType, transform).GetComponent<BulletScript>();
                    newBullet.transform.position = transform.position + transform.forward;
                    newBullet.transform.rotation = transform.rotation;
                    newBullet.isEnemyShot = isEnemy;
                    newBullet.GetComponent<Rigidbody>().velocity = firedShip.GetComponent<Rigidbody>().velocity;
                    newBullet.gameObject.SetActive(true);
                    newBullet.FireBullet();
                    // Set transform and set initial velocity of rigidbody component
                    break;
                case GameConstants.GunTypes.ShotGun:
                    // Do it for 5 shots
                    for(int i = 0; i < 6; i++)
                    {
                        ShotgunBulletScript newShoutgunBullet = gameManager.GetBullet(gunType, transform).GetComponent<ShotgunBulletScript>();
                        newShoutgunBullet.transform.position = transform.position + transform.forward;
                        newShoutgunBullet.transform.eulerAngles =  new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Random.Range(-10f, 10f), transform.eulerAngles.z);
                        newShoutgunBullet.isEnemyShot = isEnemy;
                        // Set bullet velocity to ship velocity
                        newShoutgunBullet.GetComponent<Rigidbody>().velocity = firedShip.GetComponent<Rigidbody>().velocity;
                        newShoutgunBullet.gameObject.SetActive(true);
                        newShoutgunBullet.FireShotgun();
                    }
                    // Get 5-10 ShotGun Bullets from GameManager.ShotGunBulletPool
                    // Give random spread to rotation/initial position
                    // Set Initial Velocity of rigidbody
                    break;
                case GameConstants.GunTypes.LaserGun:
                    LaserBulletScript newLaserBullet = gameManager.GetBullet(gunType, transform).GetComponent<LaserBulletScript>();
                    newLaserBullet.transform.position = transform.position + transform.forward;
                    newLaserBullet.transform.SetParent(transform);

                    float randomY = transform.eulerAngles.y + (isEnemy ? Random.Range(-5f, 5f) : 0f);
                    newLaserBullet.transform.eulerAngles = new Vector3(transform.eulerAngles.x, randomY, transform.eulerAngles.z);
                    newLaserBullet.isEnemyShot = isEnemy;

                    newLaserBullet.gameObject.SetActive(true);
                    newLaserBullet.FireLaser();

                    break;
            }
            // Cannot fire and start Coroutine to be able to fire next
            canFire = false;
            StartCoroutine(ResetCanFire());
        }
    }

    IEnumerator ResetCanFire()
    {
        // Wait for firePerSeconds and the say canFire is true 
        yield return new WaitForSeconds(firePerSeconds);
        canFire = true;
    }
}
