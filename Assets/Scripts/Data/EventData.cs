
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Random,
    Scheduled,
    SpecialBroadcast
}

[Serializable]
public class EventEffect
{
    public string parameterName; // PlayerData의 파라미터 이름
    public int value;            // 변화량
}

[Serializable]
public class EventData
{
    public string eventName;
    public string eventDescription;
    public EventType eventType;
    public List<EventEffect> effects;
    public int triggerMonth; // 정기 이벤트 발생 월
    public int triggerDay;   // 정기 이벤트 발생 일
    public Sprite eventCG;   // 이벤트 CG 스프라이트 (선택 사항)

    // TODO: 이벤트 발생 조건 추가 (예: 특정 능력치 이상/이하, 특정 날짜)
    // public List<EventCondition> conditions;

    public EventData(string name, string description, EventType type, List<EventEffect> effects, int triggerMonth = 0, int triggerDay = 0, Sprite eventCG = null)
    {
        this.eventName = name;
        this.eventDescription = description;
        this.eventType = type;
        this.effects = effects;
        this.triggerMonth = triggerMonth;
        this.triggerDay = triggerDay;
        this.eventCG = eventCG;
    }
}
