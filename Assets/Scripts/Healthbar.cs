using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    public Image healthImage;
    public float maxHealth;

    void Start () {
    }

    void Update () {
    }

    /**
     * Sets the number of health points left, with valid range from 0 to maxHealth.
     */
    public void SetHealth(float newHealth) {
        healthImage.fillAmount = (float) newHealth / (float) maxHealth;
    }

    /**
     * Sets the health value left in percentage, with valid range from 0.0 to 1.0;
     */
    public void SetHealthPct(float newHealth) {
        healthImage.fillAmount = newHealth;
    }
}
