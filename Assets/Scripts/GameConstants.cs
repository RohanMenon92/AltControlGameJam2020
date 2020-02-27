using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public enum InputSignals
    {
        P2,
        P3,
        P4,
        B1,
        B2,
        B3,
        P1
    }
    public enum GunTypes
    {
        MachineGun,
        ShotGun,
        LaserGun
    }

    public static int NormalBulletPoolSize = 100;
    public static int ShotgunBulletPoolSize = 50;
    public static int LaserBulletPoolSize = 10;
    public static float maxHealth = 100;
    public static float maxEnergy = 100;
    internal static float EnergyConsumptionThrust = 0.5f;

    public static float RotateRudderRate = 30f;
    public static float RotateAimRate = 30f;
    internal static float RechargeGain = 5f;
    internal static string HighScorePlayerPref = "HighScore";

    // Start is called before the first frame update

}
