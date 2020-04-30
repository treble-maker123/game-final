using UnityEngine;
using System.Collections;
/**
 * This class contains the behavior associated with the spawning tile and is attached
 * to the starting tile in Ground.cs
 */

public class SpawnPoint : MonoBehaviour {

    private GameObject enemies;
    private int numSpawned;

    public GameObject player;
    public Ground ground;

    public static bool GamePaused = false;
    public static float SpawnInterval = 2.0f;
    public static int NumToSpawn = 10;
    public static MobType TypeToSpawn = MobType.mob1;

    /**
     * The number of mobs spawned in the current iteration. Call reset
     * to reset the counter;
     */
    public int NumSpawned {
        get { return numSpawned; }
    }

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
        // if (Input.GetKeyDown(KeyCode.Space)) {
            // IEnumerator spawnCoroutine = StartSpawn(5, MobType.mob1, 2.0f);
            // StartCoroutine(spawnCoroutine);
        // }
    }

    /**
     * For coroutines to start spawning mobs.
     */
    public IEnumerator StartSpawn(int numToSpawn, MobType type, float interval) {
        while (!SpawnPoint.GamePaused && numSpawned < numToSpawn) {
            SpawnMob(type);
            numSpawned++;
            yield return new WaitForSeconds(interval);
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

    /**
     * Resets the counter for the number of mobs respawned for this iteration.
     */
    public void ResetCounter() {
        numSpawned = 0;
    }

    public enum MobType {
        mob1
    }
}
