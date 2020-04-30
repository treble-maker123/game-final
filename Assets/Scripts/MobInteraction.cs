﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInteraction : MonoBehaviour {

    public float maxHealth = 100f;
    public float currentHealth;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Healthbar hb = this.transform.GetComponentInChildren<Healthbar>();
            hb.SetHealth(currentHealth);
        }
    }
}
