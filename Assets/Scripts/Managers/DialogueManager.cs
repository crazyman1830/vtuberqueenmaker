
using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueManager : ManagerBase
{
    public event Action<DialogueData> OnDialogueStart;
    public event Action OnDialogueEnd;

    private List<DialogueData> allDialogues;

    public override void ManagedInitialize()
    {
        LoadAllDialogues();
        Debug.Log("DialogueManager initialized.");
    }

    void LoadAllDialogues()
    {
        // TODO: JSON 또는 ScriptableObject로부터 모든 대화 데이터를 불러오는 로직 구현
        allDialogues = new List<DialogueData>();

        // 예시 대화 추가
        List<DialogueLine> introLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "나레이션", dialogueText = "VTuber로서의 첫 걸음을 내딛는 당신...", speakerPortrait = null },
            new DialogueLine { speakerName = "VTuber", dialogueText = "안녕하세요! 오늘부터 VTuber 활동을 시작합니다!", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("intro_dialogue", introLines));

        List<DialogueLine> stressLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "VTuber", dialogueText = "으으... 너무 힘들다...", speakerPortrait = null },
            new DialogueLine { speakerName = "매니저", dialogueText = "스트레스가 너무 높네요. 휴식이 필요해요.", speakerPortrait = null }
        };
        List<DialogueChoice> stressChoices = new List<DialogueChoice>
        {
            new DialogueChoice { choiceText = "휴식한다", effects = new List<EventEffect> { new EventEffect { parameterName = "stress", value = -20 } }, nextDialogueID = "rest_outcome" },
            new DialogueChoice { choiceText = "더 일한다", effects = new List<EventEffect> { new EventEffect { parameterName = "stress", value = 10 }, new EventEffect { parameterName = "fame", value = 5 } }, nextDialogueID = "work_outcome" }
        };
        allDialogues.Add(new DialogueData("stress_dialogue", stressLines, stressChoices));

        // 휴식 선택 시 결과 대화
        List<DialogueLine> restOutcomeLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "나레이션", dialogueText = "충분한 휴식으로 스트레스가 해소되었다.", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("rest_outcome", restOutcomeLines));

        // 더 일하기 선택 시 결과 대화
        List<DialogueLine> workOutcomeLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "나레이션", dialogueText = "무리하게 일한 결과, 스트레스는 쌓였지만 인지도가 상승했다.", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("work_outcome", workOutcomeLines));

        // 라이벌 이벤트: 미나토 아쿠아의 도전
        List<DialogueLine> aquaChallengeLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "미나토 아쿠아", dialogueText = "안녕! 너 요즘 잘 나간다며? 나랑 게임 합방 한 번 할래?", speakerPortrait = null }
        };
        List<DialogueChoice> aquaChallengeChoices = new List<DialogueChoice>
        {
            new DialogueChoice { choiceText = "수락한다 (구독자, 인지도 상승)", effects = new List<EventEffect> { new EventEffect { parameterName = "subscribers", value = 10000 }, new EventEffect { parameterName = "fame", value = 300 } }, nextDialogueID = "aqua_collab_success" },
            new DialogueChoice { choiceText = "거절한다 (스트레스 감소)", effects = new List<EventEffect> { new EventEffect { parameterName = "stress", value = -10 } }, nextDialogueID = "aqua_collab_decline" }
        };
        allDialogues.Add(new DialogueData("rival_aquas_challenge", aquaChallengeLines, aquaChallengeChoices));

        List<DialogueLine> aquaCollabSuccessLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "나레이션", dialogueText = "미나토 아쿠아와의 합방은 대성공이었다!", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("aqua_collab_success", aquaCollabSuccessLines));

        List<DialogueLine> aquaCollabDeclineLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "나레이션", dialogueText = "합방을 거절했지만, 당신은 잠시나마 평화를 얻었다.", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("aqua_collab_decline", aquaCollabDeclineLines));

        // 라이벌 이벤트: 호시마치 스이세이의 견제
        List<DialogueLine> suiseiCheckLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "호시마치 스이세이", dialogueText = "요즘 신인들이 너무 많아서 말이야~ 실력 없는 애들은 금방 사라지던데?", speakerPortrait = null }
        };
        List<DialogueChoice> suiseiCheckChoices = new List<DialogueChoice>
        {
            new DialogueChoice { choiceText = "무시한다 (스트레스 증가)", effects = new List<EventEffect> { new EventEffect { parameterName = "stress", value = 10 } } },
            new DialogueChoice { choiceText = "더 열심히 한다 (스트레스 증가, 능력치 상승)", effects = new List<EventEffect> { new EventEffect { parameterName = "stress", value = 20 }, new EventEffect { parameterName = "singingSkill", value = 5 } } }
        };
        allDialogues.Add(new DialogueData("rival_suiseis_check", suiseiCheckLines, suiseiCheckChoices));

        // 스태프 이벤트: 매니저의 격려
        List<DialogueLine> managerEncouragementLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "김매니저", dialogueText = "정말 잘하고 있어요! 이대로만 가면 분명 최고의 VTuber가 될 거예요!", speakerPortrait = null }
        };
        allDialogues.Add(new DialogueData("manager_encouragement", managerEncouragementLines));

        // 스태프 이벤트: 코디의 제안
        List<DialogueLine> coordinatorSuggestionLines = new List<DialogueLine>
        {
            new DialogueLine { speakerName = "박코디", dialogueText = "새로운 의상 컨셉을 생각해봤는데, 한번 시도해볼까요?", speakerPortrait = null }
        };
        List<DialogueChoice> coordinatorSuggestionChoices = new List<DialogueChoice>
        {
            new DialogueChoice { choiceText = "시도한다 (매력 상승, 자금 감소)", effects = new List<EventEffect> { new EventEffect { parameterName = "charm", value = 5 }, new EventEffect { parameterName = "money", value = -2000 } } },
            new DialogueChoice { choiceText = "나중에 한다 (변화 없음)", effects = new List<EventEffect> { } }
        };
        allDialogues.Add(new DialogueData("coordinator_suggestion", coordinatorSuggestionLines, coordinatorSuggestionChoices));
    }

    public void StartDialogue(string dialogueID)
    {
        DialogueData dialogueToStart = allDialogues.Find(d => d.dialogueID == dialogueID);
        if (dialogueToStart != null)
        {
            OnDialogueStart?.Invoke(dialogueToStart);
            Debug.Log($"Starting dialogue: {dialogueID}");
        }
        else
        {
            Debug.LogWarning($"Dialogue with ID {dialogueID} not found.");
        }
    }

    public void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
        Debug.Log("Dialogue ended.");
    }

    // TODO: 대화 진행 로직 (다음 라인, 선택지 처리 등) 추가
}
