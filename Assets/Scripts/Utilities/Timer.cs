using UnityEngine;
using System;

public class Timer
{
    public float DurationTime { get; private set; }
    public Action OnComplete;

    private float _timeCount;
    private bool _isPause;

    public Timer() : this(0f, null) {}


    public Timer(float durationTime, Action onComplete = null)
    {
        DurationTime = durationTime;
        OnComplete = onComplete;
        _timeCount = 0f;
        _isPause = false;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (_isPause)
            return;

        _timeCount += deltaTime;

        if (_timeCount >= DurationTime)
        {
            OnComplete?.Invoke();
            Debug.Log("Timer Complete");
            // 计时结束后重置计时器 达到一直在计时的效果
            ResetTime();
        }
    }

    public void Pause() => _isPause = true;

    public void Resume() => _isPause = false;

    public void ResetTime()
    {
        _timeCount = 0f;
        _isPause = false;
    }
}
