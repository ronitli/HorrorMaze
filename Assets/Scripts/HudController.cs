using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;


public class UnityTimer
{
    private Stopwatch stopwatch;
    private float elapsedTime;

    public UnityTimer()
    {
        stopwatch = new Stopwatch();
        elapsedTime = 0f;
    }

    public void Start()
    {
        stopwatch.Start();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    public void Reset()
    {
        stopwatch.Reset();
        elapsedTime = 0f;
    }

    public void Update()
    {
        if (stopwatch.IsRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public TimeSpan ElapsedTime
    {
        get { return stopwatch.Elapsed; }
    }

    public float ElapsedTimeSeconds
    {
        get { return elapsedTime; }
    }
}


public class HudController : MonoBehaviour
{
    private UnityTimer timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText; 

    public int score{private set; get;} = 0;
    private int Seconds = 0;
    
    private void Start()
    {
        timer = new UnityTimer();
        timer.Start();
        scoreText.SetText("Score: 0");
    }

    private void Update()
    {
        timer.Update();
        ScoreContainer.time = timer.ElapsedTime;
        timerText.SetText(
            string.Format("Time: {0:00}:{1:00}:{2:00}", 
                timer.ElapsedTime.Hours, 
                timer.ElapsedTime.Minutes, 
                timer.ElapsedTime.Seconds
                )
            );
        if (Seconds != timer.ElapsedTime.Seconds){
            UpdateScore(score +1);
        }
        Seconds = timer.ElapsedTime.Seconds;
    }

    public void UpdateScore(int score)
    {
        scoreText.SetText("Score: " + score);
        this.score = score;
        ScoreContainer.score = score;
    }
}
