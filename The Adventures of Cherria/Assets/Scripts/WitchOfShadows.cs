using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitchOfShadows : Enemy
{
    [Header("General")]
    public Slider healthSlider;
    public float healthLerpSpeed;
    private Material material;
    private Transform player;
    public float minPositionY;

    [Header("Teleportation")]
    public float fadingSpeed;
    private float currentFadingTime;
    private bool fadingIn;
    private bool fadingOut;

    public float minTeleportationCooldown;
    public float maxTeleportationCooldown;
    private float teleportationCountdown;
    public float maxTeleportationDistance;

    [Header("Shadow Ball")]
    public GameObject shadowBallPrefab;
    public Transform shadowBallSpawnPoint;
    public float shadowBallSpeed;
    public int shadowBallDamage;
    public float shadowBallChargeTime;

    public float minShootingCooldown;
    public float maxShootingCooldown;
    private float shootingCountdown;

    [Header("Darkness")]
    public GameObject darknessPostProcessingVolume;
    public float minDarknessCooldown;
    public float maxDarknessCooldown;
    public float darknessDuration;
    private float darknessCountdown;
    private float darknessTimeLeft;

    [Header("Darkness")]
    public GameObject zombieBroccoliPrefab;
    public Transform zombieSummoningRayPoint;
    public float minSummoningCooldown;
    public float maxSummoningCooldown;
    private float summoningCountdown;

    public override void Start()
    {
        base.Start();

        material = gameObject.GetComponent<SpriteRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        teleportationCountdown = Random.Range(minTeleportationCooldown, maxTeleportationCooldown);
        shootingCountdown = Random.Range(minShootingCooldown, maxShootingCooldown);
        darknessCountdown = Random.Range(minDarknessCooldown, maxDarknessCooldown);
        summoningCountdown = Random.Range(minSummoningCooldown, maxSummoningCooldown);

        darknessPostProcessingVolume.SetActive(false);
        fadingIn = fadingOut = false;
    }
    
    public override void Update()
    {
        base.Update();

        healthSlider.value = Mathf.Lerp(healthSlider.value, healthPoints, Time.deltaTime * healthLerpSpeed);

        if (transform.position.y < minPositionY)
        {
            Vector2 pos = transform.position;
            pos.y = minPositionY;
            transform.position = pos;
        }

        #region Teleportation
        if (!fadingIn && !fadingOut)
        {
            if (teleportationCountdown <= 0)
            {
                teleportationCountdown = Random.Range(minTeleportationCooldown, maxTeleportationCooldown);

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
            shootingCountdown = Random.Range(minShootingCooldown, maxShootingCooldown);
            ShootShadowBall();
        }
        else
        {
            shootingCountdown -= Time.deltaTime;
        }
        #endregion

        #region Darkness
        if (darknessCountdown <= 0)
        {            
            if (darknessTimeLeft <= 0)
            {
                darknessPostProcessingVolume.SetActive(false);
                darknessCountdown = Random.Range(minDarknessCooldown, maxDarknessCooldown);
                darknessTimeLeft = darknessDuration;
            }
            else
            {
                darknessPostProcessingVolume.SetActive(true);
                darknessTimeLeft -= Time.deltaTime;
            }
        }
        else
        {
            darknessPostProcessingVolume.SetActive(false);
            darknessCountdown -= Time.deltaTime;
        }
        #endregion

        #region Zombie Summoning
        if (summoningCountdown <= 0)
        {
            summoningCountdown = Random.Range(minSummoningCooldown, maxSummoningCooldown);
            
            if (Physics2D.Raycast(zombieSummoningRayPoint.position, Vector2.down))
            {
                RaycastHit2D hit = Physics2D.Raycast(zombieSummoningRayPoint.position, Vector2.down);
                GameObject zombieBroccoli = Instantiate(zombieBroccoliPrefab, hit.point, Quaternion.identity);
                zombieBroccoli.transform.position += zombieBroccoli.transform.localScale;
            }
        }
        else
        {
            summoningCountdown -= Time.deltaTime;
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
