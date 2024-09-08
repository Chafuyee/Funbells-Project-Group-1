using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private Button startButton;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("startButton");

        startButton.clicked += StartGame;
    }

    void StartGame()
    {
        // Load the first gameplay scene
        SceneManager.LoadScene("Scene1");
    }
}
