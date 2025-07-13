
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

public class ScheduleManager : MonoBehaviour
{
    public List<ScheduleEntry> weeklySchedule;

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

        switch (entry.activityType)
        {
            case ActivityType.BroadcastGame:
                GameManager.Instance.CharacterManager.HandleGameBroadcast(entry.durationHours);
                break;
            case ActivityType.BroadcastChat:
                GameManager.Instance.CharacterManager.HandleChatBroadcast(entry.durationHours);
                break;
            case ActivityType.BroadcastSong:
                GameManager.Instance.CharacterManager.HandleSongBroadcast(entry.durationHours);
                break;
            case ActivityType.BroadcastASMR:
                GameManager.Instance.CharacterManager.HandleASMRBroadcast(entry.durationHours);
                break;
            case ActivityType.VocalTraining:
                GameManager.Instance.CharacterManager.HandleVocalTraining(entry.durationHours);
                break;
            case ActivityType.DanceLesson:
                GameManager.Instance.CharacterManager.HandleDanceLesson(entry.durationHours);
                break;
            case ActivityType.SpeechClass:
                GameManager.Instance.CharacterManager.HandleSpeechClass(entry.durationHours);
                break;
            case ActivityType.GamePractice:
                GameManager.Instance.CharacterManager.HandleGamePractice(entry.durationHours);
                break;
            case ActivityType.Rest:
                GameManager.Instance.CharacterManager.HandleRest(entry.durationHours);
                break;
            case ActivityType.ProduceVideo:
                GameManager.Instance.CharacterManager.HandleProduceVideo(entry.durationHours);
                break;
            case ActivityType.RecordCoverSong:
                GameManager.Instance.CharacterManager.HandleRecordCoverSong(entry.durationHours);
                break;
            case ActivityType.ManageSNS:
                GameManager.Instance.CharacterManager.HandleManageSNS(entry.durationHours);
                break;
            case ActivityType.Collaboration:
                GameManager.Instance.CharacterManager.HandleCollaboration(entry.durationHours);
                break;
            case ActivityType.Event:
                GameManager.Instance.CharacterManager.HandleOfflineEvent(entry.durationHours);
                break;
            case ActivityType.GoodsProduction:
                GameManager.Instance.CharacterManager.HandleGoodsProduction(entry.durationHours);
                break;
            // 다른 활동들에 대한 처리 추가
        }
    }
}
