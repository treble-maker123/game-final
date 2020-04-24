using UnityEngine;

public class Tile : MonoBehaviour {

    private int gridLocX;
    private int gridLocY;
    private bool isWaypoint;

    public GameObject waypoint;

    public int GridLocX {
        get { return gridLocX; }
        internal set { gridLocX = value; }
    }

    public int GridLocY {
        get { return gridLocY; }
        internal set { gridLocY = value; }
    }

    public bool IsWaypoint {
        get { return isWaypoint; }
        set {
            isWaypoint = value;

            if (isWaypoint) {
                waypoint = Instantiate(Resources.Load("Waypoint")) as GameObject;
                waypoint.name = "Waypoint-" + gridLocX + "," + gridLocY;
                waypoint.tag = "Waypoint";
                waypoint.transform.parent = transform;
                waypoint.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                waypoint.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
                waypoint.AddComponent<BoxCollider>();
                waypoint.GetComponent<BoxCollider>().size = new Vector3(0.01f, 1.5f, 0.01f);
                waypoint.SetActive(true);
            } else {
                Destroy(waypoint);
            }
        }
    }

    void Start () {

    }

    void Update () {

    }
}
