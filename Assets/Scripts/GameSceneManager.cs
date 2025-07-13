
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private bool isPaused = false;

    [Header("Audio")]
    public AudioClip gameBGM;

    void Start()
    {
        InitializeGameScene();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.TimeManager != null)
        {
            GameManager.Instance.TimeManager.OnDayChanged -= HandleDayChanged;
        }
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }
    }

    private void InitializeGameScene()
    {
        Debug.Log("Welcome to the Game Scene!");

        // GameManager에서 매니저 인스턴스 가져오기
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        // UIManager 초기화 (GameManager에서 이미 초기화되지만, 씬에 있는 UIManager 컴포넌트가 있다면 연결)
        if (GameManager.Instance.UIGameManager != null)
        {
            // UIManager는 GameManager에서 관리하므로, 씬에 있는 UIManager 컴포넌트를 직접 초기화할 필요 없음
            // 다만, 씬에 있는 UI 요소들을 GameManager.Instance.UIGameManager에 연결하는 작업은 필요할 수 있음
        }
        else
        {
            Debug.LogError("UIGameManager instance not found in GameManager!");
        }

        // TimeManager 이벤트 구독
        GameManager.Instance.TimeManager.OnDayChanged += HandleDayChanged;

        // BGM 재생
        if (gameBGM != null)
        {
            GameManager.Instance.SoundManager.PlayBGM(gameBGM);
        }

        // 초기 UI 업데이트
        HandleDayChanged(GameManager.Instance.TimeManager.CurrentDay, GameManager.Instance.TimeManager.CurrentWeek, GameManager.Instance.TimeManager.CurrentMonth, GameManager.Instance.TimeManager.CurrentYear);
    }

    private void HandleDayChanged(int day, int week, int month, int year)
    {
        // 현재 스케줄에 따라 배경 변경
        ScheduleEntry currentSchedule = GameManager.Instance.ScheduleManager.GetDailySchedule(GameManager.Instance.TimeManager.CurrentDate.DayOfWeek == System.DayOfWeek.Sunday ? 6 : (int)GameManager.Instance.TimeManager.CurrentDate.DayOfWeek - 1);
        
        Sprite newBackground = GameManager.Instance.BackgroundManager.GetBackgroundSprite(currentSchedule.activityType);
        if (newBackground != null)
        {
            GameManager.Instance.BackgroundManager.SetBackground(newBackground);
        }

        // 스케줄 처리
        GameManager.Instance.ScheduleManager.ProcessDailySchedule(GameManager.Instance.TimeManager.CurrentDate.DayOfWeek == System.DayOfWeek.Sunday ? 6 : (int)GameManager.Instance.TimeManager.CurrentDate.DayOfWeek - 1);
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        Debug.Log($"Game paused: {isPaused}");
    }
}