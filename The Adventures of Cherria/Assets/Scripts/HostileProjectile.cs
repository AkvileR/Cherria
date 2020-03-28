using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileProjectile : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.GetComponent<Player>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
