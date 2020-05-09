using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : MonoBehaviour
{
    public static bool GamePaused = false;

    private Transform target;
    private GameObject targetEnemy;

    [Header("Attribute")]

    public float range = 15f;
    public int duration = 10;
    public float percentage = 0.8f;
    private float fireCountdown = 0f;
    public float fireRate = 3f;

    public int rngCost = 300;
    public int durCost = 300;
    public int perCost = 300;

    [Header("Unity Setup")]

    public string enemyTag = "Mobs";

    // Use this for initialization
    void Start()
    {
        //Called twice a second
        InvokeRepeating("SlowTarget", 0f, 0.5f);

    }

    void SlowTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range)
            {
                var speedComponent = enemy.GetComponent<FollowWaypoint>();
                if (!speedComponent.slowCheck)
                {
                    speedComponent.slowCheck = true;
                    speedComponent.SlowingMob(duration, percentage);
                }
            }
        }
    }
}
