using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateRepCounter : MonoBehaviour
{
    public TextMeshPro worldRepCountText;
    private CheckRep checkRep;
    private int repCount;

    // Start is called before the first frame update
    void Start()
    {
        repCount = checkRep.reps;
    }

    // Update is called once per frame
    void Update()
    {
        worldRepCountText.text = repCount.ToString();
    }
}
