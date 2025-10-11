using UnityEngine;
using System;
using System.Collections;

public class AccTimer : MonoBehaviour
{

    [Header("Timer Settings")]
    [SerializeField] public float startInterval = 5.0f; // <- set your 5s here
    [SerializeField] public float minInterval = 0.15f;
    [SerializeField] public float linearStep = 0.02f;
    [SerializeField] public bool useUnscaledTime = true;
    [SerializeField] public bool autoStart = false;

    public event Action Tick;

    Coroutine _loop;
    float _interval;
    float _elapsed;
    bool _running;

    void OnEnable()
    {
        if (autoStart) Begin();
    }

    void OnDisable()
    {
        End();
    }

    public void Begin()
    {
        if (_running)
        {
            Debug.LogWarning($"[AcceleratingTimer] Begin() ignored: already running on {name}.");
            return;
        }

        // Sanity clamps
        if (startInterval < 0.0001f) startInterval = 0.0001f;
        if (minInterval < 0.0001f) minInterval = 0.0001f;
        if (linearStep < 0f) linearStep = 0f;
        if (minInterval > startInterval) minInterval = startInterval;

        _interval = startInterval;
        _elapsed = 0f;

        Debug.Log($"[AcceleratingTimer] BEGIN on '{name}' " +
                  $"start={_interval:F2}s, min={minInterval:F2}s, step={linearStep:F3}s, " +
                  $"unscaled={(useUnscaledTime ? "YES" : "NO")}");

        _loop = StartCoroutine(Loop());
        _running = true;
    }

    public void End()
    {
        if (_loop != null) StopCoroutine(_loop);
        _loop = null;
        _running = false;
        // Optional: Debug.Log($"[AcceleratingTimer] END on '{name}'");
    }

    IEnumerator Loop()
    {
        float timer = _interval;
        int dbgCount = 0;

        while (true)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (dt > 0.25f) dt = 0.25f;

            _elapsed += dt;
            timer -= dt;

            while (timer <= 0f)
            {
                Tick?.Invoke();

                // Debug first few ticks to verify cadence
                if (dbgCount < 6)
                {
                    Debug.Log($"[AcceleratingTimer] Tick at {Time.time:F2}s | interval was {_interval:F3}s");
                    dbgCount++;
                }

                _interval -= linearStep;
                if (_interval < minInterval) _interval = minInterval;

                timer += Mathf.Max(_interval, 0.0001f);
            }

            yield return null;
        }
    }
}