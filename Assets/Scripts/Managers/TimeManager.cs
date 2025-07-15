
using System;
using UnityEngine;

public class TimeManager : ManagerBase
{
    public DateTime CurrentDate { get; private set; }

    private float dayTimer;
    private float secondsPerDay = 1.0f;

    public int CurrentDay => CurrentDate.Day;
    public int CurrentWeek => (CurrentDate.DayOfYear / 7) + 1;
    public int CurrentMonth => CurrentDate.Month;
    public int CurrentYear => CurrentDate.Year;

    public override void ManagedInitialize()
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
        CurrentDate = CurrentDate.AddDays(1);
        GameEvents.OnDateChanged?.Invoke(CurrentDay, CurrentWeek, CurrentMonth, CurrentYear);
        Debug.Log($"A new day has begun: {CurrentDate.ToShortDateString()}");

        // 년도 변경 이벤트는 필요 시 GameEvents에 추가 가능
    }
}
