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

    float currBeamLength;
    float closestObjectDistance;

    float timeAlive;
    GameManager gameManager;
    LineRenderer lineRenderer;
    Sequence beamTween;
    string shaderNoiseAmount = "Vector1_AE1B794B";
    string shaderFadeAmount = "Vector1_7E907178";

    Transform endPoint;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        lineRenderer = this.GetComponentInChildren<LineRenderer>();

        endPoint = lineRenderer.transform.GetChild(0);
    }

    void OnEnable()
    {
        timeAlive = 0;
    }

    private void OnDisable()
    {
        if (lineRenderer != null)
        {
            lineRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
        this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
    }

    void CheckDeath(float time)
    {
        timeAlive += time;
        if (timeAlive > aliveForSeconds)
        {
            ReturnBulletToPool();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool isValidHit = false;
        // Set Closest Object and apply to beam length
        if(!isEnemyShot)
        {
            isValidHit = other.tag == "Asteroid" || other.tag == "EnemyShip";
        } else
        {
            isValidHit = other.tag == "Asteroid" || other.tag == "Player";
        }


        if (isValidHit)
        {
            float objectDist = Vector3.Distance(transform.position, other.transform.position);
            if (objectDist < currBeamLength)
            {
                currBeamLength = objectDist;
            }
        }
    }

    void UpdateBeamLength()
    {
        lineRenderer.transform.localScale = new Vector3(1f, 1f, currBeamLength);
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, (currBeamLength / 2) - 5f);
        this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, currBeamLength + 5);

        // Reset beamLength for next Calculation
        currBeamLength = beamLength;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        UpdateBeamLength();
    }

    void ReturnBulletToPool()
    {
        // Return Bullet
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.LaserGun);
    }

    internal void OnHit()
    {
        // Create Particle Effect for hit
        //ReturnBulletToPool();
    }

    public void OnShield(Vector3 normalVector, Vector3 startPoint)
    {
        // Create new Laser Bullet at Reflection Point
        GameObject reflectedObject = gameManager.GetBullet(GameConstants.GunTypes.LaserGun, endPoint);

        LaserBulletScript reflectedLaser = reflectedObject.GetComponent<LaserBulletScript>();
        // Set same time as current laser
        reflectedLaser.isEnemyShot = !isEnemyShot;

        reflectedLaser.transform.position = startPoint;
        reflectedLaser.transform.forward = Vector3.Reflect(endPoint.forward, normalVector);

        reflectedObject.SetActive(true);
        reflectedLaser.FireLaser();
        reflectedLaser.timeAlive = timeAlive;
        // TO DO  DO LASER REFLECTION BASED ON OBJECT CREATION
    }

    internal void FireLaser()
    {
        timeAlive = 0;
        if(lineRenderer != null)
        {
            lineRenderer.transform.localScale = new Vector3(1f, 1f, beamLength);
            this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, (beamLength / 2) - 5);
            this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, beamLength);

            currBeamLength = beamLength;

            beamTween = DOTween.Sequence();
            lineRenderer.material.SetFloat(shaderNoiseAmount, 1f);
            lineRenderer.material.SetFloat(shaderFadeAmount, 0f);

            beamTween.Insert(0f, lineRenderer.material.DOFloat(0.1f, shaderNoiseAmount, aliveForSeconds * 0.75f));
            beamTween.Insert(0f, lineRenderer.material.DOFloat(10f, shaderFadeAmount, aliveForSeconds * 0.5f));
            
            beamTween.Insert(aliveForSeconds * 0.8f, lineRenderer.material.DOFloat(1.0f, shaderNoiseAmount, aliveForSeconds * 0.2f));
            beamTween.Insert(aliveForSeconds * 0.8f, lineRenderer.material.DOFloat(0.0f, shaderFadeAmount, aliveForSeconds * 0.2f));
            
            beamTween.Play();
        }
    }
}
