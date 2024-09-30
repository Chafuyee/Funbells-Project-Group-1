using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateRepCounter : MonoBehaviour
{
    public TextMeshPro worldRepCountText;
    public CheckRep checkRepScript;
    private int repCount;

    // Start is called before the first frame update
    void Start()
    {
        checkRepScript = GetComponent<CheckRep>();
    }

    // Update is called once per frame
    void Update()
    {
        worldRepCountText.text = repCount.ToString();
        repCount = checkRepScript.getReps();
    }
}
