using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    private float _time;
    private float _remainingTime;

    private IEnumerator _countdown;

    private MonoBehaviour _context;

    public event Action TimeIsOver;

    public float TimerTime => _time;

    public Timer(MonoBehaviour context)
    {
        _context = context;
    }

    public void Set(float time)
    {
        _time = time;
        _remainingTime = _time;
    }

    public void StartCountingTime()
    {
        _countdown = Countdown();
        _context.StartCoroutine(_countdown);
    }

    public void StopCountingTime()
    {
        if (_countdown != null)
            _context.StopCoroutine(_countdown);
    }

    private IEnumerator Countdown()
    {
        while (_remainingTime >= 0)
        {
            _remainingTime -= Time.deltaTime;

            yield return null;
        }

        TimeIsOver?.Invoke();
    }

    public void SetPaused(bool isPaused)
    {
        StopCountingTime();
        Set(_remainingTime);

        if (isPaused == false)
            StartCountingTime();
    }
}
