using System.Collections;
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
            //This will eventually somehow call the healthbar script to set the current health/percentage.
            //But right now, I cannot access the script by trying every way that I can think of.
            Healthbar hb = this.gameObject.GetComponent<Healthbar>();
            //hb.SetHealth(currentHealth);
            Debug.Log(hb);
        }
    }
}
