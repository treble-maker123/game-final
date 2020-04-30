using UnityEngine;
/**
 * This class contains the behavior associated with the spawning tile and is attached
 * to the starting tile in Ground.cs
 */

public class SpawnPoint : MonoBehaviour {

    private GameObject enemies;

    public GameObject player;
    public Ground ground;

    // Use this for initialization
    void Start () {
        GameObject[] rootObjects = gameObject.scene.GetRootGameObjects();

        for (int i = 0; i < rootObjects.Length; i++) {
            // Find the MobGroup game object so we can spawn enemies under it
            if (rootObjects[i].name == "MobGroup") {
                enemies = rootObjects[i];
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnMob(MobType.mob1);
        }
    }

    /**
     * This method will spawn a mob of the specific type on the tile.
     */
    public void SpawnMob(MobType type) {
        GameObject mob;

        switch(type) {
            case MobType.mob1:
                mob = Instantiate(Resources.Load("Mob1")) as GameObject;
                mob.name = "Mob";
                mob.tag = "Mobs";
                // Set as a child of the enmies GameObject
                mob.transform.parent = enemies.transform;
                mob.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                // Add rigitbody
                mob.AddComponent<Rigidbody>();
                mob.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY
                    | RigidbodyConstraints.FreezeRotationX
                    | RigidbodyConstraints.FreezeRotationZ;
                // Setup waypoint-following script
                mob.AddComponent<FollowWaypoint>();
                mob.GetComponent<FollowWaypoint>().waypoints = ground.waypointTransforms;
                mob.GetComponent<FollowWaypoint>().sceneController = ground.sceneController;
                mob.GetComponent<FollowWaypoint>().onEndReached +=
                    ground.sceneController.GetComponent<GameState>().MobReachesDestination;
                // Setup healthbar to have a reference of where the player is
                Healthbar hb = mob.GetComponentInChildren<Healthbar>();
                hb.player = player;

                mob.AddComponent<MobInteraction>();
                mob.GetComponent<MobInteraction>().maxHealth = 100f;
                mob.SetActive(true);
                break;
            default:
                return;
        }
    }

    public enum MobType {
        mob1
    }
}
