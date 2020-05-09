using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInteraction : MonoBehaviour {

    public float maxHealth = 100f;
    public float currentHealth;

    public delegate void OnMobKilled();
    public event OnMobKilled onMobKilled;

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
            //Temp money add
            GameObject scene = GameObject.FindGameObjectWithTag("SC");
            scene.GetComponent<GameState>().Gold += 50;

            if (onMobKilled != null) {
                onMobKilled();
            }
        }
        else
        {
            Healthbar hb = this.transform.GetComponentInChildren<Healthbar>();
            hb.SetHealth(currentHealth);
        }
    }
}
