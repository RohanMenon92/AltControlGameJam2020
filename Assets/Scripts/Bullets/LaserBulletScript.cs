using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletScript : MonoBehaviour
{
    // Can be part of a "Bullet" Super Class
    public bool isEnemyShot;
    public float damagePerSecond = 0.1f;
    public float damage = 5f;
    public float bulletSpeed = 0;
    public float aliveForSeconds = 0.25f;
    public float beamLength = 50;

    float currBeamLength = 50;
    float closestObjectDistance;

    float timeAlive;
    GameManager gameManager;
    LineRenderer lineRenderer;
    Sequence beamTween;
    string shaderNoiseAmount = "Vector1_AE1B794B";
    string shaderFadeAmount = "Vector1_7E907178";

    Transform endPoint;

    LaserBulletScript reflectedLaser;
    private float beamcollisionLength = 50f;
    private bool isColliding = false;
    private bool is3d = false;

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

    private void OnTriggerExit(Collider other)
    {
        // On exit from collider, reset is Colliding
        isColliding = false;
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

    public void CheckBeamCollisionStay(Vector3 hitPoint, Vector3 normal)
    {
        // TO DO: fix this, reflect the laser only when bool called isShield is true, in this function
        float distanceToBeam = Vector3.Distance(hitPoint, transform.position) + 5f;
        if (distanceToBeam < currBeamLength)
        {
            beamcollisionLength = distanceToBeam;
            if (reflectedLaser != null)
            {
                Vector3 reflectionAngle = Vector3.Reflect(endPoint.forward, normal);
                if(!is3d)
                {
                    reflectionAngle = new Vector3(reflectionAngle.x, 0f, reflectionAngle.z);
                }
                reflectedLaser.transform.forward = reflectionAngle;
                reflectedLaser.transform.position = hitPoint + reflectedLaser.transform.forward;
            }
        }


        isColliding = true;
    }

    void UpdateBeamLength()
    {
        // Hold down position of beam
        if (!isColliding)
        {
            currBeamLength = beamLength;
        } else
        {
            currBeamLength = beamcollisionLength;
        }

        lineRenderer.transform.localScale = new Vector3(1f, 1f, currBeamLength);
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, (currBeamLength / 2) - 5f);
        this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, currBeamLength + 5);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeamLength();
        CheckDeath(Time.deltaTime);
    }

    private void FixedUpdate()
    {
    }

    void ReturnBulletToPool()
    {
        if(reflectedLaser!=null)
        {
            reflectedLaser.ReturnBulletToPool();
        }
        // Return Bullet
        gameManager.ReturnBulletToPool(gameObject, GameConstants.GunTypes.LaserGun);
    }

    internal void OnHit()
    {
        // Create Particle Effect for hit
        //ReturnBulletToPool();
    }

    public void OnShieldEnter(Vector3 normalVector, Vector3 startPoint)
    {
        // Create new Laser Bullet at Reflection Point
        GameObject reflectedObject = gameManager.GetBullet(GameConstants.GunTypes.LaserGun, endPoint);

        reflectedLaser = reflectedObject.GetComponent<LaserBulletScript>();
        // Set same time as current laser
        reflectedLaser.isEnemyShot = !isEnemyShot;

        reflectedLaser.transform.position = startPoint;
        reflectedLaser.transform.forward = Vector3.Reflect(endPoint.forward, normalVector);

        reflectedObject.SetActive(true);
        reflectedLaser.FireLaser();
    }

    internal void FireLaser()
    {
        timeAlive = 0;
        lineRenderer = this.GetComponentInChildren<LineRenderer>();

        lineRenderer.transform.localScale = new Vector3(1f, 1f, beamLength);
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, (beamLength / 2) - 5);
        this.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, beamLength);

        currBeamLength = beamLength;
        UpdateBeamLength();

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
