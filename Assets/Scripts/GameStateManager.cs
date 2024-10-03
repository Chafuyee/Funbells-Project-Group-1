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

    private int exerciseState = 0;

    public GameObject currentActive;
    public GameObject lastActive;
    public GameObject originalDumbell;

    public GameObject handGestureTracking;
    public GameObject curlEndingShadow;
    public GameObject curlStartingShadow;
    public GameObject holdHeightShadow;

    public bool repDetectionOn = true;
    public bool holdDetectionOn = false;
    public PlayerMovement playerController;

    public AudioSource audioSource;

    public CheckRep checkRepScript;

    public int stateReps;
    public bool isPaused;
    private int currentState = 0;
    private float stateDuration = 15f;
    private float stateTimer = 0f;

    private bool onCurl = true;

    public HoldTimer holdTimer;
    private float moveTimer = 0f;
    private float fallTimer = 0f;

    public int optionalStateTrigger = 0;
    private int maxStates = 10;
    private float forwardInput;

    string[,] csvData;

    private int csvDataIndex = 0;

    void Start()
    {
        checkRepScript = GetComponent<CheckRep>();
        isPaused = true;
        handGestureTracking.SetActive(true);
        generateExperimentList();
    }

    // Update is called once per frame
    void Update()
    {
        // Move Player Forward
        if (moveTimer > 0)
        {
            GameManager.instance.SetForwardInput(1);
            moveTimer -= Time.deltaTime;
        }
        else if (moveTimer < 0)
        {
            moveTimer = 0;
            GameManager.instance.SetForwardInput(0);
        }
        else
        {
            GameManager.instance.SetForwardInput(0);
        }

        // Move Player back
        if (fallTimer > 0)
        {
            GameManager.instance.SetForwardInput(-1);
            fallTimer -= Time.deltaTime;
        } else if (moveTimer < 0)
        {
            moveTimer = 0;
            GameManager.instance.SetForwardInput(0);
        }
        else
        {
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
                //hideCurlShadows();
                repDetectionOn = false;
                break;

            case 3: // TEST CASE
                holdDetectionOn = true;
                activateHoldTimer();
                activateHoldVisual();
                playerController.setStarted();
                break;

            case 4: // Start Menu
                hideHoldShadows();
                //disableHandTracking();
                repDetectionOn = false;
                activateStartMenu(); //
                playerController.setStarted(); // Set XR Rig on Fixed Axis
                break;
            case 5: // Set 1s - Curl

                // Generate the next state using the first row of csvData
                if (csvDataIndex < csvData.GetLength(1)) // Ensure index is within bounds
                {
                    string incrementCode = csvData[0, csvDataIndex];
                    generateNextState(incrementCode);
                    // CODE FOR CURL EXERCISE FOLLOWED BY HOLD EXERCISE
                    if (exerciseState == 0)
                    {
                        activateCurlVisual();
                        //checkRepScript.activateRepCounter();
                        activateRepCount();
                        hideHoldShadows();
                        repDetectionOn = true;
                    }
                    else if (exerciseState == 1)
                    {
                        repDetectionOn = false;
                        holdDetectionOn = true;
                        activateHoldVisual();
                        activateHoldTimer();

                    }
                }
                else
                {
                    // GAME OVER CONDITION
                    Debug.Log("All entries in the first row of csvData have been processed.");
                }

                // NEED BUTTON TO MOVE TO NEXT EXERCISE e.g., csvDataIndex++


                break;
        }
    }

    public void progressExercise()
    {
        if (exerciseState != 2)
        {
            exerciseState++;
        } else
        {
            exerciseState = 0;
        }
    }
    private void changeWeightVisual(string weightName)
    {
        if (lastActive == null)
        {
            lastActive = originalDumbell;
        } else
        {
            lastActive = currentActive;
        }

        GameObject nextWeight = GameObject.Find(weightName);
        currentActive = nextWeight;
        currentActive.SetActive(true);
        lastActive.SetActive(false);

    }

    private void generateNextState(string incrementCode)
    {
        float sizeInKg = float.Parse(incrementCode.Substring(0, 4));
        string visualRepresentation = incrementCode.Substring(4);
        changeWeightVisual(incrementCode);
        if (visualRepresentation == "s")
        {

            Debug.Log("Small weight, Size: " + sizeInKg);
        }
        else if (visualRepresentation == "n")
        {
            Debug.Log("Normal weight, Size: " + sizeInKg);
            // Pull normal weight relative to sizeInKg
        }
        else
        {
            Debug.Log("Big weight, Size: " + sizeInKg);
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

    public bool checkHoldDetectionOn()
    {
        return holdDetectionOn;
    }

    public void incrementMoveTmr()
    {
        moveTimer = moveTimer + 1f;
    }

    public void incrementFallTmr()
    {
        fallTimer = fallTimer + 0.1f;
        Debug.Log(fallTimer.ToString());
    }

    public void generateExperimentList()
    {
        // Define the CSV data as a string array
        csvData = new string[,]
        {
            { "1.75s", "1.75n", "2.75s", "2.75b", "3.75s", "1.75b", "3.75n", "3.75b", "2.75n" },
            { "3.75n", "2.75n", "3.75s", "3.75b", "2.75s", "1.75b", "1.75s", "2.75b", "1.75n" },
            { "2.75b", "1.75b", "1.75n", "3.75b", "1.75s", "2.75n", "2.75s", "3.75n", "3.75s" },
            { "2.75s", "3.75s", "1.75s", "3.75n", "1.75n", "2.75n", "2.75b", "3.75b", "1.75b" },
            { "3.75b", "2.75n", "1.75b", "3.75n", "2.75b", "3.75s", "1.75n", "2.75s", "1.75s" },
            { "1.75n", "1.75s", "2.75b", "2.75s", "1.75b", "3.75s", "3.75b", "3.75n", "2.75n" },
            { "3.75n", "3.75s", "2.75n", "2.75s", "3.75b", "1.75s", "1.75b", "1.75n", "2.75b" },
            { "1.75b", "2.75b", "3.75b", "1.75n", "2.75n", "1.75s", "3.75n", "2.75s", "3.75s" },
            { "2.75s", "1.75s", "3.75s", "1.75n", "3.75n", "2.75b", "2.75n", "1.75b", "3.75b" },
            { "2.75n", "3.75b", "3.75n", "1.75b", "3.75s", "2.75b", "2.75s", "1.75n", "1.75s" },
            { "1.75n", "2.75b", "1.75s", "1.75b", "2.75s", "3.75b", "3.75s", "2.75n", "3.75n" },
            { "3.75s", "3.75n", "2.75s", "2.75n", "1.75s", "3.75b", "1.75n", "1.75b", "2.75b" },
            { "1.75b", "3.75b", "2.75b", "2.75n", "1.75n", "3.75n", "1.75s", "3.75s", "2.75s" },
            { "1.75s", "2.75s", "1.75n", "3.75s", "2.75b", "3.75n", "1.75b", "2.75n", "3.75b" },
            { "2.75n", "3.75n", "3.75b", "3.75s", "1.75b", "2.75s", "2.75b", "1.75s", "1.75n" },
            { "2.75b", "1.75n", "1.75b", "1.75s", "3.75b", "2.75s", "2.75n", "3.75s", "3.75n" },
            { "3.75s", "2.75s", "3.75n", "1.75s", "2.75n", "1.75n", "3.75b", "2.75b", "1.75b" },
            { "3.75b", "1.75b", "2.75n", "2.75b", "3.75n", "1.75n", "3.75s", "1.75s", "2.75s" }
        };
    }


    void activateCurlVisual()
    {
        curlSetVisual.SetActive(true);
        holdSetVisual.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        holdTimerVisual.SetActive(false);
        startMenu.SetActive(false);
    }

    void activateHoldVisual()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        calibrationHints.SetActive(false);
        holdCalibrationHints.SetActive(false);
        repCounterVisual.SetActive(false);
        startMenu.SetActive(false);
    }

    void hideCurlShadows()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        curlStartingShadow.SetActive(false);
        curlEndingShadow.SetActive(false);
        holdHeightShadow.SetActive(true);
        holdHeightShadow.SetActive(true);
    }

    void hideHoldShadows()
    {
        curlSetVisual.SetActive(false);
        holdSetVisual.SetActive(false);
        holdHeightShadow.SetActive(false);
        holdHeightShadow.SetActive(false);
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
