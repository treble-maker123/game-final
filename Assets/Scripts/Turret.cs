using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public static bool GamePaused = false;

    private Transform target;
    private GameObject targetEnemy;

    [Header("Attribute")]

    public float dmg = 5;
    public float range = 15f;
    public float fireRate = 3f;
    private float fireCountdown = 0f;

    public int dmgCost = 100;
    public int rngCost = 100;
    public int frCost = 100;

    [Header("Unity Setup")]

    public string enemyTag = "Mobs";
    public string enemyType;
    public Transform partToRotate;
    private float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

	// Use this for initialization
	void Start () {
        //Called twice a second
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
		
	}

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            if (enemy.name == enemyType)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        //Found an enemy
        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GamePaused) return;
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        Quaternion rotateLook = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, rotateLook, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0)
        {
            Debug.Log("Target Found!");
            Shoot();
            //if fireRate was 2f, we fire 2 bullets per second
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

	}

    void Shoot()
    {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target, dmg);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
