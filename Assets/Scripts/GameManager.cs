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
    public float currShieldAngle;


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

        shipShield.localEulerAngles = new Vector3(0f, currShieldAngle * 360, 0f);


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

        ship.GetComponent<Rigidbody>().AddForce(ship.forward * currThrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    internal void UpdateThrustInput(float val)
    {
        currThrust = val - 0.5f;
    }

    internal void UpdateRudderAngle(float val)
    {
        currRudderAngle = val - 0.5f;
    }

    internal void UpdateShieldAngle(float val)
    {
        currShieldAngle = val;
    }

    internal void UpdateRechargeButton(bool val)
    {
        if(isRechargePressed && !val)
        {
            //OnRechargeUp();
        } else if(!isRechargePressed && val)
        {
            OnRechargeActivate();
        }
        isRechargePressed = val;
    }

    private void OnRechargeActivate()
    {
        throw new NotImplementedException();
    }

    internal void UpdateFireButton(bool val)
    {
        if (isFiring && !val)
        {
            //OnFireUp();
        }
        else if (!isFiring && val)
        {
            OnFireActivate();
        }
        isRechargePressed = val;
    }

    private void OnFireActivate()
    {
        throw new NotImplementedException();
    }

    internal void UpdateShieldButton(bool val)
    {
        if (isShielding && !val)
        {
            //OnFireUp();
        }
        else if (!isShielding && val)
        {
            OnShieldActivate();
        }
        isShielding = val;
    }

    private void OnShieldActivate()
    {
        throw new NotImplementedException();
    }
}
