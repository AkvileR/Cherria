using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowBerry : MonoBehaviour
{
    public GameObject spriteMaskPrefab;
    public GameObject visualEffectPrefab;
    public int revealDuration;
    private bool hit = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player" && !hit)
        {
            hit = true;
            GameObject spriteMask = Instantiate(spriteMaskPrefab, transform.position, transform.rotation);
            GameObject visualEffect = Instantiate(visualEffectPrefab, transform.position, transform.rotation);
            Destroy(spriteMask, revealDuration);
            Destroy(visualEffect, revealDuration);
            Destroy(gameObject);
        }

        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
}
