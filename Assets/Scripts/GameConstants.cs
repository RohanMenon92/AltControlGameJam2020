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
    public static float maxHealth = 300;
    public static float maxEnergy = 300;
    internal static float EnergyConsumptionThrust = 0.5f;

    public static float RotateRudderRate = 90f;
    public static float RotateAimRate = 120f;
    internal static float RechargeGain = 30f;
    internal static string HighScorePlayerPref = "HighScore";

    internal static float ShieldAppearTime = 0.75f;
    public static float ShieldEnergyUsage = 0.2f;

    // Start is called before the first frame update

}
