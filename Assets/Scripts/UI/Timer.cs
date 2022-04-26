using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float time;
    TextMeshProUGUI timerText;
    public static bool timePaused = true;

    private void Start()
    {
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (!timePaused)
        {
            time += Time.unscaledDeltaTime;
            UpdateTime();
        }
    }

    public static void ResetTimer()
    {
        time = 0;
    }
    void UpdateTime()
    {
        float timeToDisplay = (int)(time * 100) / 100.0f;
        timerText.text = timeToDisplay.ToString();
    }
    public static void SetTimePaused(bool pause)
    {
        timePaused = pause;
    }
    public static float GetRoundedTime()
    {
        return (int)(time * 100) / 100.0f;
    }
}
