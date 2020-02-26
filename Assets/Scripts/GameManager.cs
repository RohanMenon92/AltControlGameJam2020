using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // public player
    public PlayerScript player;

    bool isRechargePressed;
    bool isFiring;
    bool isShielding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {      


    }

    internal void UpdateThrustInput(float val)
    {
        player.currThrust = val - 0.5f;
    }

    internal void UpdateRudderAngle(float val)
    {
        player.currRudderAngle = val - 0.5f;
    }

    internal void UpdateAimAngle(float val)
    {
        player.currAimAngle = val - 0.5f;
    }

    internal void UpdatePreciseAimAngle(float val)
    {
        player.currPreciseAimAngle = val - 0.5f;
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
        player.OnRecharge();
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
        player.OnFire();
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
        player.OnShield();
    }
}
