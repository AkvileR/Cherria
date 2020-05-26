using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    [Header("Health")]
    public int maxHealth;
    private int currentHealth;
    public int fallDeathThreshold;
    private bool alive = true;

    [Header("UI")]
    public Slider healthSlider;
    public float healthLerpSpeed;
    public Image projectileImage;
    public TextMeshProUGUI projectileAmmoText;

    [Header("Special Abilities")]
    public PlayerProjectile[] projectiles;
    private int activeProjectile;
    private int projectilesAcquired = 2;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
        alive = true;

        foreach (PlayerProjectile pp in projectiles)
        {
            pp.currentAmmo = pp.ammo;
        }

        ChangeProjectileUI();
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * healthLerpSpeed);

        if (!alive)
        {
            return;
        }

        if (transform.position.y < fallDeathThreshold)
        {
            TakeDamage(currentHealth);
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(RestartLevel());
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            AudioManager.PlayJump();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            activeProjectile++;
            ChangeProjectileUI();
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            activeProjectile--;
            ChangeProjectileUI();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ShootProjectile();
        }

        if (Input.GetButtonDown("Esc"))
        {
            SceneManager.LoadScene(4);
        }
    }
   

    void FixedUpdate()
    {
        if (!alive)
        {
            return;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

   
    #region Health and health UI

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (damage > 0)
        {
            AudioManager.PlayTakeDamage();
        }
    }

    void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Instakill":
                TakeDamage(currentHealth);
                break;
            case "Next Level Door":
                //foreach (PlayerProjectile pp in projectiles)
                //{
                //    pp.ammo = pp.currentAmmo;
                //}
                SceneManager.LoadScene(4);
                break;
        }
    }

    public void GetAmmo(int projectileNo, int amount)
    {
        projectiles[projectileNo].currentAmmo += amount;
    }

    void ChangeProjectileUI()
    {
        if (projectilesAcquired == 0)
        {
            projectileImage.gameObject.SetActive(false);
            activeProjectile = 0;
        }
        else
        {
            if (activeProjectile == -1)
            {
                activeProjectile = projectilesAcquired - 1;
            }
            else if (activeProjectile == projectilesAcquired)
            {
                activeProjectile = 0;
            }

            projectileImage.gameObject.SetActive(true);
            projectileImage.sprite = projectiles[activeProjectile].sprite;
            projectileAmmoText.text = projectiles[activeProjectile].currentAmmo.ToString();
        }
    }

    void ShootProjectile()
    {
        if (projectilesAcquired != 0 && projectiles[activeProjectile].currentAmmo > 0)
        {
            GameObject projectile = Instantiate(projectiles[activeProjectile].prefab, transform.position, transform.rotation);
            Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * projectiles[activeProjectile].flyingSpeed;
            projectile.GetComponent<Rigidbody2D>().velocity = dir;
            projectiles[activeProjectile].currentAmmo--;
            ChangeProjectileUI();
        }        
    }

    IEnumerator RestartLevel()
    {
        alive = false;
        Color nulifiedAlpha = gameObject.GetComponent<SpriteRenderer>().color;
        nulifiedAlpha.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = nulifiedAlpha;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        AudioManager.PlayDie();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

