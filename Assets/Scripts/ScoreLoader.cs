using TMPro;
using UnityEngine;

public class ScoreLoader : MonoBehaviour
{
    private static string _scoreMessage = "Your score is: ",
        _timeMessage = "You survived for: ";
    public TextMeshProUGUI resultText;

    // Start is called before the first frame update
    void Start()
    {
        resultText.SetText(_scoreMessage + ScoreContainer.score + "\n" + _timeMessage + 
        string.Format("{0:00}:{1:00}:{2:00}", 
                ScoreContainer.time.Hours, 
                ScoreContainer.time.Minutes, 
                ScoreContainer.time.Seconds
                )
            );
    }
}
