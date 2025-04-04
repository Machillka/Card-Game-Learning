using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestroomPanelController : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton, backToMapButton;

    public Effect restEffect;
    public ObjectEventSO loadMapEvent;

    private CharacterBase player;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restButton.clicked += OnRestButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;


    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnRestButtonClicked()
    {
        restEffect.Excute(player, null);
        restButton.SetEnabled(false);
    }
}
