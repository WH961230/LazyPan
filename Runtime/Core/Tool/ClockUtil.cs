using System;
using System.Collections.Generic;
using LazyPan;
using UnityEngine;

public enum ClockType {
    DateTimeClock,
    UnityTimeClock,
}

public class Clock {
    Action onAlarm;
    public readonly ClockType type;
    public readonly float alarmTime1;
    public readonly DateTime alarmTime2 = DateTime.Now;
    readonly bool repeat = false;
    readonly float interval = 10;
    float nextAlarmSecond = 0f;
    public bool IsStopped { get; private set; }

    public Clock(float time, Action callBack) {
        type = ClockType.UnityTimeClock;
        alarmTime1 = time;
        onAlarm = callBack;
    }

    public Clock(float time, float interval, Action callBack) {
        type = ClockType.UnityTimeClock;
        alarmTime1 = time;
        repeat = true;
        this.interval = interval;
        nextAlarmSecond = time + interval;
        onAlarm = callBack;
    }

    public Clock(DateTime time, Action callBack) {
        type = ClockType.DateTimeClock;
        alarmTime2 = time;
        onAlarm = callBack;
    }

    public void Invoke() {
        switch (type) {
            case ClockType.DateTimeClock:
                if (DateTime.Now >= alarmTime2) {
                    Alarm(true);
                    IsStopped = true;
                }

                break;
            case ClockType.UnityTimeClock:
                if (repeat) {
                    if (Time.time >= nextAlarmSecond) {
                        Alarm(false);
                        nextAlarmSecond += interval;
                    }
                } else {
                    if (Time.time >= alarmTime1) {
                        Alarm(true);
                        IsStopped = true;
                    }
                }

                break;
        }
    }

    public void Abandon() {
        if (IsStopped) {
            return;
        }

        IsStopped = true;
        onAlarm = null;
    }

    private void Alarm(bool once) {
        try {
            onAlarm?.Invoke();
        }
        catch (Exception e) {
            IsStopped = true;
            onAlarm = null;
        }
        finally {
            if (once) {
                onAlarm = null;
            }
        }
    }
}

public class ClockUtil : SingletonMonoBehaviour<ClockUtil> {
    readonly List<Clock> clocks = new List<Clock>();

    public Clock AlarmAt(DateTime dateTime, Action callBack) {
        if (dateTime < DateTime.Now) {
            return null;
        }

        var clock = new Clock(dateTime, callBack);
        clocks.Add(clock);
        return clock;
    }

    public Clock AlarmAfter(float second, Action callBack) {
        if (second < 0f) {
            return null;
        }

        var clock = new Clock(Time.time + second, callBack);
        if (second == 0f) {
            clock.Invoke();
            return clock;
        }

        clocks.Add(clock);
        return clock;
    }

    public Clock AlarmRepeat(float delay, float repeatInterval, Action callBack) {
        if (delay < 0f) {
            return null;
        }

        if (repeatInterval <= 0f) {
            return null;
        }

        var clock = new Clock(Time.time + delay, repeatInterval, callBack);
        clocks.Add(clock);
        return clock;
    }

    public void Stop(Clock clock) {
        clock?.Abandon();
    }

    public void Dispose() {
        foreach (var clock in clocks) {
            clock.Abandon();
        }
    }

    private void Update() {
        for (var i = clocks.Count - 1; i >= 0; i--) {
            var clock = clocks[i];
            if (clock.IsStopped) {
                clocks.RemoveAt(i);
            } else {
                clock.Invoke();
            }
        }
    }
}