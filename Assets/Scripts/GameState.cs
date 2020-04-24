﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
/**
 * This class is attached to the GameAction scene's controller to handle various aspects
 * of the game.
 */

public class GameState : MonoBehaviour {
    private bool gamePaused;
    private int level;
    private int lives;

    public GameObject ground;
    public GameObject gameMenuPanel;
    public GameObject gameOverlay;
    public Text livesLeftText;
    public KeyCode pauseKey;

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

    void Start () {
        InitializeVariables();
    }

    void Update () {
        if (Input.GetKeyDown(pauseKey)) {
            GamePaused = !GamePaused;
        }
    }

    public void InitializeVariables() {
        GamePaused = false;
        Level = 1;
        Lives = 40;
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
}
