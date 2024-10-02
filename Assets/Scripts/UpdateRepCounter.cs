using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateRepCounter : MonoBehaviour
{
    public TextMeshPro worldRepCountText;
    public CheckRep checkRepScript;
    public GameStateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        checkRepScript = GetComponent<CheckRep>();

    }

    // Update is called once per frame
    void Update()
    {
        worldRepCountText.text = stateManager.stateReps.ToString();
    }
}
