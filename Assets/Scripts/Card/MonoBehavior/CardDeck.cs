using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPosition;                                            // 抽牌出现的位置
    public float motionDelay = 0.2f;                                        // 移动动画前后延迟
    public bool isHorizontal;

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
                //TODO: 洗牌 更新弃牌堆|抽牌堆的数字
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            Card card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardList.Add(card);

            var delayTime = i * motionDelay;
            // 每次抽取卡牌后重新计算布局
            SetCardLayout(delayTime);
        }
    }

    private void SetCardLayout(float delatTime)
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Card currentCard = handCardList[i];
            //TODO: 化简重复计算部分
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardList.Count, isHorizontal);
            // currentCard.transform.SetPositionAndRotation(cardTransform.position, cardTransform.rotation);
            // 放大后开始移动;
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delatTime).onComplete = () =>
            {
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.3f);
                currentCard.transform.DOMove(cardTransform.position, 0.2f);
            };

            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
        }
    }
}
