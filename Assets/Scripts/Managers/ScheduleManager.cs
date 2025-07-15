
using UnityEngine;
using System.Collections.Generic;

public enum ActivityType
{
    None,
    BroadcastGame,
    BroadcastChat,
    BroadcastSong,
    BroadcastASMR,
    VocalTraining,
    DanceLesson,
    SpeechClass,
    GamePractice,
    Rest,
    Collaboration,
    Event,
    GoodsProduction,
    ProduceVideo,
    RecordCoverSong,
    ManageSNS
}

[System.Serializable]
public class ScheduleEntry
{
    public ActivityType activityType;
    public int durationHours; // 활동 시간 (예: 1시간, 2시간)

    public ScheduleEntry(ActivityType type, int hours)
    {
        activityType = type;
        durationHours = hours;
    }
}

public class ScheduleManager : ManagerBase
{
    public List<ScheduleEntry> weeklySchedule;

    public override void ManagedInitialize()
    {
        Initialize();
        GameEvents.OnDateChanged += OnDateChanged;
    }

    void OnDestroy()
    {
        GameEvents.OnDateChanged -= OnDateChanged;
    }

    private void OnDateChanged(int day, int week, int month, int year)
    {
        // 날짜가 변경되면 해당 요일의 스케줄 처리 이벤트를 발생시킨다.
        int dayOfWeek = (int)GameManager.Instance.GetManager<TimeManager>().CurrentDate.DayOfWeek; // 0: Sunday, 1: Monday...
        ProcessDailySchedule(dayOfWeek);
    }

    public void Initialize()
    {
        weeklySchedule = new List<ScheduleEntry>();
        // 초기 스케줄 설정 (예: 일주일치 빈 스케줄)
        for (int i = 0; i < 7; i++)
        {
            weeklySchedule.Add(new ScheduleEntry(ActivityType.None, 0));
        }
        Debug.Log("ScheduleManager initialized with empty weekly schedule.");
    }

    public void SetDailySchedule(int dayIndex, ActivityType activity, int duration)
    {
        if (dayIndex >= 0 && dayIndex < weeklySchedule.Count)
        {
            weeklySchedule[dayIndex] = new ScheduleEntry(activity, duration);
            Debug.Log($"Day {dayIndex} schedule set to {activity} for {duration} hours.");
        }
        else
        {
            Debug.LogError($"Invalid day index: {dayIndex}");
        }
    }

    public ScheduleEntry GetDailySchedule(int dayIndex)
    {
        if (dayIndex >= 0 && dayIndex < weeklySchedule.Count)
        {
            return weeklySchedule[dayIndex];
        }
        return new ScheduleEntry(ActivityType.None, 0);
    }

    public void ProcessDailySchedule(int dayIndex)
    {
        ScheduleEntry entry = GetDailySchedule(dayIndex);
        Debug.Log($"Processing schedule for Day {dayIndex}: {entry.activityType} for {entry.durationHours} hours.");

        // 직접 CharacterManager를 호출하는 대신, 이벤트를 통해 스케줄 처리를 요청
        GameEvents.OnProcessSchedule?.Invoke(dayIndex);
    }
}
