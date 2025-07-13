
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class EventPopupUI : MonoBehaviour
{
    public TextMeshProUGUI eventTitleText;
    public TextMeshProUGUI eventDescriptionText;
    public Button closeButton;

    // TODO: 선택지 버튼들을 위한 리스트 추가
    // public List<Button> choiceButtons;

    public void ShowEvent(EventData eventData)
    {
        eventTitleText.text = eventData.eventName;
        eventDescriptionText.text = eventData.eventDescription;
        gameObject.SetActive(true);
    }

    public void HideEvent()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        closeButton.onClick.AddListener(HideEvent);
        // TODO: 선택지 버튼들에 리스너 추가
    }
}
