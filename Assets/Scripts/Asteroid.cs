using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private ParticleSystem particle;
    private MeshRenderer renderer;
    private AsteroidPool pool;
    public int scoreReward = 350;

    GameManager gameManager;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        pool = FindObjectOfType<AsteroidPool>();
        renderer = GetComponent<MeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();

        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnHit()
    {
        // play explosion animation

        particle.Play();
        StartCoroutine(waitForAnimation());
        GetComponent<MeshCollider>().enabled = false;
        // added for a possibility to count asteroid score
        if (renderer.isVisible)
        {
            FindObjectOfType<GameManager>().score += scoreReward;
        }
    }

    IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(0.15f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        particle.Stop();
        yield return new WaitForSeconds(2.0f);
        pool.unusedAsteroid.Add(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider collision)
    {
        // Check Laser CollisionFOrBullet
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();

        if (laserBullet != null)
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

            gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);

            laserBullet.CheckBeamCollisionStay(collisionPoint, collisionNormal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LaserBulletScript laserBullet = other.gameObject.GetComponent<LaserBulletScript>();
        ShotgunBulletScript shotgunBullet = other.gameObject.GetComponent<ShotgunBulletScript>();
        BulletScript normalBullet = other.gameObject.GetComponent<BulletScript>();

        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

        gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);

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
        // Use damage to reduce health here
        OnHit();
    }
}
