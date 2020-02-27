using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletScript : MonoBehaviour
{
    // Can be part of a "Bullet" Super Class
    public bool isEnemyShot;
    public float damage;
    public float bulletSpeed = 0;
    public float aliveForSeconds = 0.25f;
    public float beamLength = 50;

    float timeAlive;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        transform.GetComponentInChildren<LineRenderer>().transform.localScale = new Vector3(0f, 0f, beamLength);
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
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.LaserGun);
    }

    internal void OnHit()
    {
        //ReturnBulletToPool();
    }

    internal void FireLaser()
    {
        this.GetComponentInChildren<LineRenderer>().material.SetFloat("NoiseAmount", 0f);
        this.GetComponentInChildren<LineRenderer>().material.SetFloat("NoiseScale", 0f);
        this.GetComponentInChildren<LineRenderer>().material.DOFloat(0.8f, "NoiseAmount", aliveForSeconds);
        this.GetComponentInChildren<LineRenderer>().material.DOFloat(0.8f, "NoiseScale", aliveForSeconds);
    }
}
