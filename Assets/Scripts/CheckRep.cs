using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRep : MonoBehaviour
{
    [SerializeField] public GameObject startDumbell;
    [SerializeField] public GameObject endDumbell;
    public int reps;
    public bool startReached;

    // Start is called before the first frame update
    void Start()
    {
        reps = 0;
        startReached = false;
    }
    void Update(){
        
    }

    void OnTriggerEnter(Collider other){
        
        if (gameObject.activeSelf){
            if (startDumbell.activeSelf && endDumbell.activeSelf){ //Both Dumbells are active
                Debug.Log("Both dumbells are active");
                // Check for collision
                if(other.gameObject == startDumbell){
                    startReached = true;
                    Debug.Log("Start Reached");
                }
              
                if(other.gameObject == endDumbell && startReached == true){
                    reps += 1;
                    startReached = false;
                    Debug.Log("End Reached");
                    Debug.Log(reps.ToString());
                }


            }
        }
    }
}
