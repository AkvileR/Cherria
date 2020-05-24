using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueberry : MonoBehaviour
{
    public GameObject spriteMaskPrefab;
    public GameObject visualEffectPrefab;
    public int revealDuration;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject spriteMask = Instantiate(spriteMaskPrefab, transform.position, transform.rotation);
        GameObject visualEffect = Instantiate(visualEffectPrefab, transform.position, transform.rotation);
        Destroy(spriteMask, revealDuration);
        Destroy(visualEffect, revealDuration);
        Destroy(gameObject);

        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
}
