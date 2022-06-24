using System;
using System.Collections.Generic;
using UnityEngine;
public class TimerUtils : MonoBehaviour
{
    private class Timer
    {
        private readonly Action callback;
        private float timer;
        private readonly float initTimer;
        private readonly bool call;
        public bool isSame(Action callback, float t, bool checkTimer = true)
        {
            if (checkTimer)
            {
                return this.callback.Equals(callback) && (initTimer == t);
            }
            else
            {
                return this.callback.Equals(callback);
            }
        }
        public float InitialTimer => initTimer;
        public Timer(float t, Action callback, bool call)
        {
            timer = t;
            initTimer = t;
            this.call = call;
            this.callback = callback;
        }
        public void Step()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (Invoke())
                {
                    Pop(this);
                }
            }
        }
        private bool Invoke()
        {
            try
            {
                if (call)
                {
                    callback();
                }
                return true;
            }
            catch
            {
                // Mostly caused by destroyal of object.
                Pop(this);
                return false;
            }
        }
    }
    private static List<Timer> timer = new List<Timer>();
    private void Update()
    {
        for (int i = 0; i < timer.Count; i++)
        {
            if (timer[i] != null)
            {
                timer[i].Step();
            }
        }
        for (int i = 0; i < timer.Count; i++)
        {
            if (timer[i] == null)
            {
                timer.RemoveAt(i);
                i = 0;
            }
        }
    }
    /// <summary>
    /// Set delay for an action before it is called.
    /// </summary>
    /// <param name="t">Time in seconds</param>
    /// <param name="callback">Action</param>
    /// <param name="immediate">Is it going to be ran immediately?</param>
    /// <param name="call">Is it going to be called on time elapse?</param>
    /// <returns>If the action has been successfully added to queue, it will return true. Otherwise, false</returns>
    public static bool AddTimer(float t, Action callback, bool call = true)
    {
        foreach (var item in timer)
        {
            if (item != null && item.isSame(callback, t))
            {
                return false;
            }
        }
        timer.Add(new Timer(t, callback, call));
        return true;
    }
    public static bool ActionExists(Action callback)
    {
        foreach (var item in timer)
        {
            if (item != null && item.isSame(callback, 0, false))
            {
                return true;
            }
        }
        return false;
    }
    private static void Pop(Timer timer)
    {
        TimerUtils.timer.Remove(timer);
    }
    public static void Cancel(Action action)
    {
        timer.RemoveAll((m) => m.isSame(action, 0, false));
    }
}
