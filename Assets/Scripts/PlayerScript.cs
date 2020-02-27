using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class PlayerScript : MonoBehaviour
{
    [Header("Slider Parameters")]
    [Range(0.0f, 1.0f)]
    public float currThrust;
    [Range(-0.5f, 0.5f)]
    public float currRudderAngle;
    [Range(-0.5f, 0.5f)]
    public float currAimAngle;
    [Range(-0.5f, 0.5f)]
    public float currPreciseAimAngle;

    [Header("Movement Parameters")]
    public float shipSpeed;
    public Transform shipShield;


    [Header("Game Play")]
    public List<GunPort> gunPorts;
    public float health, energy;

    public VisualEffect engineEffect;

    // Start is called before the first frame update
    void Start()
    {
        health = GameConstants.maxHealth;
        energy = GameConstants.maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FireCannons();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            OnRecharge();
        }

        // 359 degrees because we want to limit rotation parameters to the potentiometer
        // Rotate Ship
        transform.localEulerAngles = AngleLerp(transform.localEulerAngles, new Vector3(0f, transform.localEulerAngles.y + (GameConstants.RotateRudderRate * currRudderAngle), 0f), Time.deltaTime);

        // Rotate Ship by rate
        shipShield.localEulerAngles = AngleLerp(shipShield.localEulerAngles, new Vector3(0f, shipShield.localEulerAngles.y + (GameConstants.RotateAimRate * currAimAngle), 0f), Time.deltaTime);
    }

    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        //float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        //float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        //Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        Vector3 Lerped = new Vector3(0, yLerp, 0);
        return Lerped;
    }
    private void FixedUpdate()
    {
        PerformThrust();
    }

    void PerformThrust()
    {
        engineEffect.SetFloat("EngineThrust", currThrust);
        energy -= currThrust * GameConstants.EnergyConsumptionThrust;
        GetComponent<Rigidbody>().AddForce(transform.forward * currThrust * shipSpeed, ForceMode.VelocityChange);
    }


    void FireCannons()
    {
        foreach(GunPort gun in gunPorts)
        {
            gun.Fire(false, transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            // Ramming logic
        }


        // HitLogic
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();
        ShotgunBulletScript shotgunBullet = collision.gameObject.GetComponent<ShotgunBulletScript>();
        BulletScript normalBullet = collision.gameObject.GetComponent<BulletScript>();

        if (laserBullet != null && laserBullet.isEnemyShot)
        {
            TakeDamage(laserBullet.damage);
            laserBullet.OnHit();
        }
        else if (shotgunBullet != null && shotgunBullet.isEnemyShot)
        {
            TakeDamage(shotgunBullet.damage);
            shotgunBullet.OnHit();
        }
        else if (normalBullet != null && normalBullet.isEnemyShot)
        {
            TakeDamage(normalBullet.damage);
            normalBullet.OnHit();
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
    }

    // BUTTON PRESSES
    internal void OnShield()
    {
        throw new NotImplementedException();
    }

    internal void OnRecharge()
    {
        energy += GameConstants.RechargeGain;
    }

    internal void OnFire()
    {
        FireCannons();
    }
}
