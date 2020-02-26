using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerScript player;
    public float enemyHealth;
    public float speed; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 15f)
        {
            transform.LookAt(player.transform.position);
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
}
