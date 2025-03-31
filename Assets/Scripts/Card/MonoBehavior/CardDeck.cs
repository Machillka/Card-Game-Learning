using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;

    private List<CardDataSO> drawDeck = new List<CardDataSO>();             // 抽牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>();          // 弃牌堆
    private List<Card> handCardList = new List<Card>();                     // 当前手牌

    private void Start()
    {
        InitializeDeck();
        DrawCard(3);
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();
        foreach (var cardLibraryEntry in cardManager.currnetCardLibrary.cardLibraryList)
        {
            // 因为同一种卡牌可能有多张，所以根据amount来添加对应个数的卡牌
            for (int i = 0; i < cardLibraryEntry.amount; i++)
            {
                drawDeck.Add(cardLibraryEntry.cardData);
            }
        }

        //TODO: 洗牌 更新弃牌堆|抽牌堆的数字
    }

    [ContextMenu("DrawCard")]
    public void DrawCard()
    {
        DrawCard(1);
    }

    private void DrawCard(int amount)
    {
        if (amount > drawDeck.Count)
        {
            Debug.LogError("DrawCard amount is more than drawDeck count");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                break;
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            Card card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            handCardList.Add(card);
            // 每次抽取卡牌后重新计算布局
            SetCardLayout();
        }
    }

    private void SetCardLayout()
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Card currentCard = handCardList[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardList.Count, true);
            currentCard.transform.SetPositionAndRotation(cardTransform.position, cardTransform.rotation);
        }
    }
}
