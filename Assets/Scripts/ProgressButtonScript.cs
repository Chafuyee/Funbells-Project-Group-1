using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        int sets = GameManager.instance.GetSets();
        sets += 1;
        if (sets > GameManager.instance.GetMaxSets())
        {
            sets = 0;
            int reps = GameManager.instance.GetReps();
            reps += 1;
            GameManager.instance.SetReps(reps);
        }
        GameManager.instance.SetSets(sets);
    }
}
