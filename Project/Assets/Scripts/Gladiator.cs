using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gladiator : MonoBehaviour
{
    [Header("Attributes")]
    public float maxHealth;
    public float moveSpeed;
    public float dashSpeed;
    public float offensiveVelocity;

    [Header("HUDs")]
    [SerializeField] Button DashButton;
    // public Slider HealthBar { get; set; }

    public float currentHealth { get; set; }
    [HideInInspector] public bool isPowerDashing = false;
    [HideInInspector] public bool isGlued = false;
    [HideInInspector] public bool isAlive = true;

    [Header("iFrames")]
    public float iFrame;
    private float iFrameCD;

    [Header("Gladiator-only Particles")]
    public ParticleSystem dashingParticles;

    // Material / color related stuff
    private Material material;
    private Color originalColor;
    private Color hitColor;
    [HideInInspector] Color fullSpeedColor;

    [Header("Effects")]
    public CharacterEffects effects;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
    }

    void Start()
    {
        hitColor = Color.white;
    }

    void FixedUpdate()
    {
        if (iFrameCD > 0)
        {
            iFrameCD -= Time.deltaTime;
            material.SetColor("_EmissionColor", material.color + Color.white);
            material.EnableKeyword("_EMISSION");
            // Debug.Log("iframecd: " + iFrameCD);
        }
        else 
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    /*
    public void SetupHealthBar()
    {
        HealthBar.minValue = 0;
        HealthBar.maxValue = maxHealth;
        UpdateHealthBar();
    }
    */

    public void UpdateHealth(float latestHealth)
    {
        if (iFrameCD > 0) return;

        if (latestHealth == 0)
        {
            isAlive = false;
            effects.PlayPopSound();
            effects.PlayDeathParticles();
            Destroy(gameObject);
        }

        currentHealth = latestHealth;
        // UpdateHealthBar();
    }

    /*
    public void UpdateHealthBar()
    {
        HealthBar.value = currentHealth;
    }
    */

    public void StartIFrame()
    {
        iFrameCD = iFrame;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        Rigidbody gladiatorRb = GetComponent<Rigidbody>();

        // Debug.Log("gladiatorRb velocity magnitude: " + gladiatorRb.velocity.magnitude);
        // Debug.Log("gladiator offensive velocity: " + offensiveVelocity);

        if (collidedObject.tag == "Enemy" && 
            iFrameCD <= 0 &&
            gladiatorRb.velocity.magnitude <= (0.8 * offensiveVelocity))
            // ^ only receive damage when velocity less than 80% of offensiveVelocity
        {
            UpdateHealth(currentHealth - 1); // MAY NEED TO CHANGE TO OTHER VALUE
            StartIFrame();
        }

        if (collidedObject.tag == "EnemyProjectile")
        {
            Debug.Log("hit by projectile");
            // Destroy(collision.gameObject);

            if (iFrameCD <= 0)
            {
                UpdateHealth(currentHealth - 1); // MAY NEED TO CHANGE TO OTHER VALUE
                StartIFrame();
            }            
        }
    }
}
