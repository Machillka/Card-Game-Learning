using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class FadePanel : MonoBehaviour
{
    private VisualElement background;

    private void Awake()
    {
        background = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Background");
        background.style.opacity = 0f;
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0f, 1f, duration, value =>
            {
                background.style.opacity = value;
            }).SetEase(Ease.OutCubic);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1f, 0f, duration, value =>
            {
                background.style.opacity = value;
            }).SetEase(Ease.OutCubic);
    }
}
