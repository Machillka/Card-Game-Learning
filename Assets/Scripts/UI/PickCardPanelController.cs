using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanelController : MonoBehaviour
{
    public CardManager cardManager;

    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;
    private CardDataSO currentCardData;

    private List<Button> cardButtons = new List<Button>();

    private Button comfirmButton;

    [Header("广播事件")]
    public ObjectEventSO finishPickCardEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Panel");
        cardContainer = rootElement.Q<VisualElement>("Container");
        comfirmButton = rootElement.Q<Button>("ConfirmButton");

        comfirmButton.clicked += OnComfirmButtonClicked;

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();

            // 初始化卡牌
            InitializeCard(card, data);

            var cardButton = card.Q<Button>(name: "Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton, data);
        }
    }

    private void OnComfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardEvent.RaiseEvent(null, this);
    }

    private void OnCardClicked(Button cardButton, CardDataSO cardData)
    {
        currentCardData = cardData;
        foreach (var card in cardButtons)
        {
            if (card == cardButton)
            {
                card.SetEnabled(false);
            }
            else
            {
                card.SetEnabled(true);
            }
        }
    }

    private void InitializeCard(VisualElement card, CardDataSO cardData)
    {
        card.dataSource = cardData;

        var cardSpriteElement = card.Q<VisualElement>(name: "CardSprite");
        var cardCost = card.Q<Label>(name: "EnergyCost");
        var cardDescription = card.Q<Label>(name: "CardDescription");
        var cardType = card.Q<Label>(name: "CardType");
        var cardName = card.Q<Label>(name: "CardName");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.cardName;
        cardCost.text = cardData.cardCost.ToString();
        cardDescription.text = cardData.cardDescription;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException()
        };
    }
}
