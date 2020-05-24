using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPlayerProjectile : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            col.GetComponent<Enemy>().healthPoints -= damage;
        }

        Destroy(gameObject);
    }
}
