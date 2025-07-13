
using UnityEngine;
using UnityEngine.UI;

public class EventCGManager : MonoBehaviour
{
    public Image cgImage;
    public GameObject cgPanel;

    public void Initialize()
    {
        if (cgPanel != null) cgPanel.SetActive(false);
        Debug.Log("EventCGManager initialized.");
    }

    public void ShowCG(Sprite cgSprite)
    {
        if (cgImage != null && cgPanel != null)
        {
            cgImage.sprite = cgSprite;
            cgPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("CG Image or Panel not assigned in EventCGManager.");
        }
    }

    public void HideCG()
    {
        if (cgPanel != null)
        {
            cgPanel.SetActive(false);
        }
    }

    // TODO: CG 전환 효과 (페이드 인/아웃 등) 추가
}
