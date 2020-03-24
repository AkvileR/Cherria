using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    public int movementSpeed;
    public float checkRayLength;
    public Transform leftCheck;
    public Transform rightCheck;
    public GameObject graphics;
    private SpriteRenderer sr;
    private int dirMultiplier = 1;

    private void Start()
    {
        sr = graphics.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * movementSpeed * dirMultiplier * Time.deltaTime);
        
        if (Physics2D.Raycast(leftCheck.position, Vector2.left, checkRayLength) || !Physics2D.Raycast(leftCheck.position, Vector2.down, checkRayLength))
        {
            dirMultiplier = 1;
            sr.flipX = false;
        }
        else if (Physics2D.Raycast(rightCheck.position, Vector2.right, checkRayLength) || !Physics2D.Raycast(rightCheck.position, Vector2.down, checkRayLength))
        {
            dirMultiplier = -1;
            sr.flipX = true;
        }
        /*
        Debug.DrawRay(leftCheck.position, Vector2.left, Color.cyan, checkRayLength);
        Debug.DrawRay(leftCheck.position, Vector2.down, Color.cyan, checkRayLength);
        Debug.DrawRay(rightCheck.position, Vector2.down, Color.cyan, checkRayLength);
        Debug.DrawRay(rightCheck.position, Vector2.right, Color.cyan, checkRayLength);
        */
    }
}
