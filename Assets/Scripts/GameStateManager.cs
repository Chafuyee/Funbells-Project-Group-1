using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject gameOverMenu;
    public GameObject calibrationHints;
    public GameObject repCounterVisual;
    public GameObject holdTimerVisual;
    
    public AudioSource audioSource;

    public CheckRep checkRepScript;

    public bool isPaused;
    private int currentState = 0;
    private int currentRepsMax = 4;
    private float stateDuration = 15f;
    private float stateTimer = 0f;

    public int optionalStateTrigger = 0;
    private int maxStates = 10;


    // Start is called before the first frame update
    void Start()
    {

        checkRepScript = GetComponent<CheckRep>();

        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(true);

        isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void TriggerPauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        stateTimer = 0f;
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        StartNextState();
    }

    void StartNextState()
    {
        currentState++;
        Debug.Log("State: " + currentState);

        optionalStateTrigger = 0; // Trigger to rep counter

        if (currentState % 2 == 0)
        {
            Debug.Log("State for Pushing Rock : Curl Exercise");
        }
        else
        {
            Debug.Log("State for resisting the rock : Hold Exercises");
        }
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        isPaused = false;
        StartNextState();
    }

    void EndGame()
    {
        Debug.Log("Game over! You've complete all 10 stages");
        gameOverMenu.SetActive(true);
        isPaused = true;
    }

    public void RestartGame()
    {
        gameOverMenu.SetActive(false);
        currentState = 0;
        StartGame();
    }
}
