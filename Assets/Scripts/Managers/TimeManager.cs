
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public event Action<int, int, int, int> OnDayChanged;
    public event Action OnYearChanged;

    public DateTime CurrentDate { get; private set; }

    private float dayTimer;
    private float secondsPerDay = 1.0f;

    public int CurrentDay => CurrentDate.Day;
    public int CurrentWeek => (CurrentDate.DayOfYear / 7) + 1;
    public int CurrentMonth => CurrentDate.Month;
    public int CurrentYear => CurrentDate.Year;

    public void Initialize()
    {
        CurrentDate = new DateTime(2025, 1, 1); // 게임 시작 날짜
        secondsPerDay = 1.0f; // 테스트를 위해 1초로 설정
        Debug.Log($"TimeManager initialized. Start date: {CurrentDate.ToShortDateString()}");
    }

    public void ResetTime()
    {
        CurrentDate = new DateTime(2025, 1, 1);
        dayTimer = 0f;
        Debug.Log("Time reset to initial state.");
    }

    public void SetTime(DateTime date)
    {
        CurrentDate = date;
        dayTimer = 0f;
        Debug.Log($"Time set to: {CurrentDate.ToShortDateString()}");
    }

    public void Tick(float deltaTime)
    {
        dayTimer += deltaTime;
        if (dayTimer >= secondsPerDay)
        {
            dayTimer -= secondsPerDay;
            AdvanceDay();
        }
    }

    private void AdvanceDay()
    {
        int oldYear = CurrentDate.Year;
        CurrentDate = CurrentDate.AddDays(1);
        OnDayChanged?.Invoke(CurrentDay, CurrentWeek, CurrentMonth, CurrentYear);
        Debug.Log($"A new day has begun: {CurrentDate.ToShortDateString()}");

        if (CurrentDate.Year != oldYear)
        {
            OnYearChanged?.Invoke();
            Debug.Log($"A new year has begun: {CurrentDate.Year}");
        }
    }
}
