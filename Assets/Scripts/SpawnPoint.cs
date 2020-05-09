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

    /**
     * The number of mobs spawned in the current iteration. Call reset
     * to reset the counter;
     */
    public int NumSpawned {
        get { return numSpawned; }
    }

    // Use this for initialization
    void Start () {
        ResetCounter();
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
    public IEnumerator StartSpawn(int numToSpawn, MobType type, float interval, float health) {
        while (!SpawnPoint.GamePaused && numSpawned < numToSpawn) {
            if (GamePaused) {
                yield return new WaitForSeconds(0.1f);
            } else {
                SpawnMob(type, health);
                numSpawned++;
                yield return new WaitForSeconds(interval);
            }
        }
    }

    /**
     * This method will spawn a mob of the specific type on the tile.
     */
    public void SpawnMob(MobType type, float health) {
        GameObject mob;
        AnimationClip walkClip;

        // programmatically add a healthbar
        GameObject healthbar = Instantiate(Resources.Load("Healthbar")) as GameObject;
        healthbar.name = "Healthbar";

        switch(type) {
            case MobType.skeleton:
                mob = Instantiate(Resources.Load("Skeletons_demo/models/DungeonSkeleton_demo")) as GameObject;
                mob.name = "Ground";
                mob.transform.position = new Vector3(5f, 0.5f, 5f);

                mob.AddComponent<Rigidbody>();
                mob.AddComponent<BoxCollider>();
                // raise the collider so the skeleton is not levitated
                mob.GetComponent<BoxCollider>().center = new Vector3(0f, 1.1f, 0f);
                mob.GetComponent<BoxCollider>().size = new Vector3(0.7f, 2.1f, 0.7f);

                // fine-tune healthbar
                healthbar.transform.parent = mob.transform;
                healthbar.transform.localPosition = new Vector3(0f, 3f, 0f);

                // setup animation
                mob.AddComponent<Animation>();
                walkClip = Resources.Load<AnimationClip>("Skeletons_demo/animation/DS_onehand_walk");
                break;
            case MobType.stone:
                mob = Instantiate(Resources.Load("StoneMonster/StoneMonster")) as GameObject;

                mob.name = "Air";
                mob.transform.position = new Vector3(5f, 1.5f, 5f);

                mob.AddComponent<Rigidbody>();

                mob.AddComponent<BoxCollider>();
                mob.GetComponent<BoxCollider>().center = new Vector3(0f, 0.5f, 0f);
                mob.GetComponent<BoxCollider>().size = new Vector3(0.8f, 1.2f, 0.8f);

                // fine-tune healthbar
                healthbar.transform.parent = mob.transform;
                healthbar.transform.localPosition = new Vector3(0f, 1.8f, 0f);

                // setup animation
                mob.AddComponent<Animation>();
                walkClip = Resources.Load<AnimationClip>("StoneMonster/Animations/StoneMonster_Run");
                break;
            case MobType.orc:
                mob = Instantiate(Resources.Load("Orc/model/Orc_Wolfrider")) as GameObject;

                mob.name = "Ground";
                mob.transform.position = new Vector3(5f, 1.5f, 5f);

                mob.AddComponent<Rigidbody>();

                mob.AddComponent<BoxCollider>();
                mob.GetComponent<BoxCollider>().center = new Vector3(0f, 0.9f, 0f);
                mob.GetComponent<BoxCollider>().size = new Vector3(0.8f, 1.7f, 0.6f);

                healthbar.transform.parent = mob.transform;
                healthbar.transform.localPosition = new Vector3(0f, 2.4f, 0f);

                mob.AddComponent<Animation>();
                walkClip = Resources.Load<AnimationClip>("Orc/animation/Orc_wolfrider_03_run");
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
        Physics.IgnoreCollision(
                mob.GetComponent<Collider>(),
                player.GetComponent<Collider>());

        // kicking off animation
        walkClip.legacy = true;
        mob.GetComponent<Animation>().AddClip(walkClip, "walk");
        mob.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        mob.GetComponent<Animation>().Play("walk");

        // Setup healthbar to have a reference of where the player is
        Healthbar hb = mob.GetComponentInChildren<Healthbar>();
        hb.player = player;

        // Setting up mob interaction
        mob.AddComponent<MobInteraction>();
        mob.GetComponent<MobInteraction>().maxHealth = health;
        mob.GetComponent<MobInteraction>().onMobKilled +=
           ground.sceneController.GetComponent<GameState>().MobKilled;

        mob.SetActive(true);
    }

    /**
     * Resets the counter for the number of mobs respawned for this iteration.
     */
    public void ResetCounter() {
        numSpawned = 0;
    }

    public enum MobType {
        skeleton,
        stone,
        orc,
        count
    }
}
