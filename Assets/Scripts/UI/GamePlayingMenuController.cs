using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayingMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    private VisualElement panel;
    private Button backToGameButton;
    private Button backToMapButton;

    [Header("广播事件")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO backToGameEvent;

    private void Awake()
    {
        panel = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Panel");
        backToGameButton = panel.Q<Button>("BackToGameButton");
        backToMapButton = panel.Q<Button>("BackToMapButton");

        backToGameButton.clicked += BackToGame;
        backToMapButton.clicked += BackToMap;
    }

    public void BackToMap()
    {

    }

    public void BackToGame()
    {

    }
}
