using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Range(-0.5f, 0.5f)]
    public float currThrust;

    [Range(-0.5f, 0.5f)]
    public float currRudderAngle;

    [Range(-0.5f, 0.5f)]
    public float currAimAngle;

    [Range(-0.5f, 0.5f)]
    public float currPreciseAimAngle;

    public float shipSpeed;

    public Transform shipShield;

    public List<GunPort> gunPorts;

    public float health, energy;


    // Start is called before the first frame update
    void Start()
    {
        health = GameConstants.maxHealth;
        energy = GameConstants.maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        // 359 degrees because we want to limit rotation parameters to the potentiometer
        // Rotate Ship
        transform.localEulerAngles = AngleLerp(transform.localEulerAngles, new Vector3(0f, currRudderAngle * 359, 0f), Time.deltaTime);

        // Rotate Ship by rate
        shipShield.localEulerAngles = AngleLerp(shipShield.localEulerAngles, new Vector3(0f, currAimAngle * 359, 0f), Time.deltaTime);
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
        GetComponent<Rigidbody>().AddForce(transform.forward * currThrust * shipSpeed, ForceMode.VelocityChange);
    }
    void FireCannons()
    {
        foreach(GunPort gun in gunPorts)
        {
            gun.Fire(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {

        }
    }

    internal void OnShield()
    {
        throw new NotImplementedException();
    }

    internal void OnRecharge()
    {
        throw new NotImplementedException();
    }

    internal void OnFire()
    {
        throw new NotImplementedException();
    }
}
