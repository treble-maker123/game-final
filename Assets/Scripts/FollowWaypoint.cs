using System.Collections.Generic;
using UnityEngine;
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

    void Start () {
        currentPos = 0;
    }

    void Update () {
        if (currentPos < waypoints.Count) {
            GameObject currentWaypoint = waypoints[currentPos];
            Vector3 direction = currentWaypoint.transform.position - transform.position;
            direction /= direction.magnitude;
            transform.position += direction * (speed * Time.deltaTime);
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
}
