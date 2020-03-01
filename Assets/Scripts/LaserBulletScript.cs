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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        lineRenderer = this.GetComponentInChildren<LineRenderer>();
        lineRenderer.transform.localScale = new Vector3(0f, 0f, beamLength);
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, beamLength / 2);
        this.GetComponent<BoxCollider>().size = new Vector3(0f, 0f, beamLength);
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
        lineRenderer.transform.localScale = new Vector3(0f, 0f, currBeamLength);
        this.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, (currBeamLength / 2) + 0.5f);
        this.GetComponent<BoxCollider>().size = new Vector3(0f, 0f, currBeamLength + 1);


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
        //ReturnBulletToPool();
    }

    internal void FireLaser()
    {
        if(lineRenderer != null)
        {
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
