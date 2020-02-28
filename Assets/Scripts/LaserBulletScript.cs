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

    float timeAlive;
    GameManager gameManager;
    LineRenderer lineRenderer;
    Sequence beamTween;
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

    // Update is called once per frame
    void Update()
    {
        CheckDeath(Time.deltaTime);
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
            beamTween = DOTween.Sequence();
            lineRenderer.material.SetFloat("NoiseAmount", 1f);
            lineRenderer.material.SetFloat("NoiseScale", 1f);
            beamTween.Insert(0f, lineRenderer.material.DOFloat(0.1f, "NoiseAmount", aliveForSeconds / 0.75f));
            beamTween.Insert(0f, lineRenderer.material.DOFloat(0.2f, "NoiseScale", aliveForSeconds / 0.75f));
            beamTween.Insert(aliveForSeconds / 0.75f, lineRenderer.material.DOFloat(0.0f, "NoiseAmount", aliveForSeconds / 0.25f));
            beamTween.Insert(aliveForSeconds / 0.75f, lineRenderer.material.DOFloat(0.0f, "NoiseScale", aliveForSeconds / 0.25f));
            beamTween.Play();
        }
    }
}
