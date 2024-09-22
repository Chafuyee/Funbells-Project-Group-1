using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar;

    // Update is called once per frame
    void Update()
    {
        int progress = GameManager.instance.GetSets();
        int maxProgress = GameManager.instance.GetMaxSets();
        float progressPercent = (float) progress / (float) maxProgress;
        Debug.Log("Progress: " + progressPercent);
        progressBar.value = progressPercent;
    }
}