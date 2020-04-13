using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Health UI")]
    public Slider healthSlider;
    public float lerpSpeed;

    [Header("Special Abilities")]
    public GameObject glowBerryPrefab;
    public int glowBerryFlyingSpeed;
    private bool glowBerryAbility = false;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
        alive = true;
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * lerpSpeed);

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

        if (glowBerryAbility && (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) )
        {
            ShootGlowBerry();
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
            case "Glow Berry":
                glowBerryAbility = true;
                Destroy(col.gameObject);
                break;
            case "Next Level Door":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

    void ShootGlowBerry()
    {
        GameObject revealingItem = Instantiate(glowBerryPrefab, transform.position, transform.rotation);
        Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * glowBerryFlyingSpeed;
        revealingItem.GetComponent<Rigidbody2D>().velocity = dir;
    }

    IEnumerator RestartLevel()
    {
        alive = false;
        Color nulifiedAlpha = gameObject.GetComponent<SpriteRenderer>().color;
        nulifiedAlpha.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = nulifiedAlpha;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        AudioManager.PlayDie();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

