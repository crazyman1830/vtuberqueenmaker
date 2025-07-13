
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScheduleDayUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI activityText;
    public Button dayButton;

    private int dayIndex;
    private Action<int> onDaySelected;

    public void Initialize(int index, Action<int> selectionCallback)
    {
        this.dayIndex = index;
        this.onDaySelected = selectionCallback;
        dayText.text = GetDayName(index);
        dayButton.onClick.AddListener(() => onDaySelected?.Invoke(dayIndex));
    }

    public void UpdateActivity(ActivityType activityType)
    {
        activityText.text = activityType.ToString();
    }

    private string GetDayName(int index)
    {
        switch (index)
        {
            case 0: return "월";
            case 1: return "화";
            case 2: return "수";
            case 3: return "목";
            case 4: return "금";
            case 5: return "토";
            case 6: return "일";
            default: return "";
        }
    }
}
