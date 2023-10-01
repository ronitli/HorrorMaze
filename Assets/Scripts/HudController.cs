using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;


public class UnityTimer
{
    private Stopwatch _stopwatch;
    private float _elapsedTime;

    public UnityTimer()
    {
        _stopwatch = new Stopwatch();
        _elapsedTime = 0f;
    }

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _stopwatch.Reset();
        _elapsedTime = 0f;
    }

    public void Update()
    {
        if (_stopwatch.IsRunning)
        {
            _elapsedTime += Time.deltaTime;
        }
    }

    public TimeSpan elapsedTime
    {
        get { return _stopwatch.Elapsed; }
    }

    public float elapsedTimeSeconds
    {
        get { return _elapsedTime; }
    }
}


public class HudController : MonoBehaviour
{
    private UnityTimer _timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText; 

    public int score{private set; get;}
    private int _seconds;
    
    private void Start()
    {
        _timer = new UnityTimer();
        _timer.Start();
        scoreText.SetText("Score: 0");
    }

    private void Update()
    {
        _timer.Update();
        ScoreContainer.time = _timer.elapsedTime;
        timerText.SetText(
            string.Format("Time: {0:00}:{1:00}:{2:00}", 
                _timer.elapsedTime.Hours, 
                _timer.elapsedTime.Minutes, 
                _timer.elapsedTime.Seconds
                )
            );
        if (_seconds != _timer.elapsedTime.Seconds){
            UpdateScore(score +1);
        }
        _seconds = _timer.elapsedTime.Seconds;
    }

    public void UpdateScore(int amount)
    {
        scoreText.SetText("Score: " + amount);
        this.score = amount;
        ScoreContainer.score = amount;
    }
}
