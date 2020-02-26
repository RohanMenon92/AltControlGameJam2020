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
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.LaserGun);
    }

    internal void OnHit()
    {
        //ReturnBulletToPool();
    }

    internal void FireLaser()
    {
        Debug.Log("FIRE LAAAAAAAAAZZZZZZZZZZZZZUUUUUUUUURRRRRRRRRRRRR");
    }
}
