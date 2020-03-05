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
    public bool turretSpecificRotate = false;

    public VisualEffect engineEffect;

    ShieldScript shieldScript;
    AdaptiveShieldScript adaptiveShield;
    
    bool isShielding;
    Collider selfCollider;
    GameManager gameManager;
    private bool is3D = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        health = GameConstants.maxHealth;
        energy = GameConstants.maxEnergy;

        InitializeShieldColliders();

        selfCollider = GetComponent<Collider>();
    }

    void InitializeShieldColliders()
    {
        shieldScript = GetComponentInChildren<ShieldScript>(true);
        adaptiveShield = GetComponentInChildren<AdaptiveShieldScript>(true);

        shieldScript.gameObject.SetActive(true);
        adaptiveShield.gameObject.SetActive(true);

        adaptiveShield.shieldOn = false;
        shieldScript.TurnOffShield();
    }

    void TakeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCannons();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShieldOn();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            ShieldOff();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRecharge();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (currThrust < 1.0f)
            {
                currThrust += 0.01f;
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (currThrust - 0.01f < 0f)
            {
                currThrust = 0f;
            }
            else if (currThrust > 0.0f)
            {
                currThrust -= 0.01f;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (currRudderAngle > -0.5f)
            {
                currRudderAngle -= 0.001f;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (currRudderAngle < 0.5f)
            {
                currRudderAngle += 0.001f;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currAimAngle < 0.5f)
            {
                currAimAngle += 0.001f;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (currAimAngle > -0.5f)
            {
                currAimAngle -= 0.001f;
            }
        }
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

    void CheckShield()
    {
        if(isShielding)
        {
            if(energy > 0f)
            {
                energy -= GameConstants.ShieldEnergyUsage;
            } else
            {
                ShieldOff();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health<0)
        {
            gameManager.GameOver();
        }
        TakeInput();
        CheckShield();
        DoShipRotations();
    }

    private void DoShipRotations()
    {
        // 359 degrees because we want to limit rotation parameters to the potentiometer
        // Rotate Ship
        transform.localEulerAngles = AngleLerp(transform.localEulerAngles, new Vector3(0f, transform.localEulerAngles.y + (GameConstants.RotateRudderRate * currRudderAngle), 0f), Time.deltaTime);

        if (turretSpecificRotate)
        {
            // Rotate Ship to specific amount
            shipShield.localEulerAngles = AngleLerp(shipShield.localEulerAngles, new Vector3(0f, currAimAngle * 359, 0f), Time.deltaTime);
        }
        else
        {
            // Rotate Ship by rate
            shipShield.localEulerAngles = AngleLerp(shipShield.localEulerAngles, new Vector3(0f, shipShield.localEulerAngles.y + (GameConstants.RotateAimRate * currAimAngle), 0f), Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        PerformThrust();
    }

    void PerformThrust()
    {
        if(energy > 0f)
        {
            energy -= currThrust * GameConstants.EnergyConsumptionThrust;
            engineEffect.SetFloat("EngineThrust", currThrust);
            engineEffect.SetFloat("RudderAngle", currRudderAngle);
            GetComponent<Rigidbody>().AddForce(transform.forward * currThrust * shipSpeed, ForceMode.VelocityChange);
        } else
        {
            engineEffect.SetFloat("EngineThrust", 0f);
        }
    }


    void FireCannons()
    {
        if(energy > 0f)
        {
            foreach (GunPort gun in gunPorts)
            {
                gun.Fire(false, transform);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            // Ramming logic
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Check Laser CollisionFOrBullet
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();

        if (laserBullet != null && laserBullet.isEnemyShot)
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

            if (!is3D)
            {
                // Convert to 2D tangent system
                collisionNormal = new Vector3(collisionNormal.x, 0f, collisionNormal.z);
            }

            if (isShielding && !IsFrontShot(collision.transform))
            {
                // Get Effect for follow up shots every few frames
                if (Time.frameCount % GameConstants.BeamDamageRate == 0)
                {
                    gameManager.BeginEffect(GameConstants.EffectTypes.ShieldHit, collisionPoint, collisionNormal);
                }
            }
            else
            {
                if (Time.frameCount % GameConstants.BeamDamageRate == 0)
                {
                    gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal);
                    TakeDamage(laserBullet.damagePerSecond);
                }
            }
            laserBullet.CheckBeamCollisionStay(collisionPoint, collision.ClosestPoint(transform.position));
        }
    }

    bool IsFrontShot(Transform shotPoint)
    {
        return (shipShield.forward - shotPoint.forward).magnitude > GameConstants.ShieldFrontThreshold;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // TODO: Create COmmon BulletClass
        // HitLogic
        LaserBulletScript laserBullet = collision.gameObject.GetComponent<LaserBulletScript>();
        ShotgunBulletScript shotgunBullet = collision.gameObject.GetComponent<ShotgunBulletScript>();
        BulletScript normalBullet = collision.gameObject.GetComponent<BulletScript>();

        // Convert BoundsToLocalSpace
        Vector3 collisionPoint = collision.ClosestPoint(transform.position);
        Vector3 collisionNormal = transform.InverseTransformDirection(collisionPoint - transform.position).normalized;

        if (!is3D)
        {
            // Convert to 2D tangent system
            collisionNormal = new Vector3(collisionNormal.x, 0f, collisionNormal.z);
        }

        // Check if the shot is from the front within a threshold 1.7f is north east to north west
        if (laserBullet != null && laserBullet.isEnemyShot)
        {
            if(isShielding && !IsFrontShot(collision.transform))
            {
                // Calculate Normal Differently for laser beam
                collisionNormal = transform.InverseTransformDirection(selfCollider.ClosestPoint(collision.transform.position) - transform.position).normalized;
                
                // Spawn Effect and set its parent as ship
                gameManager.BeginEffect(GameConstants.EffectTypes.ShieldHit, collisionPoint, collisionNormal).transform.SetParent(transform);

                adaptiveShield.OnHit(collisionNormal, laserBullet.damage);
                laserBullet.OnShieldEnter(transform.TransformDirection(collisionNormal), collision.ClosestPoint(transform.position));
            }
            else
            {
                // Get Effect for laser on initial shot
                gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                TakeDamage(laserBullet.damage);
                laserBullet.OnHit();
            }
        }
        else if (shotgunBullet != null && shotgunBullet.isEnemyShot)
        {
            if (isShielding && !IsFrontShot(collision.transform))
            {
                adaptiveShield.OnHit(collisionNormal, shotgunBullet.damage);
                gameManager.BeginEffect(GameConstants.EffectTypes.ShieldHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                shotgunBullet.OnShield(transform.TransformDirection(collisionNormal));
            }
            else
            {
                TakeDamage(shotgunBullet.damage);
                gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                shotgunBullet.OnHit();
            }
        }
        else if (normalBullet != null && normalBullet.isEnemyShot)
        {
            if (isShielding && !IsFrontShot(collision.transform))
            {
                adaptiveShield.OnHit(collisionNormal, normalBullet.damage);
                gameManager.BeginEffect(GameConstants.EffectTypes.ShieldHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                normalBullet.OnShield(transform.TransformDirection(collisionNormal));
            } else
            {
                TakeDamage(normalBullet.damage);
                gameManager.BeginEffect(GameConstants.EffectTypes.BulletHit, collisionPoint, collisionNormal).transform.SetParent(transform);
                normalBullet.OnHit();
            }
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void ToggleShield(bool shieldOn)
    {
        isShielding = shieldOn;
        if (isShielding)
        {
            shieldScript.TurnOnShield();
        } else
        {
            shieldScript.TurnOffShield();
        }
        
        adaptiveShield.shieldOn = isShielding;
    }

    // BUTTON PRESSES
    public void ShieldOn()
    {
        if(energy > 0f)
        {
            ToggleShield(true);
        }
    }
    public void ShieldOff()
    {
        ToggleShield(false);
    }

    internal void OnRecharge()
    {
        if (energy + GameConstants.RechargeGain > GameConstants.maxEnergy)
        {
            energy = GameConstants.maxEnergy;
        }
        else
        {
            energy += GameConstants.RechargeGain;
        }
    }

    internal void OnFire()
    {
        if(!isShielding)
        {
            FireCannons();
        }
    }
}
