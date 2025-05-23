using System.Collections.Generic;
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

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    public ObjectEventSO playerTurnEndEvent;

    private void Start()
    {
        InitializeDeck();
        // DrawCard(5);
    }

    public void NewTurnDrawCards()
    {
        //TODO[x] 应该是先回复cost 再抽牌 需要重新设置执行顺序
        DrawCard(4);
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

        //TODO[x] 洗牌 更新弃牌堆|抽牌堆的数字
        ShuffleDeck();
    }

    [ContextMenu("DrawCard")]
    public void DrawCardTest()
    {
        DrawCard(1);
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                // TODO[x] 洗牌 更新弃牌堆|抽牌堆的数字
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            // 更新UI 抽牌堆减少
            drawCountEvent.RaiseEvent(drawDeck.Count, this);

            Card card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardList.Add(card);

            var delayTime = i * motionDelay;
            // 每次抽取卡牌后重新计算布局
            SetCardLayout(delayTime);
        }
    }

    /// <summary>
    /// 设置卡牌位置
    /// </summary>
    /// <param name="delatTime">动画延迟事件</param>
    private void SetCardLayout(float delatTime)
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Card currentCard = handCardList[i];
            // TODO: 化简重复计算部分
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardList.Count, isHorizontal);

            // 卡牌能量判断
            currentCard.UpdateCardState();
            // currentCard.isAvaliable = cardCost <= player.CurrentMana;

            // 放大后开始移动;
            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delatTime).onComplete = () =>
            {
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.3f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DOMove(cardTransform.position, 0.2f);
            };

            // 设置卡牌叠放顺序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            // 更新卡牌数据
            currentCard.UpdatePositionRotation(cardTransform.position, cardTransform.rotation);
        }
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        //TODO: 显示UI
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int shuffleIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[shuffleIndex];
            drawDeck[shuffleIndex] = temp;
        }
    }

    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardList.Remove(card);
        cardManager.DiscardCardObject(card.gameObject);

        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        SetCardLayout(0f);
    }

    /// <summary>
    /// 检测手牌数量, 如果没有手牌, 则结束玩家回合
    /// </summary>
    public void CheckHandCard()
    {
        if (handCardList.Count <= 0)
        {
            StartCoroutine(playerTurnEndEvent.DelayRaiseEvent(null, this, 0.7f));
        }
    }

    [ContextMenu("TestUI")]
    public void Test()
    {
        drawCountEvent.RaiseEvent(100, this);
        discardCountEvent.RaiseEvent(114, this);
    }

    /// <summary>
    /// 玩家回合结束，回收卡牌
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        foreach (var card in handCardList)
        {
            // DiscardCard(card);
            discardDeck.Add(card.cardData);
            cardManager.DiscardCardObject(card.gameObject);
        }

        handCardList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardList)
        {
            cardManager.DiscardCardObject(card.gameObject);
        }

        handCardList.Clear();
        InitializeDeck();
    }

}
