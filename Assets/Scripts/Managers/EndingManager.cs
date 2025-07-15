using UnityEngine;
using System;

public class EndingManager : ManagerBase
{
    private int lastCheckedYear = -1;

    public override void ManagedInitialize()
    {
        GameEvents.OnDateChanged += OnDateChanged;
        Debug.Log("EndingManager initialized.");
    }

    void OnDestroy()
    {
        GameEvents.OnDateChanged -= OnDateChanged;
    }

    private void OnDateChanged(int day, int week, int month, int year)
    {
        if (year != lastCheckedYear)
        {
            lastCheckedYear = year;
            CheckEndingCondition(year);
        }
    }

    void CheckEndingCondition(int currentYear)
    {
        // 예시: 게임 시작 후 3년이 지나면 엔딩 체크
        if (currentYear >= 2028) // 2025년 시작 -> 2028년은 3년 후
        {
            TriggerEnding();
        }
    }

    void TriggerEnding()
    {
        var characterManager = GameManager.Instance.GetManager<CharacterManager>();
        if (characterManager == null || characterManager.CurrentPlayerData == null) return;

        PlayerData playerData = characterManager.CurrentPlayerData;
        string endingTitle = "";
        string endingDescription = "";

        // 엔딩 조건에 따른 분기
        if (playerData.subscribers >= 1000000 && playerData.fame >= 10000)
        {
            endingTitle = "전설의 VTuber";
            endingDescription = "당신은 전설적인 VTuber가 되어 역사에 이름을 남겼습니다!";
        }
        else if (playerData.subscribers >= 500000 && playerData.singingSkill >= 100)
        {
            endingTitle = "아이돌 데뷔";
            endingDescription = "당신의 뛰어난 노래 실력으로 아이돌로 데뷔하게 되었습니다!";
        }
        else if (playerData.stress >= 80)
        {
            endingTitle = "번아웃";
            endingDescription = "과도한 스트레스로 인해 결국 번아웃이 찾아왔습니다...";
        }
        else
        {
            endingTitle = "평범한 VTuber";
            endingDescription = "당신은 평범하지만 행복한 VTuber 생활을 이어갔습니다.";
        }

        string finalStats = $"최종 구독자: {playerData.subscribers.ToString("N0")}명\n" +
                            $"최종 인지도: {playerData.fame}\n" +
                            $"최종 자금: {playerData.money.ToString("N0")}원";

        GameManager.Instance.GetManager<UIGameManager>()?.ShowEndingScreen(endingTitle, endingDescription, finalStats);
    }
}

