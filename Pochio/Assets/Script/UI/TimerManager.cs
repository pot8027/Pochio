using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text HHmm;

    public Text mmm;

    private Stopwatch _stopWatch = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        _stopWatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        var span = _stopWatch.Elapsed;
        HHmm.text = $"{string.Format("{0:00}", span.Minutes)}:{string.Format("{0:00}", span.Seconds)}";
        mmm.text = $"{string.Format("{0:00}", span.Milliseconds)}";
    }

    public void TimerStop()
    {
        _stopWatch.Stop();
    }

    public void TimerStart()
    {
        _stopWatch.Start();
    }

    public TimeSpan GetSpan()
    {
        return _stopWatch.Elapsed;
    }
}
