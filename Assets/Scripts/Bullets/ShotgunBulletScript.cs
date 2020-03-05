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
    public float aliveForSeconds = 1f;

    float timeAlive;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    void OnEnable()
    {
        timeAlive = 0;
    }
    void CheckDeath(float time)
    {
        timeAlive += time;
        if (timeAlive > aliveForSeconds)
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
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.ShotGun);
    }

    internal void OnHit()
    {
        gameObject.SetActive(false);
        ReturnBulletToPool();
    }

    // Called when player refelects a shot
    internal void OnShield(Vector3 normalVector)
    {
        Debug.Log("OnShield " + gameObject);
        // When HitByShield, reverseBullet according to normal Vector
        isEnemyShot = !isEnemyShot;
        timeAlive = 0;

        Vector3 newVelocity = Vector3.Reflect(this.GetComponent<Rigidbody>().velocity, normalVector);
        this.GetComponent<Rigidbody>().velocity = new Vector3(newVelocity.x, 0f, newVelocity.z);
    }

    internal void FireShotgun()
    {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
