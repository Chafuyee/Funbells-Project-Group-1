using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // Singleton instance for global access

    private int playerScore = 0;  // Variable to track the player's score

    void Awake()
    {
        // Singleton pattern: Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Method to get the current player score
    public int GetScore()
    {
        return playerScore;
    }

    // Method to set the player's score
    public void SetScore(int score)
    {
        playerScore = score;
    }

    // Method to add points to the player's score
    public void AddScore(int points)
    {
        playerScore += points;
    }

    // Method to save the current score to PlayerPrefs
    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.Save();  // Ensure data is written to disk
    }

    // Method to load the saved score from PlayerPrefs
    public void LoadScore()
    {
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);  // Load score, defaulting to 0 if none is saved
    }
}