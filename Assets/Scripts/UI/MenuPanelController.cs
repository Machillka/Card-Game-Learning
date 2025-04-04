using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanelController : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton;

    public ObjectEventSO newGameEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitGameButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked() => Application.Quit();

    private void OnNewGameButtonClicked()
    {
       newGameEvent.RaiseEvent(null, this);
    }
}
