using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
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
    private Difficulty difficulty;
    private SpawnPoint spawn;

    private bool building;
    private bool spawning;

    public Camera arialView;
    public GameObject ground;
    public GameObject gameMenuPanel;
    public GameObject difficultyPanel;
    public GameObject tipsPanel;
    public GameObject scoreBoardInputPanel;
    public GameObject gameOverlay;
    public GameObject mobGroup;
    public GameObject player;
    public Text tipsHeader;
    public Text tipsText;
    public Text livesLeftText;
    public Text goldText;
    public Text levelText;
    public Text scoreText;
    public KeyCode pauseKey;

    public InputField playerName;
    public Button scoreBoardSubmit;

    // countdown variables
    private int buildCountDown;
    private int tallyCountDown;

    // variables for tuning the game
    public static readonly int TotalLives = 40;
    public static readonly int StartingGold = 200;
    public static readonly int BuildTime = 10;
    public static readonly int TallyTime = 2;
    public static readonly int MobsPerWave = 1;
    public static readonly float MobSpawnInterval = 2.0f;
    public static readonly float MobStartingHealth = 100f;

    // game stats;
    private int score;
    private int mobsKilled;

    public bool GamePaused {
        get { return gamePaused; }
        set {
            gamePaused = value;
            gameMenuPanel.SetActive(gamePaused);
            gameOverlay.SetActive(!gamePaused);
            tipsPanel.SetActive(!gamePaused);

            FollowWaypoint.GamePaused = gamePaused;
            SpawnPoint.GamePaused = gamePaused;
            Turret.GamePaused = gamePaused;

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

    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = score.ToString();
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

    public int MobsKilled {
        get { return mobsKilled; }
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

        Score = 0;
        Level = 1;
        Lives = TotalLives;
        Gold = StartingGold;

        mobsKilled = 0;
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

        scoreBoardSubmit.enabled = !string.IsNullOrEmpty(playerName.text);

        if (lives <= 0) {
            stage = Stage.GameOver;
        }

        switch(stage) {
            case Stage.Selection:
                gameMenuPanel.SetActive(false);
                gameOverlay.SetActive(false);
                tipsPanel.SetActive(false);
                scoreBoardInputPanel.SetActive(false);
                difficultyPanel.SetActive(true);
                arialView.enabled = false;
                player.GetComponent<PlayerUse>().WeaponSwap(0);
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
                tipsText.text = (MobsPerWave - spawn.NumSpawned) + " enemies left to spawn.";

                if (!spawning) {
                    SpawnPoint.MobType type =
                        (SpawnPoint.MobType)
                            UnityEngine.Random.Range(0, (int) SpawnPoint.MobType.count);

                    float health = MobStartingHealth;
                    switch(difficulty) {
                        case Difficulty.easy:
                            health *= (float) Math.Pow(1.05f, Level);
                            break;
                        case Difficulty.medium:
        Debug.Log("Mob killed!");
                            health *= (float) Math.Pow(1.10f, Level);
                            break;
                        case Difficulty.hard:
                            health *= (float) Math.Pow(1.15f, Level);
                            break;
                    }

                    IEnumerator spawnCoroutine = spawn.StartSpawn(MobsPerWave, type, MobSpawnInterval, MobStartingHealth);
                    StartCoroutine(spawnCoroutine);
                    spawning = true;
                }

                if (spawning && spawn.NumSpawned >= MobsPerWave) {
                    spawning = false;
                    stage = Stage.Battle;
                }
                break;
            case Stage.Battle:
                tipsHeader.text = "Stop The Invasion!";
                tipsText.text = NumMobsActive + " enemies left on the ground.";

                if (NumMobsActive <= 0) {
                    AdvanceLevel();
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
                }
                break;
            case Stage.GameOver:
                tipsHeader.text = "Game Over!";
                tipsText.text = "";

                // TODO: Calculate score
                Score = CalculateScore(difficulty, MobsKilled, Level);

                gameMenuPanel.SetActive(false);
                gameOverlay.SetActive(false);
                tipsPanel.SetActive(false);
                scoreBoardInputPanel.SetActive(true);
                difficultyPanel.SetActive(false);

                DisableFPS();
                break;
            default:
                Debug.LogError("Unrecognized stage: " + stage.ToString());
                break;
        }
    }

    int CalculateScore(Difficulty d, int numMobsKilled, int finalLevel) {
        float score = numMobsKilled + 10 * finalLevel;

        switch (d) {
            case Difficulty.easy:
                score *= 1.0f;
                break;
            case Difficulty.medium:
                score *= 1.4f;
                break;
            case Difficulty.hard:
                score *= 1.8f;
                break;
        }

        return (int) score;
    }

    /**
     * Countdown coroutine for the building phase.
     */
    IEnumerator BuildCountDown() {
        while (buildCountDown > 0) {
            if (GamePaused) {
                yield return new WaitForSeconds(0.1f);
            } else {
                buildCountDown -= 1;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    IEnumerator TallyCountDown() {
        while (tallyCountDown > 0) {
            if (GamePaused) {
                yield return new WaitForSeconds(0.1f);
            } else {
                tallyCountDown -= 1;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void DisableFPS() {
        player.SetActive(false);
        arialView.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableFPS() {
        player.SetActive(true);
        arialView.enabled = true;
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
     * This method is called when a mob is killed.
     */
    public void MobKilled() {
        mobsKilled += 1;
    }

    /**
     * Method called by the resume button in game menu.
     */
    public void Resume() {
        GamePaused = false;
    }

    public void SubmitScore() {
        PlayerPrefs.SetInt("score", Score);
        PlayerPrefs.SetString("name", playerName.text);
        PlayerPrefs.SetString("showSB", "true");

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SetEasy() {
        difficulty = Difficulty.easy;
        SetDifficulty();
    }

    public void SetMedium() {
        difficulty = Difficulty.medium;
        SetDifficulty();
    }

    public void SetHard() {
        difficulty = Difficulty.hard;
        SetDifficulty();
    }

    private void SetDifficulty() {
        ground.GetComponent<Ground>().GenerateMaze(difficulty);
        spawn = ground.GetComponentInChildren<SpawnPoint>();
        stage = Stage.Build;
        buildCountDown = BuildTime;

        difficultyPanel.SetActive(false);
        tipsPanel.SetActive(true);
        gameOverlay.SetActive(true);
        gameMenuPanel.SetActive(false);
        scoreBoardInputPanel.SetActive(false);

        player.transform.position = new Vector3(10f, 1.3f, 10f);
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
        Tally, // points are tallied during this timi
        GameOver
    }
}
