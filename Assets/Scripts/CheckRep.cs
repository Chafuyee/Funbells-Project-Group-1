using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRep : MonoBehaviour
{
    [SerializeField] public GameObject startDumbell;
    [SerializeField] public GameObject endDumbell;
    public int reps;
    public bool startReached;
    public AudioSource repCountedSound;
    public GameStateManager StateManager;
    private bool repCounterOn = false;

    // Start is called before the first frame update
    void Start()
    {
        reps = 0;
        startReached = false;
    }
    void Update(){

    }

    void OnTriggerEnter(Collider other){
        
        if (repCounterOn == true) {
            if (gameObject.activeSelf){
                if (startDumbell.activeSelf && endDumbell.activeSelf){ //Both Dumbells are active
                    Debug.Log("Both dumbells are active");
                    //Check for collision
                    if(other.gameObject == startDumbell){
                        startReached = true;
                        Debug.Log("Start Reached");
                    }
                
                    if(other.gameObject == endDumbell && startReached == true){
                        reps += 1;
                        StateManager.incrementMoveTmr();
                        startReached = false;
                        repCountedSound.Play();
                        Debug.Log("End Reached");
                        Debug.Log(reps.ToString());
                    }


                }
            }
        }
    }

    public void activateRepCounter() {
        repCounterOn = true;
    }

    public void deactivateRepCounter() {
        repCounterOn = false;
    }

    public int getReps() {
        return reps;
    }

    public void addRep() {
        reps += 1;
    }
}
