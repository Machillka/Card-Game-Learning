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

        energyAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        drawAmountLabel.text = "0";
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

    public void UpdateEnergyAmount(int value)
    {
        energyAmountLabel.text = value.ToString();
    }

    private void OnEndPlayerTurn()
    {
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌方回合";
        turnLabel.style.color = new StyleColor(Color.red);
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = new StyleColor(Color.white);
    }
}
