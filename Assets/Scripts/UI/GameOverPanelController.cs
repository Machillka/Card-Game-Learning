using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanelController : MonoBehaviour
{
    private Button backToStartButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        // GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").RegisterCallback<ClickEvent>(OnBackToStartButtonClicked);
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStart").clicked += OnBackToStartButtonClicked;
    }

    private void OnBackToStartButtonClicked()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
