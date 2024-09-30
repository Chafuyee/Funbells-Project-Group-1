using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateRepCounter : MonoBehaviour
{
    public TextMeshPro worldRepCountText;
    private CheckRep checkRep;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldRepCountText.text = checkRep.reps.ToString();
    }
}
