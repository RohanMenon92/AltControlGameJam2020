using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Can be part of a "Bullet" Super Class
    public bool isEnemyShot;
    public float damage;
    public float bulletSpeed;
    public float aliveForSeconds = 2f;

    float timeAlive;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();   
    }

    void CheckDeath(float time)
    {
        timeAlive += time;
        if(timeAlive > aliveForSeconds)
        {
            ReturnBulletToPool();
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckDeath(Time.deltaTime);
    }

    void ReturnBulletToPool()
    {
        // Return Bullet
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.MachineGun);
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

    internal void FireBullet()
    {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
