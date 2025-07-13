
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public UnityEngine.UI.Image speakerPortraitImage;
    public Button nextButton;
    public Transform choiceButtonContainer;
    public GameObject choiceButtonPrefab;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    public void Initialize()
    {
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    public void ShowDialogue(DialogueData dialogueData)
    {
        currentDialogue = dialogueData;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearChoiceButtons();
    }

    void DisplayCurrentLine()
    {
        if (currentLineIndex < currentDialogue.lines.Count)
        {
            DialogueLine currentLine = currentDialogue.lines[currentLineIndex];
            speakerNameText.text = currentLine.speakerName;
            dialogueText.text = currentLine.dialogueText;

            if (speakerPortraitImage != null)
            {
                speakerPortraitImage.sprite = currentLine.speakerPortrait;
                speakerPortraitImage.gameObject.SetActive(currentLine.speakerPortrait != null);
            }

            if (currentLine.voiceClip != null)
            {
                GameManager.Instance.SoundManager.PlayVoice(currentLine.voiceClip);
            }

            // 선택지 버튼 초기화
            ClearChoiceButtons();
            if (currentLineIndex == currentDialogue.lines.Count - 1 && currentDialogue.choices != null && currentDialogue.choices.Count > 0)
            {
                foreach (var choice in currentDialogue.choices)
                {
                    GameObject choiceButtonGO = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                    choiceButtonGO.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
                    choiceButtonGO.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choice));
                }
                nextButton.gameObject.SetActive(false); // 선택지가 있으면 다음 버튼 비활성화
            }
            else
            {
                nextButton.gameObject.SetActive(true);
            }
        }
        else
        {
            GameManager.Instance.DialogueManager.EndDialogue();
            dialoguePanel.SetActive(false);
        }
    }

    void OnNextButtonClicked()
    {
        currentLineIndex++;
        DisplayCurrentLine();
    }

    void OnChoiceSelected(DialogueChoice choice)
    {
        // 선택지 효과 적용
        foreach (var effect in choice.effects)
        {
            PlayerData playerData = GameManager.Instance.CharacterManager.CurrentPlayerData;
            switch (effect.parameterName)
            {
                case "stress":
                    playerData.stress = Mathf.Clamp(playerData.stress + effect.value, 0, 100);
                    break;
                case "fame":
                    playerData.fame += effect.value;
                    break;
                // 다른 파라미터들에 대한 처리 추가
            }
        }

        // 다음 대화로 분기
        if (!string.IsNullOrEmpty(choice.nextDialogueID))
        {
            GameManager.Instance.DialogueManager.StartDialogue(choice.nextDialogueID);
        }
        else
        {
            GameManager.Instance.DialogueManager.EndDialogue();
        }
        dialoguePanel.SetActive(false);
    }

    void ClearChoiceButtons()
    {
        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
