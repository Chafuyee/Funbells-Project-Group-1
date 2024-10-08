using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHold : MonoBehaviour
{
    [SerializeField] public GameObject holdingDumbbell;

    private bool isTouched;
    public GameStateManager StateManager;

    private bool isFalling = false;
    private Coroutine fallTimerCoroutine;  // Coroutine to handle fall timer
    private bool isHolding = false;  // New flag to track holding state

    void Update()
    {
        // Continuously increment fall timer if the dumbbell is not being held
        // StateManager.checkHoldDetectionOn() &&
        Debug.Log("HOLD DETECTION:" + StateManager.checkHoldDetectionOn());
        if (StateManager.checkHoldDetectionOn() && !isHolding)
        {
            StateManager.incrementFallTmr();
            Debug.Log("Fall timer incrementing, dumbbell not held.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (StateManager.checkHoldDetectionOn())
        {
            if (gameObject.activeSelf && holdingDumbbell.activeSelf)
            {
                Debug.Log("Collider detected: " + other.gameObject.name);

                if (other.gameObject == holdingDumbbell)
                {
                    Debug.Log("GOOD WORK");

                    // If the dumbbell re-enters the trigger, stop any fall timer
                    if (fallTimerCoroutine != null)
                    {
                        StopCoroutine(fallTimerCoroutine);
                        fallTimerCoroutine = null;
                    }

                    isFalling = false;
                    isHolding = true;  // Set holding flag to true
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (StateManager.checkHoldDetectionOn() && other.gameObject == holdingDumbbell)
        {
            // Start the fall timer coroutine if it hasn't started yet
            if (!isFalling)
            {
                fallTimerCoroutine = StartCoroutine(StartFallTimer());
            }
            Debug.Log("DUMBELL HAS EXITED TRIGGER");
            isHolding = false;  // Set holding flag to false when exiting
        }
    }

    private IEnumerator StartFallTimer()
    {
        isFalling = true;

        // Delay before the fall timer increments to avoid rapid triggering
        yield return new WaitForSeconds(0.5f);

        // Increment fall timer after delay
        StateManager.incrementFallTmr();
        Debug.Log("Dumbbell has exited the trigger zone and fall timer incremented");

        // Reset flag
        isFalling = false;
    }
}