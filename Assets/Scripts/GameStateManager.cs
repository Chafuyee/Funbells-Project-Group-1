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
    public GameObject holdCalibrationHints;
    public GameObject repCounterVisual;
    public GameObject holdTimerVisual;
    public GameObject curlSetVisual;
    public GameObject holdSetVisual;
    
    public GameObject handGestureTracking;
    public GameObject curlEndingShadow;
    public GameObject curlStartingShadow;
    public GameObject holdShadow;

    public bool repDetectionOn = true;
    public PlayerMovement playerController;
    
    public AudioSource audioSource;

    public CheckRep checkRepScript;

    public int stateReps;
    public bool isPaused;
    private int currentState = 0;
    private float stateDuration = 15f;
    private float stateTimer = 0f;

    public HoldTimer holdTimer;
    private float moveTimer = 0f;

    public int optionalStateTrigger = 0;
    private int maxStates = 10;
    private float forwardInput;


    // Start is called before the first frame update
    void Start()
    {
        checkRepScript = GetComponent<CheckRep>();
        isPaused = true;
        handGestureTracking.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTimer > 0) {
            GameManager.instance.SetForwardInput(1);
            moveTimer -= Time.deltaTime;
        } else if (moveTimer < 0) {
            moveTimer = 0;
            GameManager.instance.SetForwardInput(0);
        } else {
            GameManager.instance.SetForwardInput(0);
        }

        

        switch (currentState)
        {
            case 1: // Calibration step : CURLS
                activateCalibrationHints();
                repDetectionOn = false;
                break;
            case 2: // Calibration step : HOLDS
                activateHoldCalibrationHints();
                repDetectionOn = false;
                break;
            case 3: // Start Menu
                hideHoldShadows();
                //disableHandTracking();
                repDetectionOn = false;
                activateStartMenu(); //
                playerController.setStarted(); // Set XR Rig on Fixed Axis
                break;
            case 4: // Set 1s - Curl
                repDetectionOn = true;
                hideHoldShadows();
                activateCurlVisual();
                checkRepScript.activateRepCounter();
                activateRepCount();
                break;
            case 5: // Set 1s - Hold
                activateHoldTimer();
                //hideCurlShadows();
                activateHoldVisual();
                break;
            case 6: // PAUSE
                checkRepScript.deactivateRepCounter();
                activatePauseMenu();
                break;
            case 7: // Set 1n - Curl
                break;
            case 8: // Set 1n - Hold
                break;
            case 9: // PAUSE
                break;
            case 10: // Set 1b - Curl
                break;
            case 11: // Set 1b - Hold
                break;
            case 12: // PAUSE
                break;
            case 13: // Set 2s - Curl
                break;
            case 14: // Set 2s - Hold
                break;
            case 15: // PAUSE
                break;
            case 16: // Set 2n - Curl
                break;
            case 17: // Set 2n - Hold
                break;
            case 18: // PAUSE
                break;
            case 19: // Set 2b - Curl
                break;
            case 20: // Set 2b - Hold
                break;
            case 21: // PAUSE
                break;
            case 22: // Set 3s - Curl
                break;
            case 23: // Set 3s - Hold
                break;
            case 24: // PAUSE
                break;
            case 25: // Set 3n - Curl
                break;
            case 26: // Set 3n - Hold
                break;
            case 27: // PAUSE
                break;
            case 28: // Set 3b - Curl
                break;
            case 29: // Set 3b - Hold
                break;
            case 30: // PAUSE
                break;
            case 31:
                activateGameOver();
                break;
        }
    }

    private void generateNextState(string incrementCode)
    {
        float sizeInKg = float.Parse(incrementCode.Substring(0, 4));
        string visualRepresentation = incrementCode.Substring(4);
        if (visualRepresentation == "s")
        {
            // Pull small weight relative to sizeInKG
        } else if (visualRepresentation == "n")
        {
            // Pull normal weight relative to sizeInKg
        } else
        {
            // Pull big weight relative to sizeInKg
        }
    }

    public void incrementStateReps()
    {
        stateReps++;
    }

    public bool checkRepDetectionOn()
    {
        return repDetectionOn;
    }

    public void incrementMoveTmr() {
        moveTimer = moveTimer + 1f;
    }

    void activateCurlVisual() {
        curlSetVisual.SetActive(true);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);
    }

    void activateHoldVisual() {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        startMenu.SetActive(false);
    }

    void hideCurlShadows() {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        curlStartingShadow.SetActive(false);
        curlEndingShadow.SetActive(false);
        holdShadow.SetActive(true);
        holdShadow.SetActive(true);
    }

    void hideHoldShadows() {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        holdShadow.SetActive(false);
        holdShadow.SetActive(false);
        curlStartingShadow.SetActive(true);
        curlEndingShadow.SetActive(true);
    }

    void activateCalibrationHints() 
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(true);
        holdCalibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);

    }

    void activateHoldCalibrationHints() 
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(true);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);

    }

    void activatePauseMenu()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);

    }

    void activateStartMenu()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(true);
    }

    void activateGameOver()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
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
        holdTimer.activateTimer();
        holdTimerVisual.SetActive(true);
        repCounterVisual.SetActive(false);
    }

    public void disableHandTracking()
    {
        handGestureTracking.SetActive(false);
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
