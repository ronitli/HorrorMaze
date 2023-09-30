using TMPro;
using UnityEngine;

public class ScoreLoader : MonoBehaviour
{
    private static string scoreMessage = "Your score is: ",
        timeMessage = "You survived for: ";
    public TextMeshProUGUI resultText;

    // Start is called before the first frame update
    void Start()
    {
        resultText.SetText(scoreMessage + ScoreContainer.score + "\n" + timeMessage + 
        string.Format("{0:00}:{1:00}:{2:00}", 
                ScoreContainer.time.Hours, 
                ScoreContainer.time.Minutes, 
                ScoreContainer.time.Seconds
                )
            );
    }
}
