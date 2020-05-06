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

                mob.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);

                mob.AddComponent<Rigidbody>();
                mob.AddComponent<SphereCollider>();

                mob.AddComponent<MobInteraction>();
                mob.GetComponent<MobInteraction>().maxHealth = 100f;
                break;
            case MobType.skeleton:
                mob = Instantiate(Resources.Load("Skeletons_demo/models/DungeonSkeleton_demo")) as GameObject;
                mob.name = "Skeleton";
                mob.transform.position = new Vector3(5f, 0.5f, 5f);

                mob.AddComponent<Rigidbody>();
                mob.AddComponent<SphereCollider>();
                mob.GetComponent<SphereCollider>().center = new Vector3(0f, 0.5f, 0f);

                GameObject healthbar = Instantiate(Resources.Load("Healthbar")) as GameObject;
                healthbar.name = "Healthbar";
                healthbar.transform.parent = mob.transform;
                healthbar.transform.localPosition = new Vector3(0f, 3f, 0f);

                mob.AddComponent<MobInteraction>();
                mob.GetComponent<MobInteraction>().maxHealth = 100f;
                break;
            default:
                return;
        }

        mob.tag = "Mobs";

        // Set as a child of the enmies GameObject
        mob.transform.parent = enemies.transform;

        // setup rigidbody
        mob.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationZ;
        mob.GetComponent<Rigidbody>().mass = 5;

        // Setup waypoint-following script
        mob.AddComponent<FollowWaypoint>();
        mob.GetComponent<FollowWaypoint>().waypoints = ground.waypointTransforms;
        mob.GetComponent<FollowWaypoint>().sceneController = ground.sceneController;
        mob.GetComponent<FollowWaypoint>().onEndReached +=
            ground.sceneController.GetComponent<GameState>().MobReachesDestination;

        // Setup collider
        mob.GetComponent<SphereCollider>().radius = 0.5f;

        Physics.IgnoreCollision(
                mob.GetComponent<Collider>(),
                player.GetComponent<Collider>());

        // Setup healthbar to have a reference of where the player is
        Healthbar hb = mob.GetComponentInChildren<Healthbar>();
        hb.player = player;

        mob.SetActive(true);
    }

    /**
     * Resets the counter for the number of mobs respawned for this iteration.
     */
    public void ResetCounter() {
        numSpawned = 0;
    }

    public enum MobType {
        mob1,
        skeleton
    }
}
