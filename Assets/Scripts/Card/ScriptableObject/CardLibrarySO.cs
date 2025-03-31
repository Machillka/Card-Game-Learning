using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardLibrarySO", menuName = "Card/CardLibrarySO", order = 0)]
public class CardLibrarySO : ScriptableObject
{
    [TextArea]
    public string description;          // 对当前卡牌库的描述

    public List<CardLibraryEntry> cardLibraryList = new List<CardLibraryEntry>();
}

// 定义每张卡牌以及数量
[System.Serializable]
public class CardLibraryEntry
{
    public CardDataSO cardData;
    public int amount;
}