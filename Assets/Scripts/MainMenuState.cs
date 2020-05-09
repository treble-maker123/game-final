using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
/**
 * This class is attached to the controller in MainMenu scene.
 */

public class MainMenuState : MonoBehaviour {

    public Text firstPersonName;
    public Text firstPersonScore;
    public Text secondPersonName;
    public Text secondPersonScore;
    public Text thirdPersonName;
    public Text thirdPersonScore;
    public Text fourthPersonName;
    public Text fourthPersonScore;
    public Text fifthPersonName;
    public Text fifthPersonScore;
    public string[] names = new string[numPlayers];
    public int[] scores = new int[numPlayers];

    private static readonly string fileName = "score.txt";
    private static readonly int numPlayers = 5;

    private const string gameActionSceneName = "GameAction";
    private const string trainingGroundSceneName = "TrainingGround";

    private MenuState currentState;
    private List<GameObject> allPanels;

    // variables to be configured in unity
    public GameObject buttonPanel;
    public GameObject instructionPanel;
    public GameObject scoreboardPanel;
    public GameObject creditsPanel;
    // key to go back to the menu state
    public KeyCode backKey;

    void Start () {
        currentState = MenuState.menu;
        allPanels = new List<GameObject> {
            buttonPanel, instructionPanel, scoreboardPanel, creditsPanel
        };
    }

    void Update () {
        if (Input.GetKeyDown(backKey)) {
            currentState = MenuState.menu;
        }

        SyncUIState();
    }

    /**
     * This method loops through allPanels and set each one to inactive.
     */
     private void HideAllPanels() {
        allPanels.ForEach(p => p.SetActive(false));
     }

    /**
     * This method syncs the UI with the currentState variable.
     */
    private void SyncUIState() {
        HideAllPanels();

        switch (currentState) {
            case MenuState.menu:
                buttonPanel.SetActive(true);
                break;
            case MenuState.instruction:
                instructionPanel.SetActive(true);
                break;
            case MenuState.scoreboard:
                scoreboardPanel.SetActive(true);
                break;
            case MenuState.credits:
                creditsPanel.SetActive(true);
                break;
            default:
                Debug.LogError("Unrecognized menu state: " + currentState.ToString());
                break;
        }
    }

    private void SetupScoreFile() {
        if (File.Exists(fileName)) {
            string [] lines = File.ReadAllLines(fileName);
            for (int i = 0; i < numPlayers; i++) {
                string[] tokens = lines[i].Split(',');
                string name = tokens[0];
                int score = Int32.Parse(tokens[1]);
                names[i] = name;
                scores[i] = score;
            }
        } else {
            using (StreamWriter sw = File.CreateText(fileName)) {
                for (int i = 0; i < numPlayers; i++) {
                    names[i] = "-";
                    scores[i] = 0;
                    sw.WriteLine(names[i]+","+scores[i]);
                }
            }
        }
    }

    private void UpdateScoreBoard(int newScore, string newName) {
        int tempScore = 0;
        string tempName = "";

        // sort the scoreboard
        for (int i = numPlayers - 1; i >= 0; i--) {
            if (newScore > scores[i]) {
                if(i == numPlayers - 1) {
                    scores[i] = newScore;
                    names[i] = newName;
                } else {
                    tempScore = scores[i];
                    tempName = names[i];
                    scores[i] = scores[i+1];
                    names[i] = names[i+1];
                    scores[i+1] = tempScore;
                    names[i+1] = tempName;
                }
            } else {
                break;
            }
        }

        // save the update
        using (StreamWriter sw = File.CreateText(fileName)) {
            for (int i = 0; i < numPlayers; i++) {
                sw.WriteLine(names[i] + "," + scores[i]);
            }
        }

        UpdateScoreBoardUI();
    }

    private void UpdateScoreBoardUI() {
        firstPersonName.text = names[0];
        firstPersonScore.text = "" + scores[0];
        secondPersonName.text = names[1];
        secondPersonScore.text = "" + scores[1];
        thirdPersonName.text = names[2];
        thirdPersonScore.text = "" + scores[2];
        fourthPersonName.text = names[3];
        fourthPersonScore.text = "" + scores[3];
        fifthPersonName.text = names[4];
        fifthPersonScore.text = "" + scores[4];
    }

    // =========================================================
    // Button events
    // =========================================================

    /**
     * Called when the "Play" button is clicked.
     */
    public void PlayGame() {
        Debug.Log("Play button clicked, loading corresponding scene...");
        SceneManager.LoadScene(gameActionSceneName, LoadSceneMode.Single);
    }

    /**
     * Called when the "Training" button is clicked.
     */
    public void EnterTraining() {
        Debug.Log("Entering training ground!");
        SceneManager.LoadScene(trainingGroundSceneName, LoadSceneMode.Single);
    }

    /**
     * Called when the "Instruction" button is clicked.
     */
    public void ShowInstruction() {
        Debug.Log("Showing instruction menu!");
        currentState = MenuState.instruction;
    }

    /**
     * Called when the "Score Board" button is clicked.
     */
    public void ShowScoreBoard() {
        Debug.Log("Showing the score board!");
        SetupScoreFile();
        UpdateScoreBoardUI();
        currentState = MenuState.scoreboard;
    }

    /**
     * Called when the "Credits" button is clicked".
     */
    public void ShowCredits() {
        Debug.Log("Showing the credits!");
        currentState = MenuState.credits;
    }

    private enum MenuState {
        menu,
        instruction,
        scoreboard,
        credits
    }
}
