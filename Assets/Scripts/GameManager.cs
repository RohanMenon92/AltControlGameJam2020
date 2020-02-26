using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(-0.5f, 0.5f)]
    public float currThrust;

    [Range(-0.5f,0.5f)]
    public float currRudderAngle;

    [Range(-0.5f, 0.5f)]
    public float currAimAngle;

    [Range(-0.5f, 0.5f)]
    public float currPreciseAimAngle;

    public float shipSpeed;

    bool isRechargePressed;
    bool isFiring;
    bool isShielding;

    public Transform ship;
    public Transform shipShield;


    // public player

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

//        ship.localEulerAngles = new Vector3(0f, ship.localEulerAngles.y + currRudderAngle * 200 * Time.deltaTime, 0f);
        ship.localEulerAngles = new Vector3(0f, currRudderAngle * 360, 0f);

        shipShield.localEulerAngles = new Vector3(0f, currAimAngle * 360, 0f);


        if (currThrust > 0)
        {
            //PlayerPrefs.AddForce(transform.forward * currThrust);
        } else
        {
            //PlayerPrefs.velocity -= currThrust;
        }
    }

    private void FixedUpdate()
    {
        ship.GetComponentInParent<Rigidbody>().AddForce(ship.forward * currThrust * shipSpeed, ForceMode.VelocityChange);
    }

    internal void UpdateThrustInput(float val)
    {
        currThrust = val - 0.5f;
    }

    internal void UpdateRudderAngle(float val)
    {
        currRudderAngle = val - 0.5f;
    }

    internal void UpdateAimAngle(float val)
    {
        currAimAngle = val - 0.5f;
    }

    internal void UpdatePreciseAimAngle(float val)
    {
        currPreciseAimAngle = val - 0.5f;
    }

    internal void UpdateRechargeButton(bool val)
    {
        if (isRechargePressed != val)
        {
            if (!isRechargePressed)
            {
                OnRechargeActivate();
            }
        }
        isRechargePressed = val;
    }

    private void OnRechargeActivate()
    {
        Debug.Log("RECHARGE ACTIVATED!!");
    }

    internal void UpdateFireButton(bool val)
    {
        if (isFiring != val)
        {
            if(!isFiring)
            {
                OnFireActivate();
            }
        }
        isFiring = val;
    }

    private void OnFireActivate()
    {
        Debug.Log("FIRE ACTIVATED");
    }

    internal void UpdateShieldButton(bool val)
    {
        if (isShielding != val)
        {
            if (!isShielding)
            {
                OnShieldActivate();
            }
        }
        isShielding = val;
    }

    private void OnShieldActivate()
    {
        Debug.Log("SHIELD ACTIVATED");
    }
}
