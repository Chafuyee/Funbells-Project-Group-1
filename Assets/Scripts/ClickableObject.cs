using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public int scoreValue = 1;  // Amount of score to add when clicked

    void OnMouseDown()
    {
        // Call the GameManager to increase the score
        int currentScore = GameManager.instance.GetScore();
        GameManager.instance.SetScore(currentScore + scoreValue);
    }
}