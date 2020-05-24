using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPlatformEnemy : PlatformEnemy
{
    public float chargeMovementSpeed;
    private float regularMovementSpeed;
    public float visionDistance;
    public Transform leftEyeCheck;
    public Transform rightEyeCheck;

    public override void Start()
    {
        base.Start();

        regularMovementSpeed = movementSpeed;
    }
    
    public override void Update()
    {
        base.Update();

        RaycastHit2D forwardHit;

        if (sr.flipX) // left
        {
            forwardHit = Physics2D.Raycast(leftEyeCheck.position, Vector2.left, visionDistance);
        }
        else // right
        {
            forwardHit = Physics2D.Raycast(rightEyeCheck.position, Vector2.right, visionDistance);
        }

        if (forwardHit.collider != null && forwardHit.collider.CompareTag("Player"))
        {
            movementSpeed = chargeMovementSpeed;
        }
        else
        {
            movementSpeed = regularMovementSpeed;
        }
    }
}
