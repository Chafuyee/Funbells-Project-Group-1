using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoldTimer : MonoBehaviour
{
    public TextMeshPro holdTimeText;
    private float allotedTime = 15f;

    // Start is called before the first frame update
    void Start()
    {
        // You can initialize anything here if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease the allotedTime by the time passed since the last frame
        if (allotedTime > 0)
        {
            allotedTime -= Time.deltaTime;  // Subtract the time passed since the last frame
            if (allotedTime < 0)
            {
                allotedTime = 0;  // Ensure it doesn't go below 0
            }
        }

        // Update the text with the remaining time formatted to 2 decimal places
        holdTimeText.text = allotedTime.ToString("F2") + "s";
    }
}