using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class PlayerProjectile : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public float flyingSpeed;
    public int ammo;
    [HideInInspector]
    public int currentAmmo;
    /*
    public float rechargeTime;
    private float rechargeTimeLeft;

    void Start()
    {
        //rechargeTimeLeft = rechargeTime;
    }

    void Update()
    {
        if (rechargeTime != 0)
        {
            if (rechargeTimeLeft < 0)
            {
                ammo++;
                rechargeTimeLeft = rechargeTime;
            }
            else
            {
                rechargeTimeLeft -= Time.deltaTime;
            }
        }
    }
    */
}
