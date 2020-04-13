using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    public float movementSpeed;
    public float checkRayLength;
    public Transform leftCheck;
    public Transform rightCheck;
    public GameObject graphics;
    private SpriteRenderer sr;
    private int dirMultiplier = 1;
    public int damage;
    public int playerJumpForce;

    private void Start()
    {
        sr = graphics.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * movementSpeed * dirMultiplier * Time.deltaTime);

        RaycastHit2D leftHit = Physics2D.Raycast(leftCheck.position, Vector2.left, checkRayLength);
        RaycastHit2D leftDownHit = Physics2D.Raycast(leftCheck.position, Vector2.down, checkRayLength);
        RaycastHit2D rightHit = Physics2D.Raycast(rightCheck.position, Vector2.right, checkRayLength);
        RaycastHit2D rightDownHit = Physics2D.Raycast(rightCheck.position, Vector2.down, checkRayLength);

        if (leftDownHit.collider == null)
        {
            dirMultiplier = 1;
            sr.flipX = false;
        }
        else if (rightDownHit.collider == null)
        {
            dirMultiplier = -1;
            sr.flipX = true;
        }
        else if (leftHit.collider != null && leftHit.collider.tag != "Player")
        {
            dirMultiplier = 1;
            sr.flipX = false;
        }
        else if (rightHit.collider != null && rightHit.collider.tag != "Player")
        {
            dirMultiplier = -1;
            sr.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            col.collider.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            Die();
        }
    }

    private void Die()
    {
        AudioManager.PlayBroccoliDie();
        Destroy(gameObject);
    }
}
