using UnityEngine;
using UnityEngine.EventSystems; // Required for OnPointerDown and OnPointerUp events

public class BackwardButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // Called when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.SetForwardInput(-1);
        Debug.Log("Backward button is held down.");
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.SetForwardInput(0);
        Debug.Log("Backward button is released.");
    }
}
