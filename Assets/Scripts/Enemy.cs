using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerScript player;
    public float enemyHealth;
    public float speed;

    public List<GunPort> gunPorts;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.LookAt(player.transform.position);
        if (Vector3.Distance(player.transform.position, transform.position) > 15f)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        enemyHealth -= damage;
    }

    void FireCannons()
    {
        foreach (GunPort gun in gunPorts)
        {
            gun.Fire(true, transform);
        }
    }
}
