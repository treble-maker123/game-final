using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
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
    private Difficulty mazeDifficulty;
    private SpawnPoint spawn;

    private bool building;
    private bool spawning;

    public GameObject ground;
    public GameObject gameMenuPanel;
    public GameObject difficultyPanel;
    public GameObject gameOverlay;
    public GameObject mobGroup;
    public GameObject player;
    public Text tipsHeader;
    public Text tipsText;
    public Text livesLeftText;
    public Text goldText;
    public Text levelText;
    public KeyCode pauseKey;

    // countdown variables
    private int buildCountDown;
    private int tallyCountDown;

    // variables related to spawning mobs
    public int numMobsToSpawn;
    public float spawnInterval;
    public SpawnPoint.MobType mobType;

    // variables for tuning the game
    public static readonly int TotalLives = 40;
    public static readonly int StartingGold = 200;
    public static readonly int BuildTime = 5;
    public static readonly int TallyTime = 5;

    public bool GamePaused {
        get { return gamePaused; }
        set {
            gamePaused = value;
            gameMenuPanel.SetActive(gamePaused);

            if (gamePaused) {
                DisableFPS();
            } else {
                EnableFPS();
            }
        }
    }

    public int Level {
        get { return level; }
        set {
            level = value;
            levelText.text = level.ToString();
        }
    }

    public int Lives {
        get { return lives; }
        set {
            lives = value;
            livesLeftText.text = lives.ToString();

            if (lives > (lives / 3) * 2) {
                livesLeftText.color = Color.green;
            } else if (lives > (lives / 3)) {
                livesLeftText.color = new Color(1.0f, 1.0f, 0.0f);
            } else {
                livesLeftText.color = Color.red;
            }
        }
    }

    public int Gold {
        get { return gold; }
        set {
            gold = value;
            goldText.text = gold.ToString();

            if (gold > 0) {
                goldText.color = Color.green;
            } else {
                goldText.color = Color.red;
            }
        }
    }

    public Stage CurrentStage {
        get { return stage; }
    }

    public int SecondsLeftToBuild {
        get { return buildCountDown; }
    }

    public int NumMobsActive {
        get { return mobGroup.transform.childCount; }
    }

    public void InitializeVariables() {
        GamePaused = false;
        spawning = false;
        building = false;

        Level = 1;
        Lives = TotalLives;
        Gold = StartingGold;

        numMobsToSpawn = 1;
        mobType = SpawnPoint.MobType.skeleton;
        spawnInterval = 2.0f;
    }

    void Start () {
        InitializeVariables();
        DisableFPS();

        stage = Stage.Selection;
    }

    void Update () {
        if (Input.GetKeyDown(pauseKey) && stage != Stage.Selection) {
            GamePaused = !GamePaused;
        }

        SpawnPoint.GamePaused = GamePaused;

        if (GamePaused) {
            // TODO: Stop updating
        }

        switch(stage) {
            case Stage.Selection:
                break;
            case Stage.Build:
                tipsHeader.text = "Fortify Your Defenses";
                if (buildCountDown > 0) {
                    tipsText.text = buildCountDown + " seconds left to build.";
                } else {
                    tipsText.text = "";
                }

                if (!building) {
                    StartCoroutine("BuildCountDown");
                    building = true;
                }

                if (building && buildCountDown <= 0) {
                    building = false;
                    stage = Stage.Spawn;
                    spawn.ResetCounter();
                }

                break;
            case Stage.Spawn:
                tipsHeader.text = "Enemies Spawning!";
                tipsText.text = (numMobsToSpawn - spawn.NumSpawned) + " enemies left to spawn.";

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

                if (spawning && spawn.NumSpawned >= numMobsToSpawn) {
                    spawning = false;
                    stage = Stage.Battle;
                }
                break;
            case Stage.Battle:
                tipsHeader.text = "Stop The Invasion!";
                tipsText.text = NumMobsActive + " enemies left on the ground.";

                if (NumMobsActive <= 0) {
                    stage = Stage.Tally;
                    tallyCountDown = TallyTime;
                    StartCoroutine("TallyCountDown");
                }
                break;
            case Stage.Tally:
                tipsHeader.text = "Round Over!";
                tipsText.text = "Get ready for the next wave! " + tallyCountDown + " seconds left.";

                if (tallyCountDown <= 0) {
                    stage = Stage.Build;
                    AdvanceLevel();
                }
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

    IEnumerator TallyCountDown() {
        while (!GamePaused && tallyCountDown > 0) {
            tallyCountDown -= 1;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void DisableFPS() {
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableFPS() {
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        Lives -= 1;
    }

    /**
     * Method called by the resume button in game menu.
     */
    public void Resume() {
        GamePaused = false;
    }

    public void SetEasy() {
        mazeDifficulty = Difficulty.easy;
        SetDifficulty();
    }

    public void SetMedium() {
        mazeDifficulty = Difficulty.medium;
        SetDifficulty();
    }

    public void SetHard() {
        mazeDifficulty = Difficulty.hard;
        SetDifficulty();
    }

    private void SetDifficulty() {
        ground.GetComponent<Ground>().GenerateMaze(mazeDifficulty);
        spawn = ground.GetComponentInChildren<SpawnPoint>();
        stage = Stage.Build;
        buildCountDown = BuildTime;
        difficultyPanel.SetActive(false);

        player.transform.position = new Vector3(10f, 0f, 10f);
        EnableFPS();
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
        Selection, // select difficulty screen
        Build, // a window where the players can build towers
        Spawn, // mobs spawn during this time
        Battle, // this is between the end of spawning and when the last mob reaches destination
        Tally // points are tallied during this time
    }
}
