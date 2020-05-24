using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemy : Enemy
{
    public float movementSpeed;
    public float checkRayLength;
    public Transform leftCheck;
    public Transform rightCheck;
    public GameObject graphics;
    [HideInInspector]
    public SpriteRenderer sr;
    public int damage;
    public int playerJumpForce;

    public override void Start()
    {
        base.Start();

        sr = graphics.GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();

        transform.Translate(Vector2.right * movementSpeed * (sr.flipX ? -1 : 1) * Time.deltaTime);

        RaycastHit2D leftHit = Physics2D.Raycast(leftCheck.position, Vector2.left, checkRayLength);
        RaycastHit2D leftDownHit = Physics2D.Raycast(leftCheck.position, Vector2.down, checkRayLength);
        RaycastHit2D rightHit = Physics2D.Raycast(rightCheck.position, Vector2.right, checkRayLength);
        RaycastHit2D rightDownHit = Physics2D.Raycast(rightCheck.position, Vector2.down, checkRayLength);

        if (leftDownHit.collider == null)
        {
            sr.flipX = false;
        }
        else if (rightDownHit.collider == null)
        {
            sr.flipX = true;
        }
        else if (leftHit.collider != null && leftHit.collider.tag != "Player")
        {
            sr.flipX = false;
        }
        else if (rightHit.collider != null && rightHit.collider.tag != "Player")
        {
            sr.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            col.collider.GetComponent<Player>().TakeDamage(damage);
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            AudioManager.PlayBroccoliDie();
            Die();
        }
    }
}
