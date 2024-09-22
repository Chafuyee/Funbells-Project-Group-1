using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoaderUI : MonoBehaviour
{
    private Button myButton;

    void OnEnable()
    {
        // Reference the UI Document component to access the UXML
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        // Find the button by its name in the UXML
        myButton = rootVisualElement.Q<Button>("myButton");

        // Register the click event
        myButton.clicked += OnButtonClick;
    }

    void OnDisable()
    {
        // Unregister the click event
        myButton.clicked -= OnButtonClick;
    }

    // Method to load the scene when the button is clicked
    void OnButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
