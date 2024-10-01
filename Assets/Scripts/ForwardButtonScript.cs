using UnityEngine;
using UnityEngine.EventSystems; // Required for OnPointerDown and OnPointerUp events

public class ForwardButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameStateManager stateManager;

    // Called when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        // IMPORTANT CODE FOR MOVEMENT
        //GameManager.instance.SetForwardInput(1);
        stateManager.nextState();
        //Debug.Log("Forward button is held down.");
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // IMPORTANT CODE FOR MOVEMENT
        //GameManager.instance.SetForwardInput(0);
        //Debug.Log("Forward button is released.");
    }
}
