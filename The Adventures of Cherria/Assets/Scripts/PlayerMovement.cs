using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    [Header("Health")]
    public int maxHealth = 20;
    public int currentHealth;
    public HealthBar healthBar;
    public int fallDeathThreshold;

    [Header("Animation")]
    public CharacterController2D controller;
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            AudioManager.PlayJump();
        }

        if (transform.position.y < fallDeathThreshold)
        {
            RestartLevel();
        }
    }
   
    void FixedUpdate()
    {
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
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

