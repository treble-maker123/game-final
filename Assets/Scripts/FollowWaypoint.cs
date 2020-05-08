using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/**
 * GameObjects with this behavior will follow the waypoints from Ground.cs
 */

public class FollowWaypoint : MonoBehaviour {

    private int currentPos;

    // waypoints that originates from Ground.cs and passed from SpawnPoint.cs
    public List<GameObject> waypoints;
    public float speed = 5.0f;
    public GameObject sceneController;

    // this method is invoked when
    public delegate void OnEndReached();
    public event OnEndReached onEndReached;

    //Slowing variables
    public static readonly float FullSpeed = 5.0f;
    public static readonly int OriginalDuration = 1;
    public bool slowCheck = false;
    private int slowCountDown;

    void Start () {
        currentPos = 0;
    }

    void Update () {
        if (currentPos < waypoints.Count) {
            GameObject currentWaypoint = waypoints[currentPos];
            Vector3 direction = currentWaypoint.transform.position - transform.position;
            float step = speed * Time.deltaTime;

            // update position
            direction /= direction.magnitude;
            Vector3 delta = direction * step;
            Vector3 newPos = transform.position;
            // don't update y
            newPos.x += delta.x;
            newPos.z += delta.z;
            transform.position = newPos;

            // update direction
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
            // only rotate around y
            Quaternion newRotation = Quaternion.LookRotation(newDir);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = newRotation;
        }
    }

    void OnCollisionEnter (Collision targetObj) {
        // if not a waypoint, skip!
        if (targetObj.gameObject.tag != "Waypoint")
            return;

        currentPos++;

        if (currentPos == waypoints.Count) {
            if (onEndReached != null) {
                onEndReached();
            }

            Destroy(gameObject);
        }
    }

    /**
     * Countdown coroutine for the slowing phase.
     */
    IEnumerator SlowCountDown()
    {
        while (slowCountDown > 0)
        {
            slowCountDown -= 1;
            yield return new WaitForSeconds(1.0f);
        }
        slowCheck = false;
    }

    public void SlowingMob(int duration, float percent)
    {
        Debug.Log("Slowed");
        float newSpeed = FullSpeed * percent;
        speed = newSpeed;
        slowCountDown = duration;
        IEnumerator slow = SlowCountDown();
        StartCoroutine(slow);
        speed = FullSpeed;
        slowCountDown = OriginalDuration;
    }
}
