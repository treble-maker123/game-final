using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
/**
 * This class is attached to the GameAction scene's controller to handle various aspects
 * of the game.
 */

public class GameState : MonoBehaviour {
    private bool gamePaused;
    private int level;
    private int lives;
    private int gold;
    private Stage stage;
    private SpawnPoint spawn;

    private bool building;
    private bool spawning;

    public GameObject ground;
    public GameObject gameMenuPanel;
    public GameObject gameOverlay;
    public Text tipsHeader;
    public Text tipsText;
    public Text livesLeftText;
    public KeyCode pauseKey;

    // variables related to building
    public int secondsToBuild;
    private int buildCountDown;

    // variables related to spawning mobs
    public int numMobsToSpawn;
    public float spawnInterval;
    public SpawnPoint.MobType mobType;

    public int TotalLives = 40;
    public int StartingGold = 200;

    public bool GamePaused {
        get { return gamePaused; }
        set {
            gamePaused = value;
            gameMenuPanel.SetActive(gamePaused);
        }
    }

    public int Level {
        get { return level; }
        set {
            level = value;
        }
    }

    public int Lives {
        get { return lives; }
        set {
            lives = value;
            livesLeftText.text = lives.ToString();

            if (lives > (lives / 3) * 2) {
                livesLeftText.color = new Color(0.0f, 0.5f, 0.0f);
            } else if (lives > (lives / 3)) {
                livesLeftText.color = new Color(0.5f, 0.5f, 0.0f);
            } else {
                livesLeftText.color = new Color(0.5f, 0.0f, 0.0f);
            }
        }
    }

    public int Gold {
        get { return gold; }
        set { gold = value; }
    }

    public Stage CurrentStage {
        get { return stage; }
    }

    public int SecondsLeftToBuild {
        get { return buildCountDown; }
    }

    public void InitializeVariables() {
        GamePaused = false;
        spawning = false;
        building = false;

        Level = 1;
        Lives = TotalLives;
        Gold = StartingGold;

        secondsToBuild = 2;

        numMobsToSpawn = 10;
        mobType = SpawnPoint.MobType.skeleton;
        spawnInterval = 2.0f;
    }

    void Start () {
        InitializeVariables();
        spawn = ground.GetComponentInChildren<SpawnPoint>();

        // TEMPORARY, TODO: REMOVE
        stage = Stage.Build;
    }

    void Update () {
        if (Input.GetKeyDown(pauseKey)) {
            GamePaused = !GamePaused;
        }

        SpawnPoint.GamePaused = GamePaused;

        if (GamePaused) {
            // TODO: Stop updating
        }

        switch(stage) {
            case Stage.Build:
                tipsHeader.text = "Build Your Towers";
                if (buildCountDown > 0) {
                    tipsText.text = buildCountDown + " seconds left to build.";
                } else {
                    tipsText.text = "";
                }

                if (!building) {
                    Debug.Log("Starting build phase.");
                    buildCountDown = secondsToBuild;
                    StartCoroutine("BuildCountDown");
                    building = true;
                }

                if (building && buildCountDown <= 0) {
                    building = false;
                    stage = Stage.Spawn;
                }

                break;
            case Stage.Spawn:
                tipsHeader.text = "Enemies Spawning!";
                tipsText.text = (numMobsToSpawn - spawn.NumSpawned) + " enemies left to spawn.";

                SpawnPoint.SpawnInterval = spawnInterval;
                SpawnPoint.NumToSpawn = numMobsToSpawn;
                SpawnPoint.TypeToSpawn = mobType;

                if (!spawning) {
                    Debug.Log("Start spawning "
                            + numMobsToSpawn + " mobs of type "
                            + mobType.ToString() + " with an interval of "
                            + spawnInterval + " seconds.");
                    IEnumerator spawnCoroutine = spawn.StartSpawn(numMobsToSpawn,
                                                                    mobType,
                                                                    spawnInterval);
                    StartCoroutine(spawnCoroutine);
                    spawning = true;
                }
                break;
            case Stage.Battle:
                tipsHeader.text = "Stop The Invasion!";
                break;
            case Stage.Tally:
                tipsHeader.text = "Round Over!";
                break;
            default:
                Debug.LogError("Unrecognized stage: " + stage.ToString());
                break;
        }
    }

    /**
     * Countdown coroutine for the building phase.
     */
    IEnumerator BuildCountDown() {
        while (!GamePaused && buildCountDown > 0) {
            buildCountDown -= 1;
            yield return new WaitForSeconds(1.0f);
        }
    }

    /**
     * Setup the variables for the next level.
     */
    public void AdvanceLevel() {
        Level += 1;
    }

    /**
     * This method is called by the mob when it reaches its destination.
     */
    public void MobReachesDestination() {
        Debug.Log("One mob reached its destination!");
    }

    /**
     * Method called by the resume button in game menu.
     */
    public void Resume() {
        GamePaused = false;
    }

    /**
     * Method called by the quit button in game menu.
     */
    public void Quit() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public enum Difficulty {
        easy,
        medium,
        hard
    }

    /**
     * Denotes which stage the game is on right now
     */
    public enum Stage {
        Build, // a window where the players can build towers
        Spawn, // mobs spawn during this time
        Battle, // this is between the end of spawning and when the last mob reaches destination
        Tally // points are tallied during this time
    }
}
