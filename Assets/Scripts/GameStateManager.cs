using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject wristButtonUI;

    private int exerciseState = 0;

    public GameObject currentActive;
    public GameObject lastActive;
    public GameObject originalDumbell;

    public GameObject handGestureTracking;
    public GameObject curlEndingShadow;
    public GameObject curlStartingShadow;
    public GameObject holdHeightShadow;

    public bool repDetectionOn = false;
    public bool holdDetectionOn = false;
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
    private float fallTimer = 0f;

    public int optionalStateTrigger = 0;
    private int maxStates = 10;
    private float forwardInput;

    private int currentCurlSet = 1;
    private int currentHoldSet = 1;

    public TextMeshPro worldCurlVisual;
    public TextMeshPro worldHoldVisual;

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
        worldCurlVisual.text = "You're on Curl Set: " + currentCurlSet.ToString();
        worldHoldVisual.text = "You're on Hold Set: " + currentHoldSet.ToString();

        // Move Player Forward
        if (repDetectionOn == true)
        {
            if (moveTimer > 0)
            {
                GameManager.instance.SetForwardInput(1);
                moveTimer -= Time.deltaTime;
            }
            else if (moveTimer <= 0)
            {
                moveTimer = 0;
                GameManager.instance.SetForwardInput(0);
            }
        }

        // Move Player back
   
        if (holdDetectionOn == true)
        {
            if (fallTimer > 0)
            {
                GameManager.instance.SetForwardInput(-1);
                fallTimer -= Time.deltaTime;
            }
            else if (fallTimer <= 0)
            {
                fallTimer = 0;
                GameManager.instance.SetForwardInput(0);
            }
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
                holdDetectionOn = false;
                break;
            case 3: // Start Menu
                hideAllShadows();
                repDetectionOn = false;
                holdDetectionOn = false;
                activateStartMenu(); //
                playerController.setStarted(); // Set XR Rig on Fixed Axis
                break;
            case 4: // TEST HOLD OUTSIDE OF LOOP
                hideCurlShadows();
                repDetectionOn = false;
                holdDetectionOn = true;
                activateHoldVisual();
                activateHoldTimer();
                break;
            case 5: // Set 1s - Curl
                handGestureTracking.SetActive(false);
                // Generate the next state using the first row of csvData
                if (csvDataIndex < csvData.GetLength(1)) // Ensure index is within bounds
                {
                    string incrementCode = csvData[0, csvDataIndex];
                    Debug.Log(incrementCode);
                    generateNextState(incrementCode, exerciseState);
                    // CODE FOR CURL EXERCISE FOLLOWED BY HOLD EXERCISE
                    if (exerciseState == 0)
                    {
                        activateCurlVisual(); // Show Curl Visual
                        activateRepCount(); // Enable Rep Counting Mechanism
                        hideHoldShadows(); // Hide Hold Guidance Shadow
                        repDetectionOn = true; // Activate Rep Detection
                        holdDetectionOn = false; // Deactivate Hold Detection

                    }
                    else if (exerciseState == 1)
                    {
                        generateNextState(incrementCode, exerciseState); // Change the weight to hold visual
                        hideCurlShadows(); // Hide the curl calibration shadows
                        holdDetectionOn = true; // activate hold detection for exercise
                        repDetectionOn = false; // deactivate rep detection
                        activateHoldTimer(); // enable visuals
                        activateHoldVisual(); // enable visuals

                    } else if (exerciseState == 2)
                    {
                        activatePauseMenu();
                        repDetectionOn = false;
                        holdDetectionOn = false;
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
        stateReps = 0; // Reset the set reps
        if (exerciseState == 0)
        {
            currentCurlSet++;
            exerciseState++;
            holdTimer.deactivateTimer();
            holdTimer.activateTimer();

        }
        else if (exerciseState == 1)
        {
            exerciseState++;
            currentHoldSet++;
        }
        else if (exerciseState == 2) {
            exerciseState = 0;
            csvDataIndex++;

        }
       
    }
    private void changeWeightVisual(string weightName, int exerciseType)
    {
        string processedName = "";

        if (currentActive.name != "Dumbell")
        {
            lastActive = currentActive;
        }

        // Convert weight type
        if (weightName.Substring(0, 4) == "1.75")
        {
            if (exerciseType == 0)
            {
                processedName = "2" + weightName.Substring(4);
            }
            else
            {
                processedName = "bar_" + weightName.Substring(4);
            }
        }
        else if (weightName.Substring(0, 4) == "2.75")
        {
            if (exerciseType == 0)
            {
                processedName = "3" + weightName.Substring(4);
            }
            else
            {
                processedName = "2" + weightName.Substring(4);
            }
        }
        else if (weightName.Substring(0, 4) == "3.75")
        {
            if (exerciseType == 0)
            {
                processedName = "5.25" + weightName.Substring(4);
            }
            else
            {
                processedName = "3" + weightName.Substring(4);
            }
        }

        // Find the next weight GameObject
        Transform nextWeightTransform = wristButtonUI.transform.Find(processedName);
        GameObject nextWeight = nextWeightTransform.gameObject;

        // Check if the GameObject exists before activating it
        if (nextWeight != null)
        {
            currentActive = nextWeight;
            currentActive.SetActive(true);
            //Debug.Log("HERE IS CURRENT:" + currentActive.name);

            if (lastActive != null)
            {
                originalDumbell.SetActive(false);
                lastActive.SetActive(false);
                currentActive.SetActive(true);
            }
        }
        else
        {
            //Debug.LogError("GameObject with name " + processedName + " not found!");
        }
    }

    private void generateNextState(string incrementCode, int exerciseType)
    {
        float sizeInKg = float.Parse(incrementCode.Substring(0, 4));
        string visualRepresentation = incrementCode.Substring(4);
        changeWeightVisual(incrementCode, exerciseType);
        if (visualRepresentation == "s")
        {

            //Debug.Log("Small weight, Size: " + sizeInKg);
        }
        else if (visualRepresentation == "n")
        {
            //Debug.Log("Normal weight, Size: " + sizeInKg);
            // Pull normal weight relative to sizeInKg
        }
        else
        {
            //Debug.Log("Big weight, Size: " + sizeInKg);
            // Pull big weight relative to sizeInKg
        }
    }

    public void incrementStateReps()
    {
        Debug.Log("STATE REP ADDED");
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
        //Debug.Log("MOVE TIMER HERE");
        moveTimer = moveTimer + 0.25f;
    }

    public void moveForward()
    {
        GameManager.instance.SetForwardInput(1);
    }

    public void stopMovement()
    {
        GameManager.instance.SetForwardInput(0);
    }

    public void moveBackwards()
    {
        GameManager.instance.SetForwardInput(-1);
    }

    public void incrementFallTmr()
    {
        fallTimer = fallTimer + 0.1f;
        //Debug.Log(fallTimer.ToString());
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
        curlStartingShadow.SetActive(false);
        curlEndingShadow.SetActive(false);
        holdHeightShadow.SetActive(true);
        holdHeightShadow.SetActive(true);
    }

    void hideAllShadows()
    {
        curlStartingShadow.SetActive(false);
        curlEndingShadow.SetActive(false);
        holdHeightShadow.SetActive(false);
        holdHeightShadow.SetActive(false);
    }

    void hideHoldShadows()
    {
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
