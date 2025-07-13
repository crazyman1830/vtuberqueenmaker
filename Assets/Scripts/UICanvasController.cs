using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class UICanvasController : MonoBehaviour
{
    [Header("Canvas Scaler Settings")]
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    [Range(0, 1)]
    public float matchWidthOrHeight = 0.5f;

    private CanvasScaler canvasScaler;

    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        SetupCanvasScaler();
    }

    void SetupCanvasScaler()
    {
        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler component not found!");
            return;
        }

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = matchWidthOrHeight;

        Debug.Log($"Canvas Scaler configured. Mode: {canvasScaler.uiScaleMode}, Ref Res: {canvasScaler.referenceResolution}, Match: {canvasScaler.matchWidthOrHeight}");
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        SetupCanvasScaler();
    }
#endif
}