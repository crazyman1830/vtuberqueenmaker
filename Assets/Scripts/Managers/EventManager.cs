
using UnityEngine;
using System.Collections.Generic;

public class EventManager : ManagerBase
{
    public List<EventData> allEvents;
    public List<RivalData> allRivals;
    public List<StaffData> allStaff;

    public override void ManagedInitialize()
    {
        LoadAllEvents();
        LoadAllRivals();
        LoadAllStaff();
        GameEvents.OnDateChanged += CheckAndTriggerEvents;
        Debug.Log("EventManager initialized.");
    }

    void OnDestroy()
    {
        GameEvents.OnDateChanged -= CheckAndTriggerEvents;
    }

    void LoadAllEvents()
    {
        // TODO: JSON 또는 ScriptableObject로부터 모든 이벤트 데이터를 불러오는 로직 구현
        allEvents = new List<EventData>();

        // 예시 랜덤 이벤트 추가
        allEvents.Add(new EventData("영상 바이럴", "제작한 영상 중 하나가 갑자기 바이럴을 타기 시작했다!", EventType.Random,
            new List<EventEffect> { new EventEffect { parameterName = "subscribers", value = 5000 }, new EventEffect { parameterName = "fame", value = 100 } }));

        allEvents.Add(new EventData("악플 테러", "알 수 없는 이유로 악플 테러를 당했다...", EventType.Random,
            new List<EventEffect> { new EventEffect { parameterName = "stress", value = 50 }, new EventEffect { parameterName = "subscribers", value = -1000 } }));

        // 예시 정기 이벤트 추가 (생일 등)
        allEvents.Add(new EventData("VTuber 생일", "당신의 VTuber가 생일을 맞이했습니다!", EventType.Scheduled,
            new List<EventEffect> { new EventEffect { parameterName = "fame", value = 50 }, new EventEffect { parameterName = "subscribers", value = 1000 } }, 7, 11)); // 7월 11일

        // 예시 특수 방송 이벤트 추가
        allEvents.Add(new EventData("대규모 도네이션", "방송 중 엄청난 금액의 도네이션이 터졌다!", EventType.SpecialBroadcast,
            new List<EventEffect> { new EventEffect { parameterName = "money", value = 100000 }, new EventEffect { parameterName = "fame", value = 200 } }));
        allEvents.Add(new EventData("유명인 등장", "유명 스트리머가 당신의 방송에 나타났다!", EventType.SpecialBroadcast,
            new List<EventEffect> { new EventEffect { parameterName = "subscribers", value = 10000 }, new EventEffect { parameterName = "fame", value = 500 } }));

        // 라이벌 이벤트
        allEvents.Add(new EventData("라이벌의 도전", "미나토 아쿠아가 당신에게 콜라보를 제안했다!", EventType.Random,
            new List<EventEffect> { }, eventCG: null)); // CG는 나중에 추가
        allEvents.Add(new EventData("라이벌의 견제", "호시마치 스이세이가 당신을 견제하는 발언을 했다!", EventType.Random,
            new List<EventEffect> { new EventEffect { parameterName = "stress", value = 15 } }));

        // 스태프 이벤트
        allEvents.Add(new EventData("매니저의 격려", "김매니저가 당신의 노력을 칭찬하며 격려했다.", EventType.Random,
            new List<EventEffect> { new EventEffect { parameterName = "stress", value = -10 } }));
        allEvents.Add(new EventData("코디의 제안", "박코디가 새로운 의상 컨셉을 제안했다.", EventType.Random,
            new List<EventEffect> { }));
    }

    void LoadAllRivals()
    {
        allRivals = new List<RivalData>();
        // TODO: 실제 스프라이트 에셋 연결
        allRivals.Add(new RivalData("미나토 아쿠아", 1500000, 8000, "활발한 게임 방송으로 유명한 VTuber", null)); 
        allRivals.Add(new RivalData("호시마치 스이세이", 2000000, 10000, "뛰어난 노래 실력을 가진 아이돌 VTuber", null));
    }

    void LoadAllStaff()
    {
        allStaff = new List<StaffData>();
        // TODO: 실제 스프라이트 에셋 연결
        allStaff.Add(new StaffData("김매니저", "매니저", "당신의 스케줄을 관리하는 유능한 매니저", null));
        allStaff.Add(new StaffData("박코디", "코디네이터", "당신의 의상을 담당하는 코디네이터", null));
    }

    void CheckAndTriggerEvents(int day, int week, int month, int year)
    {
        var characterManager = GameManager.Instance.GetManager<CharacterManager>();
        if (characterManager == null || characterManager.CurrentPlayerData == null) return;

        // 매일 일정 확률로 랜덤 이벤트 발생
        if (UnityEngine.Random.Range(0, 100) < 10) // 10% 확률
        {
            TriggerRandomEvent();
        }

        // 정기 이벤트 체크 로직 (날짜 기반)
        foreach (var eventData in allEvents)
        {
            if (eventData.eventType == EventType.Scheduled &&
                eventData.triggerMonth == month &&
                eventData.triggerDay == day)
            {
                TriggerEvent(eventData);
            }
        }

        // 라이벌 이벤트 체크 (예: 플레이어의 구독자 수가 라이벌의 80%에 도달하면 경쟁 이벤트 발생)
        foreach (var rival in allRivals)
        {
            if (characterManager.CurrentPlayerData.subscribers >= rival.subscribers * 0.8f &&
                characterManager.CurrentPlayerData.subscribers < rival.subscribers) // 라이벌에 근접했을 때
            {
                if (UnityEngine.Random.Range(0, 100) < 5) // 5% 확률
                {
                    TriggerRivalEvent(rival);
                }
            }
        }

        // 스태프 이벤트 체크 (예: 스트레스가 높을 때 매니저가 조언)
        if (characterManager.CurrentPlayerData.stress >= 70)
        {
            if (UnityEngine.Random.Range(0, 100) < 15) // 15% 확률
            {
                TriggerStaffEvent(allStaff[0]); // 김매니저 이벤트
            }
        }
    }

    public void TriggerSpecialBroadcastEvent()
    {
        List<EventData> specialBroadcastEvents = allEvents.FindAll(e => e.eventType == EventType.SpecialBroadcast);
        if (specialBroadcastEvents.Count == 0) return;

        EventData selectedEvent = specialBroadcastEvents[UnityEngine.Random.Range(0, specialBroadcastEvents.Count)];
        TriggerEvent(selectedEvent);
    }

    void TriggerRandomEvent()
    {
        if (allEvents.Count == 0) return;

        List<EventData> randomEvents = allEvents.FindAll(e => e.eventType == EventType.Random);
        if (randomEvents.Count == 0) return;

        EventData selectedEvent = randomEvents[UnityEngine.Random.Range(0, randomEvents.Count)];
        TriggerEvent(selectedEvent);
    }

    void TriggerRivalEvent(RivalData rival)
    {
        var dialogueManager = GameManager.Instance.GetManager<DialogueManager>();
        // 라이벌 이벤트 생성 (예시)
        if (rival.rivalName == "미나토 아쿠아")
        {
            dialogueManager.StartDialogue("rival_aquas_challenge");
        }
        else if (rival.rivalName == "호시마치 스이세이")
        {
            dialogueManager.StartDialogue("rival_suiseis_check");
        }
    }

    void TriggerStaffEvent(StaffData staff)
    {
        var dialogueManager = GameManager.Instance.GetManager<DialogueManager>();
        // 스태프 이벤트 생성 (예시)
        if (staff.staffName == "김매니저")
        {
            dialogueManager.StartDialogue("manager_encouragement");
        }
        else if (staff.staffName == "박코디")
        {
            dialogueManager.StartDialogue("coordinator_suggestion");
        }
    }

    public void TriggerEvent(EventData eventData)
    {
        Debug.Log($"Event Triggered: {eventData.eventName} - {eventData.eventDescription}");

        GameManager.Instance.GetManager<UIGameManager>()?.ShowEventPopup(eventData);

        // 이벤트 CG 표시
        if (eventData.eventCG != null)
        {
            GameManager.Instance.GetManager<EventCGManager>()?.ShowCG(eventData.eventCG);
        }

        // 이벤트 효과 적용
        foreach (var effect in eventData.effects)
        {
            ApplyEventEffect(effect);
        }
    }

    void ApplyEventEffect(EventEffect effect)
    {
        var characterManager = GameManager.Instance.GetManager<CharacterManager>();
        if (characterManager == null || characterManager.CurrentPlayerData == null) return;

        PlayerData playerData = characterManager.CurrentPlayerData;

        // 리플렉션을 사용하거나, switch-case 구문으로 파라미터 직접 변경
        switch (effect.parameterName)
        {
            case "subscribers":
                playerData.subscribers += effect.value;
                break;
            case "fame":
                playerData.fame += effect.value;
                break;
            case "stress":
                playerData.stress = Mathf.Clamp(playerData.stress + effect.value, 0, 100);
                break;
            case "money":
                playerData.money += effect.value;
                break;
            // 다른 파라미터들에 대한 처리 추가
        }
        // 이벤트 효과 적용 후 데이터 변경 알림
        GameEvents.OnPlayerDataUpdated?.Invoke(playerData);
    }
}
