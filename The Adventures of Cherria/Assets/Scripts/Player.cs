using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    [Header("Health")]
    public int maxHealth = 20;
    public int currentHealth;
    public HealthBar healthBar;
    public int fallDeathThreshold;
    private bool alive = true;

    [Header("Special Abilities")]
    public GameObject revealingItemPrefab;
    public int revealingItemSpeed;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        alive = true;
    }

    void Update()
    {
        if (!alive)
        {
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            AudioManager.PlayJump();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))
        {
            ShootRevealingItem();
        }

        if (transform.position.y < fallDeathThreshold)
        {
            StartCoroutine(RestartLevel());
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Instakill")
        {
            StartCoroutine(RestartLevel());
        }
    }

    void ShootRevealingItem()
    {
        GameObject revealingItem = Instantiate(revealingItemPrefab, transform.position, transform.rotation);
        Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * revealingItemSpeed;
        revealingItem.GetComponent<Rigidbody2D>().velocity = dir;
    }

    IEnumerator RestartLevel()
    {
        alive = false;
        Color nulifiedAlpha = gameObject.GetComponent<SpriteRenderer>().color;
        nulifiedAlpha.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = nulifiedAlpha;
        
        AudioManager.PlayDie();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

