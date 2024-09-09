using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;  // Required for Coroutines

public class GameSceneManager : MonoBehaviour
{
    private Button scene1Button;
    private Button scene2Button;
    private Button scene3Button;
    private Button incrementScoreButton; // Add the increment button
    private Button decrementScoreButton; // Add the decrement button
    private Label scoreLabel;

    // Interval for refreshing the score UI
    public float refreshInterval = 0.1f; // Refresh every 1 second

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

        // Start the Coroutine to refresh the score UI periodically
        StartCoroutine(RefreshScoreUI());
    }

    // Function to handle scene switching
    void ChangeScene(string sceneName)
    {
        // Save the score before switching scenes
        GameManager.instance.Save();

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    // Increment the score and update the label
    void IncrementScore()
    {
        int currentScore = GameManager.instance.GetScore();  // Get the current score
        GameManager.instance.SetScore(currentScore + 1);  // Increment the score by 1
    }

    // Decrement the score and update the label
    void DecrementScore()
    {
        int currentScore = GameManager.instance.GetScore();  // Get the current score
        GameManager.instance.SetScore(currentScore - 1);  // Decrement the score by 1
    }

    // Coroutine to refresh the score UI periodically
    IEnumerator RefreshScoreUI()
    {
        while (true)
        {
            UpdateScoreUI();  // Refresh the score display
            yield return new WaitForSeconds(refreshInterval);  // Wait for the refresh interval
        }
    }

    // Update the score label from GameManager
    void UpdateScoreUI()
    {
        int currentScore = GameManager.instance.GetScore();  // Get the current score from GameManager
        scoreLabel.text = "Score: " + currentScore.ToString();  // Display the score
    }
}
