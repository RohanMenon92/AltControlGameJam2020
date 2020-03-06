using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerScript player;
    public float enemyHealth;
    float currentHealth;
    public float speed;
    public float shootingRange;
    public int scoreReward = 500;
    Rigidbody rb;
    public List<GunPort> gunPorts;
    public AudioClip deathSound;
    public AudioClip hitSound;
    AudioSource musicPlayer;

    GameManager gameManager;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        currentHealth = enemyHealth;
        musicPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        FireCannons();
        Die();

        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    public void Move()
    {
        transform.LookAt(player.transform.position);
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > shootingRange && distance < shootingRange * 3)
        {
            rb.isKinematic = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if (distance >= shootingRange * 3)
        {
            rb.isKinematic = true;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * 5 * Time.deltaTime);
        }
    }

    void Die()
    {
        if (currentHealth <= 0)
        {
            currentHealth = enemyHealth;
            gameObject.SetActive(false);
            gameManager.IncrementScore(scoreReward);
            gameManager.BeginEffect(GameConstants.EffectTypes.ShipExplosion, transform.position, transform.up);
            GetComponentInParent<EnemyPool>().activeEnemy.Remove(gameObject);
            musicPlayer.clip = deathSound;
            if (!musicPlayer.isPlaying)
            {
                musicPlayer.Play();
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Check Laser CollisionFOrBullet
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();

        if (laserBullet != null && !laserBullet.isEnemyShot)
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

            if (Time.frameCount % GameConstants.BeamDamageRate == 0)
            {
                gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                HitByBullet(laserBullet.damagePerSecond);
            }

            laserBullet.CheckBeamCollisionStay(collisionPoint, collisionNormal);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();
        ShotgunBulletScript shotgunBullet = collision.gameObject.GetComponent<ShotgunBulletScript>();
        BulletScript normalBullet = collision.gameObject.GetComponent<BulletScript>();

        Vector3 collisionPoint = collision.ClosestPoint(transform.position);
        Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

        gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);

        if (laserBullet != null && !laserBullet.isEnemyShot)
        {
            HitByBullet(laserBullet.damage);
            laserBullet.OnHit();
            musicPlayer.clip = hitSound;
            if (!musicPlayer.isPlaying)
            {
                musicPlayer.Play();
            }
        }
        else if (shotgunBullet != null && !shotgunBullet.isEnemyShot)
        {
            HitByBullet(shotgunBullet.damage);
            shotgunBullet.OnHit();
            musicPlayer.clip = hitSound;
            if (!musicPlayer.isPlaying)
            {
                musicPlayer.Play();
            }
        }
        else if (normalBullet != null && !normalBullet.isEnemyShot)
        {
            HitByBullet(normalBullet.damage);
            normalBullet.OnHit();
            musicPlayer.clip = hitSound;
            if (!musicPlayer.isPlaying)
            {
                musicPlayer.Play();
            }
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
