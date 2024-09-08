using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private Button scene1Button;
    private Button scene2Button;
    private Button scene3Button;
    private Button incrementScoreButton; // Add the increment button
    private Button decrementScoreButton; // Add the decrement button
    private Label scoreLabel;

    void OnEnable()
    {
        // Get the root of the UI document
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Link UI elements to the variables
        scene1Button = root.Q<Button>("scene1Button");
        scene2Button = root.Q<Button>("scene2Button");
        scene3Button = root.Q<Button>("scene3Button");
        incrementScoreButton = root.Q<Button>("incrementScoreButton"); // Link increment button
        decrementScoreButton = root.Q<Button>("decrementScoreButton"); // Link decrement button
        scoreLabel = root.Q<Label>("scoreLabel");

        // Register scene switching button clicks
        scene1Button.clicked += () => ChangeScene("Scene1");
        scene2Button.clicked += () => ChangeScene("Scene2");
        scene3Button.clicked += () => ChangeScene("Scene3");

        // Register score increment and decrement button clicks
        incrementScoreButton.clicked += IncrementScore;
        decrementScoreButton.clicked += DecrementScore;

        // Load the saved score and update the score display
        GameManager.instance.LoadScore();  // Load the saved score
        UpdateScoreUI();  // Update the UI with the loaded score
    }

    // Function to handle scene switching
    void ChangeScene(string sceneName)
    {
        // Save the score before switching scenes
        GameManager.instance.SaveScore();

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    // Increment the score and update the label
    void IncrementScore()
    {
        GameManager.instance.AddScore(1);  // Increment the score by 1
        UpdateScoreUI();  // Refresh the score display
    }

    // Decrement the score and update the label
    void DecrementScore()
    {
        GameManager.instance.AddScore(-1);  // Decrement the score by 1
        UpdateScoreUI();  // Refresh the score display
    }

    // Update the score label from GameManager
    void UpdateScoreUI()
    {
        int currentScore = GameManager.instance.GetScore();  // Get the current score from GameManager
        scoreLabel.text = "Score: " + currentScore.ToString();  // Display the score
    }
}
