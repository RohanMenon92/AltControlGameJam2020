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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<LaserBulletScript>() != null)
        {
            HitByLaser();
        }
        if (collision.gameObject.GetComponent<ShotgunBulletScript>() != null)
        {
            HitByShotgun();
        }
        if (collision.gameObject.GetComponent<BulletScript>() != null)
        {
            HitByBullet();
        }

    }

    void HitByLaser()
    {

    }

    void HitByShotgun()
    {

    }

    void HitByBullet()
    {

    }

    void FireCannons()
    {
        foreach (GunPort gun in gunPorts)
        {
            gun.Fire(true);
        }
    }
}
