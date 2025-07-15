
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIGameManager : ManagerBase
{
    [Header("Player Status UI")]
    public TextMeshProUGUI subscriberCountText;
    public Image healthBar;
    public TextMeshProUGUI stressLevelText;

    [Header("Game Info UI")]
    public TextMeshProUGUI dateTimeText;
    public TextMeshProUGUI moneyText;

    [Header("Panels & Popups")]
    public GameObject schedulePanel;
    public GameObject activitySelectionPopup;
    public GameObject settingsPopup;
    public GameObject statusPanel;

    [Header("Player Detailed Status UI")]
    public TextMeshProUGUI detailedSubscriberCountText;
    public TextMeshProUGUI talkSkillText;
    public TextMeshProUGUI gameSkillText;
    public TextMeshProUGUI singingSkillText;
    public TextMeshProUGUI dancingSkillText;
    public TextMeshProUGUI charmText;
    public TextMeshProUGUI fameText;
    public TextMeshProUGUI detailedStressLevelText;
    public TextMeshProUGUI detailedMoneyText;

    [Header("Popups")]
    public EventPopupUI eventPopupUI;
    public Button saveButton;

    [Header("Shop UI")]
    public ShopUI shopUI;
    public Button shopButton;

    [Header("Status UI")]
    public Button statusButton;

    [Header("Ending UI")]
    public EndingUI endingUI;

    [Header("Dialogue UI")]
    public DialogueUI dialogueUI;

    [Header("Schedule UI")]
    public List<ScheduleDayUI> scheduleDays;
    public List<Button> activitySelectionButtons;

    private int selectedDayIndex = -1;

    public override void ManagedInitialize()
    {
        if (schedulePanel != null) schedulePanel.SetActive(false);
        if (activitySelectionPopup != null) activitySelectionPopup.SetActive(false);
        if (settingsPopup != null) settingsPopup.SetActive(false);
        if (statusPanel != null) statusPanel.SetActive(false);
        if (eventPopupUI != null) eventPopupUI.gameObject.SetActive(false);

        for (int i = 0; i < scheduleDays.Count; i++)
        {
            int dayIndex = i;
            scheduleDays[i].Initialize(dayIndex, OnDaySelected);
        }

        if (saveButton != null) saveButton.onClick.AddListener(() => GameEvents.OnSaveGame?.Invoke());

        // 활동 선택 팝업의 버튼들에 리스너 추가
        for (int i = 0; i < activitySelectionButtons.Count; i++)
        {
            int activityIndex = i + 1; // ActivityType.None (0)을 건너뛰고 시작
            ActivityType activityType = (ActivityType)activityIndex;
            activitySelectionButtons[i].onClick.AddListener(() => OnActivitySelected(activityType));
        }

        // TODO: 다른 UI들도 Initialize()를 ManagedInitialize() 패턴으로 바꿀 수 있음
        if (eventPopupUI != null) eventPopupUI.Initialize();
        if (shopUI != null) shopUI.Initialize();
        if (endingUI != null) endingUI.Initialize();
        if (dialogueUI != null) dialogueUI.Initialize();

        if (shopButton != null) shopButton.onClick.AddListener(shopUI.ShowShop);
        if (statusButton != null) statusButton.onClick.AddListener(() =>
        {
            ToggleStatusPanel(true);
            // CharacterManager의 데이터는 이벤트를 통해 항상 최신 상태이므로, 직접 가져와서 업데이트
            UpdateDetailedPlayerStatsUI(GameManager.Instance.GetManager<CharacterManager>().CurrentPlayerData);
        });

        // 이벤트 구독
        GameEvents.OnPlayerDataUpdated += UpdatePlayerStatsUI;
        GameEvents.OnPlayerDataUpdated += UpdateDetailedPlayerStatsUI;
        GameEvents.OnDateChanged += OnDateChanged_UpdateUI;

        // DialogueManager는 UIGameManager가 직접 참조할 필요 없이, DialogueUI가 스스로 이벤트를 구독/해제하도록 만드는 것이 더 좋음 (추가 개선사항)
        GameManager.Instance.GetManager<DialogueManager>().OnDialogueStart += dialogueUI.ShowDialogue;
        GameManager.Instance.GetManager<DialogueManager>().OnDialogueEnd += dialogueUI.HideDialogue;

        // 초기 스케줄 UI 업데이트
        UpdateWeeklyScheduleUI();

        Debug.Log("UIGameManager Initialized.");

        // 초기 UI 업데이트 (캐릭터 데이터가 로드된 후 이벤트를 통해 자동으로 호출될 것임)
        var timeManager = GameManager.Instance.GetManager<TimeManager>();
        UpdateDateTimeUI(timeManager.CurrentDate.ToShortDateString());
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        GameEvents.OnPlayerDataUpdated -= UpdatePlayerStatsUI;
        GameEvents.OnPlayerDataUpdated -= UpdateDetailedPlayerStatsUI;
        GameEvents.OnDateChanged -= OnDateChanged_UpdateUI;

        if (GameManager.Instance != null)
        {
            var dialogueManager = GameManager.Instance.GetManager<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.OnDialogueStart -= dialogueUI.ShowDialogue;
                dialogueManager.OnDialogueEnd -= dialogueUI.HideDialogue;
            }
        }
    }

    private void OnDateChanged_UpdateUI(int day, int week, int month, int year)
    {
        var timeManager = GameManager.Instance.GetManager<TimeManager>();
        UpdateDateTimeUI(timeManager.CurrentDate.ToShortDateString());
    }

    void OnDaySelected(int dayIndex)
    {
        this.selectedDayIndex = dayIndex;
        activitySelectionPopup.SetActive(true);
        Debug.Log($"Day {dayIndex} selected. Opening activity selection popup.");
    }

    public void OnActivitySelected(ActivityType activity)
    {
        if (selectedDayIndex != -1)
        {
            GameManager.Instance.GetManager<ScheduleManager>().SetDailySchedule(selectedDayIndex, activity, 8); // 8시간으로 고정
            scheduleDays[selectedDayIndex].UpdateActivity(activity);
            activitySelectionPopup.SetActive(false);
            selectedDayIndex = -1;
            UpdateWeeklyScheduleUI(); // 스케줄 변경 후 UI 업데이트
        }
    }

    public void UpdateWeeklyScheduleUI()
    {
        var scheduleManager = GameManager.Instance.GetManager<ScheduleManager>();
        for (int i = 0; i < scheduleDays.Count; i++)
        {
            ScheduleEntry entry = scheduleManager.GetDailySchedule(i);
            scheduleDays[i].UpdateActivity(entry.activityType);
        }
    }

    public void UpdatePlayerStatsUI(PlayerData data)
    {
        if (subscriberCountText != null) subscriberCountText.text = $"구독자: {data.subscribers.ToString("N0")}명";
        if (stressLevelText != null) stressLevelText.text = $"스트레스: {data.stress}%";
        if (moneyText != null) moneyText.text = $"자금: {data.money.ToString("N0")}원";
        // Health bar update would involve setting fill amount based on a max health value
        // if (healthBar != null) healthBar.fillAmount = (float)data.currentHealth / data.maxHealth;
    }

    public void UpdateSubscriberUI(int count)
    {
        if (subscriberCountText != null)
        {
            subscriberCountText.text = $"구독자: {count.ToString("N0")}명";
        }
    }

    public void UpdateDateTimeUI(string dateTime)
    {
        if (dateTimeText != null)
        {
            dateTimeText.text = dateTime;
        }
    }

    public void ToggleSchedulePanel(bool show)
    {
        if (schedulePanel != null)
        {
            schedulePanel.SetActive(show);
        }
    }

    public void ToggleStatusPanel(bool show)
    {
        if (statusPanel != null)
        {
            statusPanel.SetActive(show);
        }
    }

    public void UpdateDetailedPlayerStatsUI(PlayerData data)
    {
        if (detailedSubscriberCountText != null) detailedSubscriberCountText.text = $"구독자: {data.subscribers.ToString("N0")}명";
        if (talkSkillText != null) talkSkillText.text = $"화술: {data.talkSkill}";
        if (gameSkillText != null) gameSkillText.text = $"게임 실력: {data.gameSkill}";
        if (singingSkillText != null) singingSkillText.text = $"노래 실력: {data.singingSkill}";
        if (dancingSkillText != null) dancingSkillText.text = $"춤 실력: {data.dancingSkill}";
        if (charmText != null) charmText.text = $"매력: {data.charm}";
        if (fameText != null) fameText.text = $"인지도: {data.fame}";
        if (detailedStressLevelText != null) detailedStressLevelText.text = $"스트레스: {data.stress}%";
        if (detailedMoneyText != null) detailedMoneyText.text = $"자금: {data.money.ToString("N0")}원";
    }

    public void ShowEndingScreen(string title, string description, string finalStats)
    {
        if (endingUI != null)
        {
            endingUI.ShowEnding(title, description, finalStats);
        }
    }

    public void ShowEventPopup(EventData eventData)
    {
        if (eventPopupUI != null)
        {
            eventPopupUI.ShowEvent(eventData);
        }
    }
}
