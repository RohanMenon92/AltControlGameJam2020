using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletScript : MonoBehaviour
{
    // Can be part of a "Bullet" Super Class
    public bool isEnemyShot;
    public float damage;
    public float bulletSpeed;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReturnBulletToPool()
    {
        // Return Bullet
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.ShotGun);
    }

    internal void OnHit()
    {
        ReturnBulletToPool();
    }

    // Called when player refelects a shot
    internal void OnShield(Vector3 impactPoint)
    {
        isEnemyShot = false;
        this.GetComponent<Rigidbody>().velocity = (transform.position - impactPoint).normalized * bulletSpeed;
    }

    internal void FireShotgun()
    {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
