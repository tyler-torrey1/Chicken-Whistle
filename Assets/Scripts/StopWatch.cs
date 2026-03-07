using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A general timer utility that uses a timestamp to
 * calculate the currently elapsed time. A StopWatch
 * can be used during game pause (timeScale = 0f) via
 * setting unscaledTime to true on construction.
 */

[Serializable]
public class StopWatch
{
    private readonly bool unscaledTime; // ignore game pause?
    [SerializeField] private float timestamp;

    private float currentTime => (unscaledTime) ? Time.unscaledTime : Time.time;
    public static implicit operator float(StopWatch watch) => watch.currentTime - watch.timestamp;
    public static implicit operator string(StopWatch watch) => ((float) watch).ToString();

    private StopWatch() { }
    public StopWatch(bool unscaledTime = false)
    {
        this.unscaledTime = unscaledTime;
        Start();
    }

    /**
     * Start timing by saving the current timestamp.
     */
    public void Start()
    {
        timestamp = currentTime;
    }
}
