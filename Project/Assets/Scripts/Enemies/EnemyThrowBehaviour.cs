using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowBehaviour : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] GameObject enemyObject;
    [SerializeField] GameObject signal;
    private Enemy enemy;

    [Header("Projectile")]
    public Transform projectileSpawnPosition;
    public GameObject projectilePrefab;
    public float projectileActiveTime;
    public float projectileSpeed;
    public float projectileReloadTime;

    [Header("Sound")]
    public AudioSource bowSound;

    private GameObject newProjectile;
    private Rigidbody newProjectileRb;
    private float projectileReloadTimeCd;

    void Awake()
    {
        projectileReloadTimeCd = projectileReloadTime * 2;
    }

    void Start()
    {
        enemy = enemyObject.GetComponent<Enemy>();
        signal.SetActive(false);
    }

    private void Update()
    {
        if (projectileReloadTimeCd < 0.5f && projectileReloadTimeCd > 0)
        {
            signal.SetActive(true);
        }
        else
        {
            signal.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyObject.transform.LookAt(other.gameObject.transform.position);

            if (projectileReloadTimeCd >= 0) projectileReloadTimeCd -= Time.deltaTime;

            if (projectileReloadTimeCd < 0)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        bowSound.Play();

        newProjectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, enemyObject.transform.rotation);
        newProjectile.transform.parent = transform.parent;
        newProjectileRb = newProjectile.GetComponent<Rigidbody>();
        newProjectileRb.AddForce(enemyObject.transform.forward * projectileSpeed, ForceMode.Impulse);

        Destroy(newProjectile, projectileActiveTime);

        projectileReloadTimeCd = projectileReloadTime;
    }
}
