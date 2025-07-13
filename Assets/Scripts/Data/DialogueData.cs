
using System;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public string speakerName; // 화자 이름 (예: "VTuber", "매니저", "나레이션")
    public string dialogueText; // 대화 내용
    public UnityEngine.Sprite speakerPortrait; // 화자 초상화 스프라이트
    public UnityEngine.AudioClip voiceClip; // 캐릭터 음성 클립
    // TODO: 화자 이미지/표정, 배경 변경 등 시각적 요소 추가
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText; // 선택지 텍스트
    public List<EventEffect> effects; // 선택 시 발생할 효과
    public string nextDialogueID; // 선택 시 이어질 다음 대화의 ID (선택 사항)
    // TODO: 특정 조건 등 추가
}

[System.Serializable]
public class DialogueData
{
    public string dialogueID; // 대화의 고유 ID
    public List<DialogueLine> lines; // 대화 라인 목록
    public List<DialogueChoice> choices; // 선택지 목록 (선택 사항)
    public bool isEventDialogue; // 이벤트 대화인지 여부

    public DialogueData(string id, List<DialogueLine> lines, List<DialogueChoice> choices = null, bool isEventDialogue = false)
    {
        this.dialogueID = id;
        this.lines = lines;
        this.choices = choices;
        this.isEventDialogue = isEventDialogue;
    }
}
