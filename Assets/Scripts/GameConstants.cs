using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
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

    // Start is called before the first frame update

}
