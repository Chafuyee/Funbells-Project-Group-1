using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoldTimer : MonoBehaviour
{
    public TextMeshPro holdTimeText;
    private float allotedTime = 10f;
    private bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        // You can initialize anything here if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease the allotedTime by the time passed since the last frame
        if (timerOn == true) {
            if (allotedTime > 0)
            {
                allotedTime -= Time.deltaTime;  // Subtract the time passed since the last frame
                if (allotedTime < 0)
                {
                    deactivateTimer();
                    allotedTime = 0;  // Ensure it doesn't go below 0
                }
            }
        }

        // Update the text with the remaining time formatted to 2 decimal places
        holdTimeText.text = allotedTime.ToString("F2") + "s";
    }

    public void activateTimer() {
        timerOn = true;
    }

    public void deactivateTimer() {
        timerOn = false;
        allotedTime = 10f;
    }
}