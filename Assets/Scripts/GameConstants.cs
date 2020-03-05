using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    // Input Interface
    public enum InputSignals
    {
        P1,
        P2,
        P3,
        P4,
        B1,
        B2,
        B3
    }

    // Effect Type for ships
    public enum EffectTypes
    {
        SmokeEffect,
        BulletHit,
        ShieldHit,
        ShipExplosion
    }

    // Gun Types for ships
    public enum GunTypes
    {
        MachineGun,
        ShotGun,
        LaserGun
    }
    
    // Normal Bullet Pooling Size
    public static int NormalBulletPoolSize = 100;
    // Shotgun Bullet pooling size
    public static int ShotgunBulletPoolSize = 100;
    // Laser bullet pooling size
    public static int LaserBulletPoolSize = 50;
    // Effects pooling size
    public static int BulletEffectsPoolSize = 100;
    // Effects pooling size
    public static int ShieldEffectsPoolSize = 100;
    // Effects pooling size
    public static int DamageSmokePoolSize = 100;
    // Effects pooling size
    public static int ExplosionPoolSize = 100;

    // Maximum health
    public static float maxHealth = 300;
    // Maximum energy
    public static float maxEnergy = 300;

    // Thrust consumes energy (is multiplied by rate of thrust)
    internal static float EnergyConsumptionThrust = 0.5f;

    // Speed at which rudder turns ship
    public static float RotateRudderRate = 90f;

    // Speed at which rotate aiming works
    public static float RotateAimRate = 120f;
    
    
    public static string HighScorePlayerPref = "HighScore";

    // Amount of recharge gained when pressing Recharge
    public static float RechargeGain = 30f;
    // Energy that shield uses
    public static float ShieldEnergyUsage = 0.2f;

    // Fade In time for shield (still refelects bullets as soon as it is pressed)
    public static float ShieldAppearTime = 0.75f;
    // Area in front where shield is not active
    public static float ShieldFrontThreshold = 1.7f;
    // Number of waves to win
    public static int WaveWinCondition = 5;

    // Bullet hit Lifetime
    public static float BulletHitLife = 2.0f;
    // Shield hit Lifetime
    public static float ShieldHitLife = 2.0f;
    // Smoke Damage Fade in
    public static float SmokeDamageFade = 1.0f;
    // Explosion Lifetime
    public static float ExplodeLifetime = 10.0f;

    // Beam damage rate
    public static float BeamDamageRate = 10.0f;
}
