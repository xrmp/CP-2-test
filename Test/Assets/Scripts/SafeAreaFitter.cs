using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect currentSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation currentOrientation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        // Проверяем изменения safe area или ориентации
        if (currentSafeArea != Screen.safeArea || currentOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        currentSafeArea = Screen.safeArea;
        currentOrientation = Screen.orientation;

        // Конвертируем safe area из пикселей в относительные координаты (0-1)
        Vector2 anchorMin = currentSafeArea.position;
        Vector2 anchorMax = currentSafeArea.position + currentSafeArea.size;

        // Защита от деления на ноль
        float screenWidth = Mathf.Max(Screen.width, 1);
        float screenHeight = Mathf.Max(Screen.height, 1);

        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;

        // Применяем к RectTransform
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Логируем информацию о safe area
        Debug.Log($"[SafeArea] Orientation: {GetOrientationString(Screen.orientation)}, " +
                  $"Safe Area: {currentSafeArea}, " +
                  $"Anchors: ({anchorMin.x:F3}, {anchorMin.y:F3}) - ({anchorMax.x:F3}, {anchorMax.y:F3})");
    }

    private string GetOrientationString(ScreenOrientation orientation)
    {
        switch (orientation)
        {
            case ScreenOrientation.Portrait:
                return "Portrait";
            case ScreenOrientation.PortraitUpsideDown:
                return "PortraitUpsideDown";
            case ScreenOrientation.LandscapeLeft:
                return "LandscapeLeft";
            case ScreenOrientation.LandscapeRight:
                return "LandscapeRight";
            case ScreenOrientation.AutoRotation:
                return "AutoRotation";
            default:
                return orientation.ToString();
        }
    }
}