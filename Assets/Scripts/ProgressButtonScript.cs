using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        int progress = GameManager.instance.GetProgress();
        progress += 5;
        GameManager.instance.SetProgress(progress);
    }
}
