using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchOfShadows : MonoBehaviour
{
    public int health;
    [HideInInspector]
    public int currentHealth;
    private Material material;
    private Transform player;

    [Header("Teleportation")]
    public float fadingSpeed;
    private float currentFadingTime;
    private bool fadingIn;
    private bool fadingOut;

    public float minTeleportationTime;
    public float maxTeleportationTime;
    private float teleportationCountdown;
    public float maxTeleportationDistance;

    [Header("Shadow Ball")]
    public GameObject shadowBallPrefab;
    public Transform shadowBallSpawnPoint;
    public float shadowBallSpeed;
    public int shadowBallDamage;
    public float shadowBallChargeTime;

    public float minShootingTime;
    public float maxShootingTime;
    private float shootingCountdown;

    void Start()
    {
        material = gameObject.GetComponent<SpriteRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        teleportationCountdown = Random.Range(minTeleportationTime, maxTeleportationTime);
        shootingCountdown = Random.Range(minShootingTime, maxShootingTime);
        fadingIn = fadingOut = false;
    }
    
    void Update()
    {
        #region Teleportation
        if (!fadingIn && !fadingOut)
        {
            if (teleportationCountdown <= 0)
            {
                teleportationCountdown = Random.Range(minTeleportationTime, maxTeleportationTime);

                fadingOut = true;
                currentFadingTime = 1;
            }
            else
            {
                teleportationCountdown -= Time.deltaTime;
            }
        }

        if (fadingOut)
        {
            currentFadingTime -= Time.deltaTime * fadingSpeed;

            if (currentFadingTime >= 0)
            {
                material.SetFloat("_Fade", currentFadingTime);
            }
            else
            {
                Teleport();
                
                fadingOut = false;
            }
        }

        if (fadingIn)
        {
            currentFadingTime += Time.deltaTime * fadingSpeed;

            if (currentFadingTime < 1)
            {
                material.SetFloat("_Fade", currentFadingTime);
            }
            else
            {
                fadingIn = false;
            }
        }
        #endregion

        #region Shooting
        if (shootingCountdown <= 0)
        {
            shootingCountdown = Random.Range(minShootingTime, maxShootingTime);
            ShootShadowBall();
        }
        else
        {
            shootingCountdown -= Time.deltaTime;
        }
        #endregion
    }

    private void Teleport()
    {
        Vector3 change;

        do
        {
            float x = Random.Range(-maxTeleportationDistance, maxTeleportationDistance);
            float y = Random.Range(-maxTeleportationDistance, maxTeleportationDistance);
            change = new Vector3(x, y, 0);
        }
        while (change.magnitude > maxTeleportationDistance && (transform.position + change).y < 3); // Need to set boundaries on all sides
        
        transform.position += change;

        fadingIn = true;
        currentFadingTime = 0;
    }

    private void ShootShadowBall()
    {
        GameObject shadowBall = Instantiate(shadowBallPrefab, shadowBallSpawnPoint.position, Quaternion.Euler(0, 0, Random.Range(0, 2 * Mathf.PI)));
        // Need to do the charging
        shadowBall.GetComponent<HostileProjectile>().damage = shadowBallDamage;
        shadowBall.GetComponent<Rigidbody2D>().velocity = (player.position - shadowBall.transform.position).normalized * shadowBallSpeed;
    }
}
