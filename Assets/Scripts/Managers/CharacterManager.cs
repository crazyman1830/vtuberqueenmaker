
using UnityEngine;

public class CharacterManager : ManagerBase
{
    public PlayerData CurrentPlayerData { get; private set; }
    public PlayerCharacterVisuals playerVisuals; // 플레이어 캐릭터의 시각적 요소

    public override void ManagedInitialize()
    {
        LoadPlayerData();
        GameEvents.OnProcessSchedule += ProcessSchedule;
        Debug.Log($"CharacterManager initialized for {CurrentPlayerData.vtuberName}.");
    }

    void OnDestroy()
    {
        GameEvents.OnProcessSchedule -= ProcessSchedule;
    }

    public void LoadPlayerData()
    {
        CurrentPlayerData = new PlayerData("New VTuber");
        NotifyPlayerDataUpdate();
    }

    public void SetPlayerData(PlayerData data)
    {
        CurrentPlayerData = data;
        NotifyPlayerDataUpdate();
    }

    private void NotifyPlayerDataUpdate()
    {
        GameEvents.OnPlayerDataUpdated?.Invoke(CurrentPlayerData);
    }

    public void AddSubscribers(int amount)
    {
        if (CurrentPlayerData == null) return;
        CurrentPlayerData.subscribers += amount;
        NotifyPlayerDataUpdate();
    }

    public void AddMoney(long amount)
    {
        if (CurrentPlayerData == null) return;
        CurrentPlayerData.money += amount;
        NotifyPlayerDataUpdate();
    }

    public void ApplyStress(int amount)
    {
        if (CurrentPlayerData == null) return;
        CurrentPlayerData.stress = Mathf.Clamp(CurrentPlayerData.stress + amount, 0, 100);

        if (playerVisuals != null)
        {
            if (CurrentPlayerData.stress >= 80)
            {
                playerVisuals.SetExpression("Stressed");
            }
            else if (CurrentPlayerData.stress >= 50)
            {
                playerVisuals.SetExpression("Tired");
            }
            else
            {
                playerVisuals.SetExpression("Normal");
            }
        }
        NotifyPlayerDataUpdate();
    }

    private void ProcessSchedule(int dayIndex)
    {
        ScheduleEntry entry = GameManager.Instance.GetManager<ScheduleManager>().GetDailySchedule(dayIndex);

        switch (entry.activityType)
        {
            case ActivityType.BroadcastGame: HandleGameBroadcast(entry.durationHours); break;
            case ActivityType.BroadcastChat: HandleChatBroadcast(entry.durationHours); break;
            case ActivityType.BroadcastSong: HandleSongBroadcast(entry.durationHours); break;
            case ActivityType.BroadcastASMR: HandleASMRBroadcast(entry.durationHours); break;
            case ActivityType.VocalTraining: HandleVocalTraining(entry.durationHours); break;
            case ActivityType.DanceLesson: HandleDanceLesson(entry.durationHours); break;
            case ActivityType.SpeechClass: HandleSpeechClass(entry.durationHours); break;
            case ActivityType.GamePractice: HandleGamePractice(entry.durationHours); break;
            case ActivityType.Rest: HandleRest(entry.durationHours); break;
            case ActivityType.ProduceVideo: HandleProduceVideo(entry.durationHours); break;
            case ActivityType.RecordCoverSong: HandleRecordCoverSong(entry.durationHours); break;
            case ActivityType.ManageSNS: HandleManageSNS(entry.durationHours); break;
            case ActivityType.Collaboration: HandleCollaboration(entry.durationHours); break;
            case ActivityType.Event: HandleOfflineEvent(entry.durationHours); break;
            case ActivityType.GoodsProduction: HandleGoodsProduction(entry.durationHours); break;
        }
        NotifyPlayerDataUpdate(); // 스케줄 처리 후 데이터 변경 알림
    }

    public void HandleGameBroadcast(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 구독자 증가 (게임 실력과 매력에 비례)
        int newSubscribers = (CurrentPlayerData.gameSkill * 2 + CurrentPlayerData.charm) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 상승
        CurrentPlayerData.fame += 5 * duration;

        // 스트레스 증가
        ApplyStress(5 * duration);

        // 자금 획득 (구독자 수에 비례)
        long newMoney = (long)(CurrentPlayerData.subscribers * 0.1f * duration);
        AddMoney(newMoney);

        // 게임 실력 약간 상승
        CurrentPlayerData.gameSkill += 1 * duration;

        Debug.Log($"Game broadcast results: +{newSubscribers} subscribers, +{5 * duration} fame, +{5 * duration} stress, +{newMoney} money, +{1 * duration} game skill");
    }

    public void HandleChatBroadcast(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 구독자 증가 (화술과 매력에 비례)
        int newSubscribers = (CurrentPlayerData.talkSkill * 2 + CurrentPlayerData.charm) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 상승
        CurrentPlayerData.fame += 3 * duration;

        // 스트레스 약간 감소
        ApplyStress(-2 * duration);

        // 자금 획득 (구독자 수에 비례)
        long newMoney = (long)(CurrentPlayerData.subscribers * 0.15f * duration);
        AddMoney(newMoney);

        // 화술 약간 상승
        CurrentPlayerData.talkSkill += 1 * duration;

        Debug.Log($"Chat broadcast results: +{newSubscribers} subscribers, +{3 * duration} fame, {-2 * duration} stress, +{newMoney} money, +{1 * duration} talk skill");
    }

    public void HandleSongBroadcast(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 구독자 증가 (노래 실력과 매력에 비례)
        int newSubscribers = (CurrentPlayerData.singingSkill * 3 + CurrentPlayerData.charm) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 상승
        CurrentPlayerData.fame += 7 * duration;

        // 스트레스 증가
        ApplyStress(7 * duration);

        // 자금 획득 (구독자 수에 비례)
        long newMoney = (long)(CurrentPlayerData.subscribers * 0.2f * duration);
        AddMoney(newMoney);

        // 노래 실력 약간 상승
        CurrentPlayerData.singingSkill += 1 * duration;

        Debug.Log($"Song broadcast results: +{newSubscribers} subscribers, +{7 * duration} fame, +{7 * duration} stress, +{newMoney} money, +{1 * duration} singing skill");
    }

    public void HandleASMRBroadcast(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 구독자 증가 (매력과 화술에 비례, 하지만 다른 방송보다는 적게)
        int newSubscribers = (CurrentPlayerData.charm * 2 + CurrentPlayerData.talkSkill) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 약간 상승
        CurrentPlayerData.fame += 2 * duration;

        // 스트레스 크게 감소
        ApplyStress(-10 * duration);

        // 자금 획득 (구독자 수에 비례, 후원 비율이 높다고 가정)
        long newMoney = (long)(CurrentPlayerData.subscribers * 0.25f * duration);
        AddMoney(newMoney);

        Debug.Log($"ASMR broadcast results: +{newSubscribers} subscribers, +{2 * duration} fame, {-10 * duration} stress, +{newMoney} money");
    }

    public void HandleVocalTraining(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생
        long cost = 5000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for vocal training.");
            return;
        }
        AddMoney(-cost);

        // 노래 실력 상승
        CurrentPlayerData.singingSkill += 3 * duration;

        // 스트레스 증가
        ApplyStress(3 * duration);

        Debug.Log($"Vocal training results: -{cost} money, +{3 * duration} singing skill, +{3 * duration} stress");
    }

    public void HandleDanceLesson(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생
        long cost = 5000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for dance lesson.");
            return;
        }
        AddMoney(-cost);

        // 춤 실력 상승
        CurrentPlayerData.dancingSkill += 3 * duration;

        // 스트레스 증가
        ApplyStress(5 * duration);

        Debug.Log($"Dance lesson results: -{cost} money, +{3 * duration} dancing skill, +{5 * duration} stress");
    }

    public void HandleSpeechClass(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생
        long cost = 3000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for speech class.");
            return;
        }
        AddMoney(-cost);

        // 화술 상승
        CurrentPlayerData.talkSkill += 3 * duration;

        // 스트레스 증가
        ApplyStress(2 * duration);

        Debug.Log($"Speech class results: -{cost} money, +{3 * duration} talk skill, +{2 * duration} stress");
    }

    public void HandleGamePractice(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생 (장비 유지비 등)
        long cost = 1000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for game practice.");
            return;
        }
        AddMoney(-cost);

        // 게임 실력 상승
        CurrentPlayerData.gameSkill += 3 * duration;

        // 스트레스 증가
        ApplyStress(4 * duration);

        Debug.Log($"Game practice results: -{cost} money, +{3 * duration} game skill, +{4 * duration} stress");
    }

    public void HandleRest(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 스트레스 감소
        ApplyStress(-15 * duration);

        Debug.Log($"Rest results: {-15 * duration} stress");
    }

    public void HandleProduceVideo(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생 (편집 프로그램, 소스 등)
        long cost = 2000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for video production.");
            return;
        }
        AddMoney(-cost);

        // 구독자 증가 (매력과 게임/노래/춤 실력 중 가장 높은 것 반영)
        int bestSkill = Mathf.Max(CurrentPlayerData.gameSkill, CurrentPlayerData.singingSkill, CurrentPlayerData.dancingSkill);
        int newSubscribers = (bestSkill + CurrentPlayerData.charm) * duration * 2;
        AddSubscribers(newSubscribers);

        // 인지도 상승
        CurrentPlayerData.fame += 10 * duration;

        // 스트레스 증가
        ApplyStress(8 * duration);

        Debug.Log($"Video production results: -{cost} money, +{newSubscribers} subscribers, +{10 * duration} fame, +{8 * duration} stress");
    }

    public void HandleRecordCoverSong(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 비용 발생 (녹음 스튜디오, 믹싱 등)
        long cost = 10000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for recording a cover song.");
            return;
        }
        AddMoney(-cost);

        // 구독자 증가 (노래 실력과 매력에 크게 좌우)
        int newSubscribers = (CurrentPlayerData.singingSkill * 5 + CurrentPlayerData.charm * 2) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 크게 상승
        CurrentPlayerData.fame += 15 * duration;

        // 스트레스 크게 증가
        ApplyStress(12 * duration);

        // 노래 실력 상승
        CurrentPlayerData.singingSkill += 2 * duration;

        Debug.Log($"Cover song recording results: -{cost} money, +{newSubscribers} subscribers, +{15 * duration} fame, +{12 * duration} stress, +{2 * duration} singing skill");
    }

    public void HandleManageSNS(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 구독자 증가 (매력과 화술에 비례)
        int newSubscribers = (CurrentPlayerData.charm + CurrentPlayerData.talkSkill) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 약간 상승
        CurrentPlayerData.fame += 1 * duration;

        // 스트레스 약간 감소
        ApplyStress(-1 * duration);

        Debug.Log($"SNS management results: +{newSubscribers} subscribers, +{1 * duration} fame, {-1 * duration} stress");
    }

    public void HandleCollaboration(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 인지도가 낮으면 콜라보 불가
        if (CurrentPlayerData.fame < 100)
        {
            Debug.Log("Not famous enough for a collaboration.");
            return;
        }

        // 구독자 크게 증가 (인지도와 매력에 비례)
        int newSubscribers = (CurrentPlayerData.fame + CurrentPlayerData.charm * 3) * duration;
        AddSubscribers(newSubscribers);

        // 인지도 크게 상승
        CurrentPlayerData.fame += 20 * duration;

        // 스트레스 증가
        ApplyStress(10 * duration);

        Debug.Log($"Collaboration results: +{newSubscribers} subscribers, +{20 * duration} fame, +{10 * duration} stress");
    }

    public void HandleOfflineEvent(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 인지도와 구독자 수가 일정 수준 이상이어야 개최 가능
        if (CurrentPlayerData.fame < 500 || CurrentPlayerData.subscribers < 10000)
        {
            Debug.Log("Not popular enough for an offline event.");
            return;
        }

        // 비용 발생
        long cost = 50000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for an offline event.");
            return;
        }
        AddMoney(-cost);

        // 구독자, 인지도, 자금 크게 증가
        int newSubscribers = CurrentPlayerData.subscribers * duration / 2;
        AddSubscribers(newSubscribers);
        CurrentPlayerData.fame += 50 * duration;
        long newMoney = cost * 2; // 티켓 판매 수익
        AddMoney(newMoney);

        // 스트레스 매우 크게 증가
        ApplyStress(50 * duration);

        Debug.Log($"Offline event results: -{cost} money, +{newSubscribers} subscribers, +{50 * duration} fame, +{newMoney} money, +{50 * duration} stress");
    }

    public void HandleGoodsProduction(int duration)
    {
        if (CurrentPlayerData == null) return;

        // 인지도가 낮으면 굿즈 제작 불가
        if (CurrentPlayerData.fame < 200)
        {
            Debug.Log("Not famous enough for goods production.");
            return;
        }

        // 비용 발생
        long cost = 10000 * duration;
        if (CurrentPlayerData.money < cost)
        {
            Debug.Log("Not enough money for goods production.");
            return;
        }
        AddMoney(-cost);

        // 자금 증가 (인지도와 구독자 수에 비례)
        long newMoney = (long)((CurrentPlayerData.fame + CurrentPlayerData.subscribers / 10) * 1.5f * duration);
        AddMoney(newMoney);

        // 스트레스 증가
        ApplyStress(15 * duration);

        Debug.Log($"Goods production results: -{cost} money, +{newMoney} money, +{15 * duration} stress");
    }
}
