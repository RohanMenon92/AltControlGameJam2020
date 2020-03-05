using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject shotgunBulletPrefab;
    public GameObject bulletPrefab;
    public GameObject laserBulletPrefab;

    public GameObject shieldHitPrefab;
    public GameObject bulletHitPrefab;
    public GameObject damageSmokePrefab;
    public GameObject explodeEffectPrefab;

    public Transform unusedBulletPool;
    public Transform unusedShotgunBulletPool;
    public Transform unusedLaserBulletPool;

    public Transform shieldEffectsPool;
    public Transform bulletHitEffectPool;
    public Transform smokeEffectPool;
    public Transform explodeEffectPool;

    public Transform worldBullets;


    // public player
    PlayerScript player;

    bool isRechargePressed;
    bool isFiring;
    bool isShielding;

    public int score = 0;      // for the title to have the high score
    public int scorePerSecond = 10;
    public int currentWave;

    private float elapsedTime = 0; // used for score bit every second

    // Start is called before the first frame update
    void Start()
    {
        // reset score
        score = 0;
        // Instantiate bullet pools in start
        for (int i = 0; i <= GameConstants.NormalBulletPoolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, unusedBulletPool);
            newBullet.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.ShotgunBulletPoolSize; i++)
        {
            GameObject newBullet = Instantiate(shotgunBulletPrefab, unusedShotgunBulletPool);
            newBullet.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.LaserBulletPoolSize; i++)
        {
            GameObject newBullet = Instantiate(laserBulletPrefab, unusedLaserBulletPool);
            newBullet.SetActive(false);
        }


        for (int i = 0; i <= GameConstants.EffectsPoolSize; i++)
        {
            GameObject newShieldhit = Instantiate(shieldHitPrefab, shieldEffectsPool);
            newShieldhit.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.EffectsPoolSize; i++)
        {
            GameObject newBulletHit = Instantiate(bulletHitPrefab, bulletHitEffectPool);
            newBulletHit.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.EffectsPoolSize; i++)
        {
            GameObject smokeEffect = Instantiate(damageSmokePrefab, smokeEffectPool);
            smokeEffect.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.EffectsPoolSize; i++)
        {
            GameObject explodeEffect = Instantiate(explodeEffectPrefab, explodeEffectPool);
            explodeEffect.SetActive(false);
        }

        player = FindObjectOfType<PlayerScript>();
    }

    public void IncrementWaves()
    {
        // Code for setting win condition from GameConstants
        currentWave++;
        //if(currentWave > GameConstants.WaveWinCondition)
        //{
        //    HasWon();
        //}
    }

    public void HasWon()
    {
        // Call Win Screen Here
    }

    public void GameOver()
    {
        // Call GameOver Screen Here
    }

    public GameObject GetBullet(GameConstants.GunTypes gunType, Transform gunPort)
    {
        GameObject bulletObject = null;
        // Get Bullet From pool and return it
        switch (gunType)
        {
            // Get First Child, set parent to gunport (to remove from respective pool)
            case GameConstants.GunTypes.MachineGun:
                bulletObject = unusedBulletPool.GetComponentInChildren<BulletScript>(true).gameObject;
                break;
            case GameConstants.GunTypes.ShotGun:
                bulletObject = unusedShotgunBulletPool.GetComponentInChildren<ShotgunBulletScript>(true).gameObject;
                break;
            case GameConstants.GunTypes.LaserGun:
                bulletObject = unusedLaserBulletPool.GetComponentInChildren<LaserBulletScript>(true).gameObject;
                break;
        }

        bulletObject.transform.SetParent(worldBullets);
        bulletObject.transform.position = gunPort.transform.position;
        // Return bullet and let GunPort handle how to fire and set initial velocities
        return bulletObject;
    }

    // Returning Normal Bullet to pool
    public void ReturnBulletToPool(GameObject bulletToStore, GameConstants.GunTypes bulletType)
    {
        if(bulletType == GameConstants.GunTypes.MachineGun)
        {
            // Return to normal bullet pool
            bulletToStore.transform.SetParent(unusedBulletPool);
        }
        else if (bulletType == GameConstants.GunTypes.ShotGun)
        {
            // Return to shotgun bullet pool
            bulletToStore.transform.SetParent(unusedShotgunBulletPool);
        }
        else if (bulletType == GameConstants.GunTypes.LaserGun)
        {
            // Return to laser bullet pool
            bulletToStore.transform.SetParent(unusedLaserBulletPool);
        }
        bulletToStore.gameObject.SetActive(false);
        bulletToStore.transform.localScale = Vector3.one;
        bulletToStore.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            score += scorePerSecond;
            elapsedTime = 0;
        }
    }

    internal void UpdateThrustInput(float val)
    {
        player.currThrust = val;
    }

    internal void UpdateRudderAngle(float val)
    {
        player.currRudderAngle = val - 0.5f;
    }

    internal void UpdateAimAngle(float val)
    {
        player.currAimAngle = val - 0.5f;
    }

    internal void UpdatePreciseAimAngle(float val)
    {
        player.currPreciseAimAngle = val - 0.5f;
    }

    internal void UpdateRechargeButton(bool val)
    {
        if (isRechargePressed != val)
        {
            if (!isRechargePressed)
            {
                OnRechargeActivate();
            }
        }
        isRechargePressed = val;
    }

    private void OnRechargeActivate()
    {
        player.OnRecharge();
    }

    internal void UpdateFireButton(bool val)
    {
        if (isFiring != val)
        {
            if(!isFiring)
            {
                OnFireActivate();
            }
        }
        isFiring = val;
    }

    private void OnFireActivate()
    {
        player.OnFire();
    }

    internal void UpdateShieldButton(bool val)
    {
        if (isShielding != val)
        {
            if (!isShielding)
            {
                OnShieldActivate();
            } else
            {
                OnShieldDeactivate();
            }
        }
        isShielding = val;
    }

    private void OnShieldDeactivate()
    {
        player.ShieldOff();
    }

    private void OnShieldActivate()
    {
        player.ShieldOn();
    }
}
