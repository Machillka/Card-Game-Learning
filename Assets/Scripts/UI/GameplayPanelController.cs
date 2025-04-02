using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanelController : MonoBehaviour
{
    private VisualElement rootElement;

    private Label energyAmountLabel, drawAmountLabel, discardAmountLabel, turnLabel;
    private Button endTurnButton;

    [Header("广播事件")]
    public ObjectEventSO playerTurnEndEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        // 绑定元素
        energyAmountLabel = rootElement.Q<Label>("EnergyAmountLabel");
        drawAmountLabel = rootElement.Q<Label>("DrawAmountLabel");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmountLabel");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        endTurnButton = rootElement.Q<Button>("EndTurn");

        endTurnButton.clicked += OnEndPlayerTurn;

        energyAmountLabel.text = "666";
        discardAmountLabel.text = "666";
        drawAmountLabel.text = "666";
        turnLabel.text = "游戏开始";
    }

    public void UpdateDrawCountChange(int value)
    {
        drawAmountLabel.text = value.ToString();
    }

    public void UpdateDiscardCountChange(int value)
    {
        discardAmountLabel.text = value.ToString();
    }

    private void OnEndPlayerTurn()
    {
        playerTurnEndEvent.RaiseEvent(null, this);
    }
}
