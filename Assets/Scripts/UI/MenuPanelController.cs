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

#if UNITY_EDITOR
    private void OnQuitButtonClicked() => UnityEditor.EditorApplication.isPlaying = false;
#else
    private void OnQuitButtonClicked() => Application.Quit();
#endif

    private void OnNewGameButtonClicked()
    {
       newGameEvent.RaiseEvent(null, this);
    }
}
