using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class StartButtonOnClick : Button
{
    public void OnTrigger(Collider other){
        if (other.CompareTag("HandCollider")){
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            Debug.Log("Pressed");
        }
        
    }
}
