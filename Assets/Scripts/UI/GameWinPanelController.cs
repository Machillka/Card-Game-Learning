using System;
using UnityEngine;
using UnityEngine.UIElements;


public class GameWinPanelController : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button returnToMapButton;

    [Header("广播事件")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;


    private void Awake()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
        returnToMapButton = rootElement.Q<Button>("ReturnToMapButton");

        pickCardButton.clicked += OnPickCardButtonClicked;
        returnToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaiseEvent(null, this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }
}
