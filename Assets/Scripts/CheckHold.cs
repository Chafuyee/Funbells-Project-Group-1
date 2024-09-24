using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHold : MonoBehaviour
{
    [SerializeField] public GameObject holdingDumbbell;

    private float score;
    private bool isTouched;
    // Start is called before the first frame update
    void Start()
    {
        score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void OnTriggerStay(Collider other){
        
        if (gameObject.activeSelf){
            if (holdingDumbbell.activeSelf){ //Both Dumbells are active
                
                // Check for collision
                if(other.gameObject == holdingDumbbell){
                    score += Time.deltaTime;
                    Debug.Log(score);
                }
              
            }
        }
    }

    
}
