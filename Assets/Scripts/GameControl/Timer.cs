using System;
using UnityEngine;

namespace GameControl
{
    public class Timer : MonoBehaviour
    {
        public float timeRemaining {get; private set; }
        public float duration { get; private set; }
        public delegate void Callback();

        public float getProgress() => 1 - (timeRemaining / duration);

        private Callback _callback;
        public Timer(float duration, Callback callback)
        {
            timeRemaining = duration;
            _callback = callback;
        }

        public static Timer MakeTimer(float duration, Callback callback)
        {
            GameObject timers = GameObject.Find("Timers");
            Timer t = timers.AddComponent<Timer>();
            t._callback = callback;
            t.timeRemaining = duration;
            t.duration = duration;
            return t;
        }

        private void Update()
        {
            if (timeRemaining < 0)
            {
                timeRemaining = 0f;
                _callback();
                Destroy(this);
            }
            else
            {
                timeRemaining -= Time.deltaTime;
            }
        }
    }
}