using System;

// 게임의 주요 이벤트를 관리하는 정적 클래스
public static class GameEvents
{
    // 플레이어 데이터가 변경될 때 발생하는 이벤트
    // PlayerData를 매개변수로 전달하여 UI 등에서 쉽게 접근하도록 함
    public static Action<PlayerData> OnPlayerDataUpdated;

    // 날짜가 변경될 때 발생하는 이벤트
    // 년, 월, 일, 주 정보를 전달
    public static Action<int, int, int, int> OnDateChanged;

    // 스케줄을 실행하라는 이벤트
    // 실행할 요일(0-6)을 전달
    public static Action<int> OnProcessSchedule;

    // 게임 저장/불러오기 관련 이벤트
    public static Action OnSaveGame;
    public static Action OnLoadGame;
}
