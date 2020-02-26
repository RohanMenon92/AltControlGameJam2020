﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject shotgunBulletPrefab;
    public GameObject bulletPrefab;
    public GameObject laserBulletPrefab;

    public Transform unusedBulletPool;
    public Transform unusedShotgunBulletPool;
    public Transform unusedLaserBulletPool;

    // public player
    PlayerScript player;

    bool isRechargePressed;
    bool isFiring;
    bool isShielding;

    // Start is called before the first frame update
    void Start()
    {
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

        player = FindObjectOfType<PlayerScript>();
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
                bulletObject = unusedShotgunBulletPool.GetComponentInChildren<LaserBulletScript>(true).gameObject;
                break;
        }

        bulletObject.transform.SetParent(gunPort);
        bulletObject.transform.localPosition = Vector3.zero;
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


    }

    internal void UpdateThrustInput(float val)
    {
        player.currThrust = val - 0.5f;
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
            }
        }
        isShielding = val;
    }

    private void OnShieldActivate()
    {
        player.OnShield();
    }
}
