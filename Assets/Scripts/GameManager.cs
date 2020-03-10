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

    public Transform shieldHitEffectsPool;
    public Transform bulletHitEffectsPool;
    public Transform smokeEffectsPool;
    public Transform explodeEffectsPool;

    public Transform worldBullets;
    public Transform worldEffects;


    // public player
    PlayerScript player;

    bool isRechargePressed;
    bool isFiring;
    bool isShielding;
   

    int score = 0;      // for the title to have the high score
    public int scorePerSecond = 10;
    public int currentWave;
    public bool hasFinished = false;

    private float elapsedTime = 0; // used for score bit every second

    public void IncrementScore(int scoreVal)
    {
        if(!hasFinished)
        {
            score += scoreVal;
        }
    }

    public int GetScore()
    {
        return score;
    }

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


        for (int i = 0; i <= GameConstants.ShieldEffectsPoolSize; i++)
        {
            GameObject newShieldhit = Instantiate(shieldHitPrefab, shieldHitEffectsPool);
            newShieldhit.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.BulletEffectsPoolSize; i++)
        {
            GameObject newBulletHit = Instantiate(bulletHitPrefab, bulletHitEffectsPool);
            newBulletHit.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.DamageSmokePoolSize; i++)
        {
            GameObject smokeEffect = Instantiate(damageSmokePrefab, smokeEffectsPool);
            smokeEffect.SetActive(false);
        }
        for (int i = 0; i <= GameConstants.ExplosionPoolSize; i++)
        {
            GameObject explodeEffect = Instantiate(explodeEffectPrefab, explodeEffectsPool);
            explodeEffect.SetActive(false);
        }

        player = FindObjectOfType<PlayerScript>();
    }

    public void IncrementWaves()
    {
        currentWave++;
        // Code for setting win condition from GameConstants
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
        hasFinished = true;
        if(PlayerPrefs.GetInt(GameConstants.HighScorePlayerPref) < score)
        {
            PlayerPrefs.SetInt(GameConstants.HighScorePlayerPref, score);
            FindObjectOfType<UIGame>().gameOver(true);
            return;
        }
        FindObjectOfType<UIGame>().gameOver(false);
        return;
    }
    
    
    public GameObject BeginEffect(GameConstants.EffectTypes effectType, Vector3 position, Vector3 lookAt)
    {
        GameObject effectObject = null;

        switch (effectType)
        {
            // Get First Child, set parent to gunport (to remove from respective pool)
            case GameConstants.EffectTypes.BulletHit:
                effectObject = bulletHitEffectsPool.GetComponentInChildren<BulletHitEffect>(true).gameObject;
                break;
            case GameConstants.EffectTypes.ShieldHit:
                effectObject = shieldHitEffectsPool.GetComponentInChildren<ShieldHitEffect>(true).gameObject;
                break;
            case GameConstants.EffectTypes.SmokeEffect:
                effectObject = smokeEffectsPool.GetComponentInChildren<SmokeEffect>(true).gameObject;
                break;
            case GameConstants.EffectTypes.ShipExplosion:
                effectObject = explodeEffectsPool.GetComponentInChildren<ExplosionEffect>(true).gameObject;
                break;
        }

        //bulletObject.transform.SetParent(worldBullets);
        //bulletObject.transform.position = gunPort.transform.position;
        //// Return bullet and let GunPort handle how to fire and set initial velocities
        //return bulletObject;
        effectObject.transform.SetParent(worldEffects);
        effectObject.transform.position = new Vector3(position.x, position.y, position.z);
        effectObject.transform.up = new Vector3(lookAt.x, lookAt.y, lookAt.z);

        effectObject.SetActive(true);

        switch (effectType)
        {
            // Get First Child, set parent to gunport (to remove from respective pool)
            case GameConstants.EffectTypes.BulletHit:
                effectObject.GetComponent<BulletHitEffect>().FadeIn();
                break;
            case GameConstants.EffectTypes.ShieldHit:
                effectObject.GetComponent<ShieldHitEffect>().FadeIn();
                break;
            case GameConstants.EffectTypes.SmokeEffect:
                effectObject.GetComponent<SmokeEffect>().FadeIn();
                break;
            case GameConstants.EffectTypes.ShipExplosion:
                effectObject.GetComponent<ExplosionEffect>().FadeIn();
                break;
        }

        return effectObject;
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
        if (bulletType == GameConstants.GunTypes.MachineGun)
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

    public void ReturnEffectToPool(GameObject effectToStore, GameConstants.EffectTypes effectType)
    {
        if (effectType == GameConstants.EffectTypes.BulletHit)
        {
            // Return to normal bullet pool
            effectToStore.transform.SetParent(bulletHitEffectsPool);
        }
        else if (effectType == GameConstants.EffectTypes.ShieldHit)
        {
            // Return to shotgun bullet pool
            effectToStore.transform.SetParent(shieldHitEffectsPool);
        }
        else if (effectType == GameConstants.EffectTypes.ShipExplosion)
        {
            // Return to laser bullet pool
            effectToStore.transform.SetParent(explodeEffectsPool);
        }
        else if (effectType == GameConstants.EffectTypes.SmokeEffect)
        {
            // Return to laser bullet pool
            effectToStore.transform.SetParent(smokeEffectsPool);
        }
        
        effectToStore.gameObject.SetActive(false);
        effectToStore.transform.localScale = Vector3.one;
        effectToStore.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            IncrementScore(scorePerSecond);
            elapsedTime = 0;
        }
    }

    internal void UpdateThrustInput(float val)
    {
        player.currThrust = val;
    }

    internal void UpdateRudderAngle(float val)
    {
        player.currRudderAngle = -(val - 0.5f);
    }

    internal void UpdateAimAngle(float val)
    {
        player.currAimAngle = -(val - 0.5f);
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
