using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public PlayerMovement playerController;
    
    public AudioSource audioSource;

    public CheckRep checkRepScript;

    public bool isPaused;
    private int currentState = 0;
    private int currentRepsMax = 4;
    private float stateDuration = 15f;
    private float stateTimer = 0f;

    public int optionalStateTrigger = 0;
    private int maxStates = 10;
    private float forwardInput;


    // Start is called before the first frame update
    void Start()
    {
        checkRepScript = GetComponent<CheckRep>();
        audioSource.Play();
        isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case 1:
                activateCalibrationHints();
                Debug.Log("HERE 1");
                break;
            case 2:
                activateStartMenu();
                Debug.Log("HERE 2");
                playerController.setStarted();
                GameManager.instance.SetForwardInput(1);
                break;
            case 3:
                activatePauseMenu();
                Debug.Log("HERE 3");
                break;
            case 4:
                activateGameOver();
                Debug.Log("HERE 4");
                break;
        }
        if (currentState == 2)
        {
            GameManager.instance.SetForwardInput(1);
        } else
        {
            GameManager.instance.SetForwardInput(0);
        }
    }

    void activateCalibrationHints() 
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(true);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);

    }

    void activatePauseMenu()
    {
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);

    }

    void activateStartMenu()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(true);
    }

    void activateGameOver()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        calibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);
    }

    void activateRepCount()
    {
        repCounterVisual.SetActive(true);
        holdTimerVisual.SetActive(false);
    }
    
    void activateHoldTimer()
    {
        holdTimerVisual.SetActive(true);
        repCounterVisual.SetActive(false);
    }


    public void reverseState()
    {
        currentState--;
        Debug.Log("CURRENT STATE:" + currentState.ToString());
    }

    public void nextState()
    {
        currentState++;
        Debug.Log("CURRENT STATE:" + currentState.ToString());
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
