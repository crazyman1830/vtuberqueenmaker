
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;
    public Sprite[] backgroundSprites; // Unity 에디터에서 ActivityType 순서대로 스프라이트 할당

    public void Initialize()
    {
        if (backgroundRenderer == null)
        {
            // 씬에 SpriteRenderer가 없으면 추가
            backgroundRenderer = gameObject.AddComponent<SpriteRenderer>();
            backgroundRenderer.sortingOrder = -10; // 배경이 다른 UI나 캐릭터 뒤에 오도록 설정
        }
        Debug.Log("BackgroundManager initialized.");
    }

    public void SetBackground(Sprite newBackgroundSprite)
    {
        if (backgroundRenderer != null)
        {
            backgroundRenderer.sprite = newBackgroundSprite;
        }
        else
        {
            Debug.LogWarning("BackgroundRenderer is not assigned or found.");
        }
    }

    public Sprite GetBackgroundSprite(ActivityType activityType)
    {
        int index = (int)activityType;
        if (index >= 0 && index < backgroundSprites.Length)
        {
            return backgroundSprites[index];
        }
        Debug.LogWarning($"No background sprite found for activity type: {activityType}");
        return null;
    }

    // TODO: 배경 전환 효과 (페이드 인/아웃 등) 추가
}
