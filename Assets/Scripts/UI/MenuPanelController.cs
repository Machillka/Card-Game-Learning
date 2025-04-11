using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanelController : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton;

    public ObjectEventSO newGameEvent;

    // 回到开始界面
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitGameButton");

        // UI 设置
        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;

    }

#if UNITY_EDITOR
    private void OnQuitButtonClicked() => UnityEditor.EditorApplication.isPlaying = false;
#else
    private void OnQuitButtonClicked() => Application.Quit();
#endif

    // 广播新游戏开始事件
    private void OnNewGameButtonClicked()
    {
       newGameEvent.RaiseEvent(null, this);
    }
}
