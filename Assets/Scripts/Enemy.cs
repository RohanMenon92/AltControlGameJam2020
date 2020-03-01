﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerScript player;
    public float enemyHealth;
    float currentHealth;
    public float speed;
    public float shootingRange;

    public List<GunPort> gunPorts;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();

        currentHealth = enemyHealth;
    }

    private void Update()
    {
        Move();
        FireCannons();
        Die();
    }

    public void Move()
    {
        transform.LookAt(player.transform.position);
        if (Vector3.Distance(player.transform.position, transform.position) > shootingRange)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void Die()
    {
        if (currentHealth <= 0)
        {
            currentHealth = enemyHealth;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();
        ShotgunBulletScript shotgunBullet = collision.gameObject.GetComponent<ShotgunBulletScript>();
        BulletScript normalBullet = collision.gameObject.GetComponent<BulletScript>();

        if (laserBullet != null && !laserBullet.isEnemyShot)
        {
            HitByBullet(laserBullet.damage);
            laserBullet.OnHit();
        }
        else if (shotgunBullet != null && !shotgunBullet.isEnemyShot)
        {
            HitByBullet(shotgunBullet.damage);
            shotgunBullet.OnHit();
        }
        else if (normalBullet != null && !normalBullet.isEnemyShot)
        {
            HitByBullet(normalBullet.damage);
            normalBullet.OnHit();
        }
    }

    void HitByBullet(float damage)
    {
        currentHealth -= damage;
        //Debug.Log("'BOUTTA DEAL " + damage + " TO THIS SPACESHIP");
        //Debug.Log("CURRENT HEALTH: "+currentHealth);
        //Debug.Log("OUCH!");
    }

    void FireCannons()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= shootingRange+3)
        {
            float spread = Random.Range(-10f, 10f);
            foreach (GunPort gun in gunPorts)
            {
                transform.Rotate(0f, spread, 0f);
                gun.Fire(true, transform);
                transform.Rotate(0f, -spread, 0f);
            }
        }
    }
}
