using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
/**
 * This class is attached to the controller in MainMenu scene.
 */

public class MainMenuState : MonoBehaviour {

    private enum MenuState {
        menu,
        instruction,
        scoreboard,
        credits
    }

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
        allPanels = new List<GameObject> { buttonPanel, instructionPanel, scoreboardPanel, creditsPanel };
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
        currentState = MenuState.scoreboard;
    }

    /**
     * Called when the "Credits" button is clicked".
     */
    public void ShowCredits() {
        Debug.Log("Showing the credits!");
        currentState = MenuState.scoreboard;
    }
}
